using System;

namespace Bacchus.Models {

    public class Product {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
		public DateTime BiddingEndDate { get; set; }
		public decimal HighestBid { get; set; }
	}
}
