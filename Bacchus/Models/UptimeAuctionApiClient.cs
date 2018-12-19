using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bacchus.Models
{
	public class UptimeAuctionApiClient : IUptimeAuctionApiClient
	{
		private readonly HttpClient _client;

		public UptimeAuctionApiClient( HttpClient httpClient )
		{
			httpClient.BaseAddress = new Uri( "http://uptime-auction-api.azurewebsites.net/api/Auction/" );
			//httpClient.DefaultRequestHeaders.Add( "Accept", "application/vnd.UptimeAuctionApi+json" );
			//httpClient.DefaultRequestHeaders.Add( "User-Agent", "HttpClientFactory-UptimeAuctionApi" );
			_client = httpClient;
		}

		public async Task<List<Product>> GetProducts()
		{
			return await _client.GetAsync( "http://uptime-auction-api.azurewebsites.net/api/Auction/" ).Result.Content.ReadAsAsync<List<Product>>();
		}
	}
}
