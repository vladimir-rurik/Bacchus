using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Bacchus.Models;
using System;

namespace Bacchus.Components {

    public class NavigationMenuViewComponent : ViewComponent {
        private IProductRepository repository;

        public NavigationMenuViewComponent(IProductRepository repo) {
            repository = repo;
        }

        public IViewComponentResult Invoke() {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View( repository.Products
				.Where( x => x.BiddingEndDate > DateTime.Now.ToUniversalTime() )
                .Select( x => x.ProductCategory )
                .Distinct()
                .OrderBy( x => x ) 
				);
        }
    }
}
