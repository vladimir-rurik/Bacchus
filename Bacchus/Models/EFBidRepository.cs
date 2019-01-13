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
			bid.BiddingDateTime = DateTime.UtcNow;
			bid.BidderId = $"{bid.BidderFirstName}.{bid.BidderLastName}.{bid.BiddingDateTime.ToString("yyyyMMddHHmmss.fff")}";

			_dbContext.AddRange( bid );

			_dbContext.SaveChanges();
		}
	}
}
