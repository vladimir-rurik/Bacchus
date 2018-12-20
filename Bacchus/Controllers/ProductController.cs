using Microsoft.AspNetCore.Mvc;
using Bacchus.Models;
using System.Linq;
using Bacchus.Models.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Bacchus.Controllers {

    public class ProductController : Controller {
		private readonly IUptimeAuctionApiClient _uptimeAuctionApiClient;
        private IProductRepository _repository;

        public const int PAGESIZE = 4;

        public ProductController(IProductRepository repo, IUptimeAuctionApiClient uptimeAuctionApiClient) {
            _repository = repo;
			_uptimeAuctionApiClient = uptimeAuctionApiClient;
        }

        public async Task<ViewResult> List(string category, int productPage = 1)
		{
			List<Product> products = await _uptimeAuctionApiClient.GetProducts();

			_repository.UpsertProducts( products );

			ProductsListViewModel productsListViewModel = new ProductsListViewModel
			{
				Products = _repository.Products
					.Where( p => ( category == null || p.ProductCategory == category ) && p.BiddingEndDate > DateTime.Now.ToUniversalTime() )
					.OrderBy( p => p.ProductID )
					.Skip( ( productPage - 1 ) * PAGESIZE )
					.Take( PAGESIZE ),
				PagingInfo = new PagingInfo
				{
					CurrentPage = productPage,
					ItemsPerPage = PAGESIZE,
					TotalItems = category == null ?
						_repository.Products.Where( e =>
							 e.BiddingEndDate > DateTime.Now.ToUniversalTime() ).Count() :
						_repository.Products.Where( e =>
							 e.ProductCategory == category && e.BiddingEndDate > DateTime.Now.ToUniversalTime() ).Count()
				},
				CurrentCategory = category
			};

			return View( productsListViewModel );
		}

		[HttpGet]
		public async Task<ActionResult> GetProducts()
		{
			List<Product> result = await _uptimeAuctionApiClient.GetProducts();
			return Ok( result );
		}
	}
}
