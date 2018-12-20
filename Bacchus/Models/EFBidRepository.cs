using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bacchus.Models
{
	public class EFBidRepository : IBidRepository
	{
		private ApplicationDbContext _dbContext;

		public EFBidRepository( ApplicationDbContext ctx )
		{
			_dbContext = ctx;
		}

		public void SaveBid( Bid bid )
		{
			bid.BiddingDateTime = DateTime.Now.ToUniversalTime();

			if( _dbContext.Bids.Any( ac => ac.ProductId.Equals( bid.ProductId ) ) )
			{
				_dbContext.UpdateRange( bid );
			}
			else
			{
				_dbContext.AddRange( bid );
			}

			_dbContext.SaveChanges();
		}
	}
}
