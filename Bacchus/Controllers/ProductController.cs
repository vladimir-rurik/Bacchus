using Microsoft.AspNetCore.Mvc;
using Bacchus.Models;
using System.Linq;
using Bacchus.Models.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Bacchus.Controllers {

    public class ProductController : Controller {
		private readonly IUptimeAuctionApiClient _uptimeAuctionApiClient;
        private IProductRepository _repository;
        public int PageSize = 4;

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
					.Where( p => category == null || p.ProductCategory == category )
					.OrderBy( p => p.ProductID )
					.Skip( ( productPage - 1 ) * PageSize )
					.Take( PageSize ),
				PagingInfo = new PagingInfo
				{
					CurrentPage = productPage,
					ItemsPerPage = PageSize,
					TotalItems = category == null ?
						_repository.Products.Count() :
						_repository.Products.Where( e =>
							 e.ProductCategory == category ).Count()
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
