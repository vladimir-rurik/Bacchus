Introduction

Create a web application, “Bacchus”, in which you can see the current auctions and make a bid.


Task description and requirements

“Bacchus” should show all running auctions on the first open page. In addition, a user could place bids on a chosen auction. For the bid, a user has to enter their full name and a bid amount in euros. In this task, a unique bidding user ID would be the full name + date + time. A user could indeed place a bid, however they couldn’t see other user’s bids nor the high bid at the moment, so the bid offer happens unaware of other placed bids on other user’s pages. A user should see a product name, short description, and time left ( additional information about the last parameter is in the next paragraph ).

At this very moment, auction products could be queried from a web address http://uptime-auction-api.asurewebsites.net/api/Auction (REST API). Each product has a “biddingEndDate” which means how long this product is in an active state, in other words, how long users could place bids. After a product’s “biddingEndDate” has expired the API product is no longer offered and instead of the previous product, a new product appears. Users’ bids should be bound with a product ID that API provides by “productId” name.

In addition, the application should present dynamic menus that work like a filter to show/hide products by category. Each menu item is a category ( API name: “productCategory” ) and shows/hides products under this category. The menu should not contain categories that don’t have any products.If a category menu item is chosen the products beyond the category are hidden. On the menu filter, there should be a “reset” option which clears the chosen category and shows all active product offers.

After an auction is finished, the winner’s name should not be shown, but their results with bidding histories should be saved


Additional information

Present the resolved task with a short step by step explanation or installation description of how the application could be run.
