using System.Collections.Generic;
using System.Linq;
using Moq;
using Bacchus.Controllers;
using Bacchus.Models;
using Xunit;
using Bacchus.Models.ViewModels;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Bacchus.Tests {

    public class ProductControllerTests {

        [Fact]
        public void Can_Paginate() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = "1", ProductName = "P1"},
                new Product {ProductID = "2", ProductName = "P2"},
                new Product {ProductID = "3", ProductName = "P3"},
                new Product {ProductID = "4", ProductName = "P4"},
                new Product {ProductID = "5", ProductName = "P5"}
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            ProductsListViewModel result =
                controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            // Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].ProductName);
            Assert.Equal("P5", prodArray[1].ProductName);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model() {

            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
				new Product {ProductID = "1", ProductName = "P1"},
				new Product {ProductID = "2", ProductName = "P2"},
				new Product {ProductID = "3", ProductName = "P3"},
				new Product {ProductID = "4", ProductName = "P4"},
				new Product {ProductID = "5", ProductName = "P5"}
			} ).AsQueryable<Product>());

            // Arrange
            ProductController controller =
                new ProductController(mock.Object) { PageSize = 3 };

            // Act
            ProductsListViewModel result =
                controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products() {

            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = "1", ProductName = "P1", ProductCategory = "Cat1"},
                new Product {ProductID = "2", ProductName = "P2", ProductCategory = "Cat2"},
                new Product {ProductID = "3", ProductName = "P3", ProductCategory = "Cat1"},
                new Product {ProductID = "4", ProductName = "P4", ProductCategory = "Cat2"},
                new Product {ProductID = "5", ProductName = "P5", ProductCategory = "Cat3"}
            }).AsQueryable<Product>());

            // Arrange - create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Action
            Product[] result =
                (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel)
                    .Products.ToArray();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].ProductName == "P2" && result[0].ProductCategory == "Cat2");
            Assert.True(result[1].ProductName == "P4" && result[1].ProductCategory == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
				new Product {ProductID = "1", ProductName = "P1", ProductCategory = "Cat1"},
				new Product {ProductID = "2", ProductName = "P2", ProductCategory = "Cat2"},
				new Product {ProductID = "3", ProductName = "P3", ProductCategory = "Cat1"},
				new Product {ProductID = "4", ProductName = "P4", ProductCategory = "Cat2"},
				new Product {ProductID = "5", ProductName = "P5", ProductCategory = "Cat3"}
			} ).AsQueryable<Product>());

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            Func<ViewResult, ProductsListViewModel> GetModel = result =>
                result?.ViewData?.Model as ProductsListViewModel;

            // Action
            int? res1 = GetModel(target.List("Cat1"))?.PagingInfo.TotalItems;
            int? res2 = GetModel(target.List("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = GetModel(target.List("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = GetModel(target.List(null))?.PagingInfo.TotalItems;

            // Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }



    }
}
