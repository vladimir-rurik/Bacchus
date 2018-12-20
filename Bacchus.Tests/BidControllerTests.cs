using Microsoft.AspNetCore.Mvc;
using Moq;
using Bacchus.Controllers;
using Bacchus.Models;
using Xunit;
using System;

namespace Bacchus.Tests
{
	public class BidControllerTests
	{
		[Fact]
		public void Cannot_Place_Empty_Bid()
		{
			// Arrange - create a mock repository
			Mock<IBidRepository> mock = new Mock<IBidRepository>();
			// Arrange - create an empty bid
			Bid bid = new Bid();
			// Arrange - create an instance of the controller
			BidController target = new BidController( mock.Object );

			// Act
			ViewResult result = target.PlaceBid( bid ) as ViewResult;

			// Assert - check that the method is returning the default view
			Assert.True( string.IsNullOrEmpty( result?.ViewName ) );
			// Assert - check that I am passing an invalid model to the view
			bool isValid = result?.ViewData?.ModelState?.IsValid ?? false;
			Assert.False( isValid );
		}

		[Fact]
		public void Cannot_Place_Invalid_Bid()
		{

			// Arrange - create a mock repository
			Mock<IBidRepository> mock = new Mock<IBidRepository>();
			// Arrange - create a bid on a product
			Bid bid = new Bid();
			bid.ProductId = "1";
			bid.ProductName = "Name1";

			// Arrange - create an instance of the controller
			BidController target = new BidController( mock.Object );
			// Arrange - add an error to the model
			target.ModelState.AddModelError( "error", "error" );

			// Act - try to make the bid
			ViewResult result = target.PlaceBid( bid ) as ViewResult;

			// Assert - check that the bid hasn't been passed stored
			mock.Verify( m => m.SaveBid( It.IsAny<Bid>() ), Times.Never );
			// Assert - check that the method is returning the default view
			Assert.True( string.IsNullOrEmpty( result.ViewName ) );
			// Assert - check that I am passing an invalid model to the view
			Assert.False( result.ViewData.ModelState.IsValid );
		}

		[Fact]
		public void Can_Place_Bid()
		{
			// Arrange - create a mock bid repository
			Mock<IBidRepository> mock = new Mock<IBidRepository>();
			// Arrange - create a bid
			Bid bid = new Bid()
			{
				BidderFirstName = "Name1",
				BidderLastName = "Name2",
				BiddingDateTime = DateTime.Now,
				BidderBid = 22.23,
			};

			// Arrange - create an instance of the controller
			BidController target = new BidController( mock.Object );

			// Act - try to bid
			RedirectToActionResult result =
				 target.PlaceBid( bid ) as RedirectToActionResult;

			// Assert - check that the bid has been stored
			mock.Verify( m => m.SaveBid( It.IsAny<Bid>() ), Times.Once );
			// Assert - check that the method is redirecting to the Product List action
			Assert.Equal( "List", result.ActionName );
		}
	}
}
