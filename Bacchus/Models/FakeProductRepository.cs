using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bacchus.Models
{
	public class FakeProductRepository : IProductRepository
	{
		public IQueryable<Product> Products => new List<Product> { 
			new Product { ProductName = "Foot ball", ProductCategory = "Sports", ProductDescription = "Kickass foot ball", BiddingEndDate = DateTime.Now },
			new Product { ProductName = "Surf board", ProductCategory = "Sports", ProductDescription = "Kickass surf board", BiddingEndDate = DateTime.Now.AddMinutes(1) },
			new Product { ProductName = "Running shoes", ProductCategory = "Clothing", ProductDescription = "Nike running shoes", BiddingEndDate = DateTime.Now.AddHours(2) }
		}.AsQueryable<Product>();

		public void UpsertProducts( List<Product> products )
		{
			throw new NotImplementedException();
		}
	}
}
