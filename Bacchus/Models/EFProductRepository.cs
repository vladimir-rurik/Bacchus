using System.Collections.Generic;
using System.Linq;

namespace Bacchus.Models {

	public class EFProductRepository : IProductRepository {
		private ApplicationDbContext _context;

		public EFProductRepository( ApplicationDbContext ctx ) {
			_context = ctx;
		}

		public IQueryable<Product> Products => _context.Products;

		public void UpsertProducts( List<Product> products )
		{
			foreach( Product product in products )
			{
				UpsertProduct( product );
			}
			_context.SaveChanges();
		}

		private void UpsertProduct( Product product )
		{
			if( string.IsNullOrEmpty( product.ProductID ) )
			{
				_context.Products.Add( product );
			}
			else
			{
				Product dbEntry = _context.Products
					.FirstOrDefault( p => p.ProductID == product.ProductID );
				if( dbEntry != null )
				{
					dbEntry.ProductName = product.ProductName;
					dbEntry.ProductDescription = product.ProductDescription;
					dbEntry.ProductCategory = product.ProductCategory;
					dbEntry.BiddingEndDate = product.BiddingEndDate;
				}
			}
		}
	}
}
