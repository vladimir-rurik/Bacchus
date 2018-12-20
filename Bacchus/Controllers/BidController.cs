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
		private IBidRepository _repository;

		public BidController( IBidRepository repo )
		{
			_repository = repo;
		}

		public ViewResult PlaceBid( string ProductID, string ProductName )
        {
            return View( new Bid() { ProductName = ProductName } );
        }

		[HttpPost]
		public IActionResult PlaceBid( Bid bid )
		{
			if( ModelState.IsValid )
			{
				_repository.SaveBid( bid );

				return RedirectToAction( "List", "Product" );
			}
			else
			{
				return View( bid );
			}
		}
	}
}