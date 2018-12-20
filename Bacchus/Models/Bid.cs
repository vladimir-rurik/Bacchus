using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ExpressiveAnnotations.Attributes;

namespace Bacchus.Models
{
	public class Bid
	{
		[Key]
		public string BidderId { get; set; }

		[ForeignKey( "ProductFK" )]
		public string ProductId { get; set; }

		[NotMapped]
		public string ProductName { get; set; }

		[NotMapped]
		[Required(ErrorMessage = "Please enter your first name.")]
		public string BidderFirstName { get; set; }

		[NotMapped]
		[Required(ErrorMessage = "Please enter your last name.")]
		public string BidderLastName { get; set; }

		[AssertThat( "BidderBid > 0", ErrorMessage = "Please make your bid greater than zero.")]
		public int BidderBid { get; set; }

		[NotMapped]
		public DateTime BiddingDateTime { get; set; }
	}
}
