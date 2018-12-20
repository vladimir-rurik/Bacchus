using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ExpressiveAnnotations.Attributes;

namespace Bacchus.Models
{
	public class Bid
	{
		[Key]
		public string ProductId { get; set; }
		public string ProductName { get; set; }
		[Required(ErrorMessage = "Please enter your first name.")]
		public string BidderFirstName { get; set; }
		[Required(ErrorMessage = "Please enter your last name.")]
		public string BidderLastName { get; set; }
		[AssertThat( "BidderBid > 0", ErrorMessage = "Please make your bid greater than zero.")]
		public int BidderBid { get; set; }
		public DateTime BiddingDateTime { get; set; }
	}
}
