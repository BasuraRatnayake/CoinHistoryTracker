using RestSharp;
using Newtonsoft.Json.Linq;

namespace CoinTrackerHistory.API.Services.ThirdParty;

public static class CurrencyService {
	public static async Task<decimal> GetCryptoCoinPrice(string FromCoin, string ToCoin) {
		try {
			RestClient client = new($"https://api.coingate.com");
			RestRequest request = new($"api/v2/rates/merchant/{FromCoin}/{ToCoin}");
			RestResponse response = await client.GetAsync(request);

			if (string.IsNullOrWhiteSpace(response.Content))
				throw new Exception("No Data Received From API");

			return decimal.Parse(response.Content);
		} catch (Exception) {
			throw;
		}
	}

	public static async Task<decimal> GetLKRPrice() {
		try {
			RestClient client = new RestClient("https://economia.awesomeapi.com.br");
			RestRequest request = new RestRequest($"json/last/USD-LKR");
			RestResponse response = await client.GetAsync(request);

			if (string.IsNullOrWhiteSpace(response.Content))
				throw new Exception("No Data Received From API");

			JObject obj = JObject.Parse(response.Content);
			if (obj == null)
				throw new Exception("No Data Received From API (fawazahmed0)");

			return (decimal) obj["USDLKR"]["low"];
		} catch (Exception) {
			throw;
		}
	}
}
