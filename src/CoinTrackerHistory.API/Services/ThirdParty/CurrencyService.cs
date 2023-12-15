using System;
using Amazon.Runtime;
using System.Threading;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CoinTrackerHistory.API.Services.ThirdParty;

public static class CurrencyService {
    public static async Task<decimal> GetCryptoCoinPrice(string FromCoin, string ToCoin) {
        try {
            RestClientOptions options = new RestClientOptions($"https://api.coingate.com/api/v2/rates/merchant/{FromCoin}/{ToCoin}");
            RestClient client = new RestClient(options);
            RestRequest request = new RestRequest("");
            request.AddHeader("accept", "text/plain");
            RestResponse response = await client.GetAsync(request);

            if (string.IsNullOrWhiteSpace(response.Content))
                throw new Exception("No Data Received From API (CryptoCompare)");

            return decimal.Parse(response.Content);
        } catch (Exception ex) {
            throw;
        } 
    }

    public static async Task<decimal> GetLKRPrice() {
        try {
            RestClient client = new RestClient("https://economia.awesomeapi.com.br");
            RestRequest request = new RestRequest($"json/last/USD-LKR");
            RestResponse resonse = await client.GetAsync(request);

            JObject obj = JObject.Parse(resonse.Content);
            if (obj == null)
                throw new Exception("No Data Received From API (fawazahmed0)");

            return (decimal) obj["USDLKR"]["low"];
        } catch (Exception ex) {
            throw;
        }
    }
}
