using System;

namespace Bacchus.Models {

    public class Product {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
		public DateTime BiddingEndDate { get; set; }
	}
}
