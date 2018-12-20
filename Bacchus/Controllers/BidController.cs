using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bacchus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bacchus.Controllers
{
    public class BidController : Controller
    {
        public ViewResult PlaceBid()
        {
            return View( new Bid() );
        }

		[HttpPost]
		public IActionResult PlaceBid( Bid bid )
		{
			if( ModelState.IsValid )
			{
				//repository.SaveBid( bid );
				return RedirectToAction( "List", "Product" );
			}
			else
			{
				return View( bid );
			}
		}
	}
}