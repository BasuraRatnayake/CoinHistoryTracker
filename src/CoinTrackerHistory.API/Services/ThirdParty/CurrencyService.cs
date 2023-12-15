using RestSharp;
using Newtonsoft.Json.Linq;
using CoinTrackerHistory.API.Exceptions;

namespace CoinTrackerHistory.API.Services.ThirdParty;

public static class CurrencyService {
	private static async Task<string> GetAPIResponse(string domain, string endpoint) {
		try {
			RestClient client = new(domain);
			RestRequest request = new(endpoint);
			RestResponse response = await client.GetAsync(request);

			if (string.IsNullOrWhiteSpace(response.Content))
				throw new InternalServerException();

			return response.Content;
		} catch (InternalServerException) {
			throw;
		}
	}

	public static async Task<decimal> GetCryptoCoinPrice(string FromCoin, string ToCoin) {
		try {
			string response = await GetAPIResponse("https://api.coingate.com", $"api/v2/rates/merchant/{FromCoin}/{ToCoin}");
			return decimal.Parse(response);
		} catch (InternalServerException) {
			throw;
		}
	}

	public static async Task<decimal> GetLKRPrice() {
		try {
			string response = await GetAPIResponse("https://economia.awesomeapi.com.br", $"json/last/USD-LKR");

			JObject obj = JObject.Parse(response);
			if (obj == null)
				throw new InternalServerException();

			return (decimal) obj["USDLKR"]["low"];
		} catch (InternalServerException) {
			throw;
		}
	}
}
