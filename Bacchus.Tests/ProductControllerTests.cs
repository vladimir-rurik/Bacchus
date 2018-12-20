using System.Collections.Generic;
using System.Linq;
using Moq;
using Bacchus.Controllers;
using Bacchus.Models;
using Xunit;
using Bacchus.Models.ViewModels;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bacchus.Tests {

    public class ProductControllerTests {

        [Fact]
        public async Task Can_PaginateAsync() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
			Mock<IUptimeAuctionApiClient> mock_http_client = new Mock<IUptimeAuctionApiClient>();

            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = "1", ProductName = "P1", BiddingEndDate = DateTime.Now.AddHours(3)},
                new Product {ProductId = "2", ProductName = "P2", BiddingEndDate = DateTime.Now.AddHours(3)},
                new Product {ProductId = "3", ProductName = "P3", BiddingEndDate = DateTime.Now.AddHours(3)},
                new Product {ProductId = "4", ProductName = "P4", BiddingEndDate = DateTime.Now.AddHours(3)},
                new Product {ProductId = "5", ProductName = "P5", BiddingEndDate = DateTime.Now.AddHours(3)}
			} ).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object, mock_http_client.Object);
            controller.PAGESIZE = 3;

			// Act
			ViewResult oViewResult = await controller.List( null, 2 ) as ViewResult;
			ProductsListViewModel result = oViewResult.ViewData.Model as ProductsListViewModel;

            // Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].ProductName);
            Assert.Equal("P5", prodArray[1].ProductName);
        }

        [Fact]
        public async Task Can_Send_Pagination_View_ModelAsync() {

            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
			Mock<IUptimeAuctionApiClient> mock_http_client = new Mock<IUptimeAuctionApiClient>();

			mock.Setup(m => m.Products).Returns((new Product[] {
				new Product {ProductId = "1", ProductName = "P1", BiddingEndDate = DateTime.Now.AddHours(3)},
				new Product {ProductId = "2", ProductName = "P2", BiddingEndDate = DateTime.Now.AddHours(3)},
				new Product {ProductId = "3", ProductName = "P3", BiddingEndDate = DateTime.Now.AddHours(3)},
				new Product {ProductId = "4", ProductName = "P4", BiddingEndDate = DateTime.Now.AddHours(3)},
				new Product {ProductId = "5", ProductName = "P5", BiddingEndDate = DateTime.Now.AddHours(3)}
			} ).AsQueryable<Product>());

            // Arrange
            ProductController controller =
                new ProductController(mock.Object, mock_http_client.Object ) { PAGESIZE = 3 };

            // Act
			ViewResult oViewResult = await controller.List( null, 2 ) as ViewResult;
			ProductsListViewModel result = oViewResult.ViewData.Model as ProductsListViewModel;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public async Task Can_Filter_ProductsAsync() {

            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
			Mock<IUptimeAuctionApiClient> mock_http_client = new Mock<IUptimeAuctionApiClient>();

			mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = "1", ProductName = "P1", ProductCategory = "Cat1", BiddingEndDate = DateTime.Now.AddHours(3)},
                new Product {ProductId = "2", ProductName = "P2", ProductCategory = "Cat2", BiddingEndDate = DateTime.Now.AddHours(3)},
                new Product {ProductId = "3", ProductName = "P3", ProductCategory = "Cat1", BiddingEndDate = DateTime.Now.AddHours(3)},
                new Product {ProductId = "4", ProductName = "P4", ProductCategory = "Cat2", BiddingEndDate = DateTime.Now.AddHours(3)},
                new Product {ProductId = "5", ProductName = "P5", ProductCategory = "Cat3", BiddingEndDate = DateTime.Now.AddHours(3)}
			} ).AsQueryable<Product>());

			// Arrange - create a controller and make the page size 3 items
			ProductController controller = new ProductController( mock.Object, mock_http_client.Object );
			controller.PAGESIZE = 3;

			// Action
			ViewResult oViewResult = await controller.List( "Cat2", 1 ) as ViewResult;
			Product[] result = (oViewResult.ViewData.Model as ProductsListViewModel).Products.ToArray();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].ProductName == "P2" && result[0].ProductCategory == "Cat2");
            Assert.True(result[1].ProductName == "P4" && result[1].ProductCategory == "Cat2");
        }

        [Fact]
        public async Task Generate_Category_Specific_Product_CountAsync() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
			Mock<IUptimeAuctionApiClient> mock_http_client = new Mock<IUptimeAuctionApiClient>();

			mock.Setup(m => m.Products).Returns((new Product[] {
				new Product {ProductId = "1", ProductName = "P1", ProductCategory = "Cat1", BiddingEndDate = DateTime.Now.AddHours(3)},
				new Product {ProductId = "2", ProductName = "P2", ProductCategory = "Cat2", BiddingEndDate = DateTime.Now.AddHours(3)},
				new Product {ProductId = "3", ProductName = "P3", ProductCategory = "Cat1", BiddingEndDate = DateTime.Now.AddHours(3)},
				new Product {ProductId = "4", ProductName = "P4", ProductCategory = "Cat2", BiddingEndDate = DateTime.Now.AddHours(3)},
				new Product {ProductId = "5", ProductName = "P5", ProductCategory = "Cat3", BiddingEndDate = DateTime.Now.AddHours(3)}
			} ).AsQueryable<Product>());

			ProductController target = new ProductController( mock.Object, mock_http_client.Object );
            target.PAGESIZE = 3;

            Func<ViewResult, ProductsListViewModel> GetModel = result =>
                result?.ViewData?.Model as ProductsListViewModel;

			// Action
			ViewResult oViewResult1 = await target.List( "Cat1" ) as ViewResult;
			int? res1 = GetModel( oViewResult1 )?.PagingInfo.TotalItems;

			ViewResult oViewResult2 = await target.List( "Cat2" ) as ViewResult;
			int? res2 = GetModel( oViewResult2 )?.PagingInfo.TotalItems;

			ViewResult oViewResult3 = await target.List( "Cat3" ) as ViewResult;
			int? res3 = GetModel( oViewResult3 )?.PagingInfo.TotalItems;

			ViewResult oViewResultAll = await target.List( null ) as ViewResult;
            int? resAll = GetModel(oViewResultAll)?.PagingInfo.TotalItems;

            // Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }



    }
}
