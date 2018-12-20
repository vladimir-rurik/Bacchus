using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using Bacchus.Components;
using Bacchus.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace Bacchus.Tests {

    public class NavigationMenuViewComponentTests {

        [Fact]
        public void Can_Select_Categories() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = "1", ProductName = "P1", ProductCategory = "Apples"},
                new Product {ProductId = "2", ProductName = "P2", ProductCategory = "Apples"},
                new Product {ProductId = "3", ProductName = "P3", ProductCategory = "Plums"},
                new Product {ProductId = "4", ProductName = "P4", ProductCategory = "Oranges"},
            }).AsQueryable<Product>());

            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);

            // Act = get the set of categories
            string[] results = ((IEnumerable<string>)(target.Invoke()
                as ViewViewComponentResult).ViewData.Model).ToArray();

            // Assert
            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples",
                "Oranges", "Plums" }, results));
        }

        [Fact]
        public void Indicates_Selected_Category() {

            // Arrange
            string categoryToSelect = "Apples";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = "1", ProductName = "P1", ProductCategory = "Apples"},
                new Product {ProductId = "4", ProductName = "P2", ProductCategory = "Oranges"},
            }).AsQueryable<Product>());
            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext {
                ViewContext = new ViewContext {
                    RouteData = new RouteData()
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;

            // Action
            string result = (string)(target.Invoke() as
                ViewViewComponentResult).ViewData["SelectedCategory"];

            // Assert
            Assert.Equal(categoryToSelect, result);
        }

    }
}
