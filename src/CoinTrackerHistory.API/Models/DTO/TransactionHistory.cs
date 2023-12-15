using CoinTrackerHistory.API.Services.ThirdParty;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoinTrackerHistory.API.Models.DTO;

public enum PurchaseType {
	Buy = 0,
	Sell = 1
}

public class CoinPair {
	/*
		FromCoin/ToCoin = Price
		BTC/USDT = 40000
		LKR/USDT = 310
	*/

	public required string FromCoin { get; set; }
	public required string ToCoin { get; set; }
	public decimal Price { get; set; }
}

public class TransactionHistory {
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	public required CoinPair Coin { get; set; }
	public required decimal USDTInvested { get; set; }
	public CoinPair? LKRPrice { get; set; } = new() { FromCoin = "USDT", ToCoin = "LKR" };

	public decimal? Quantity { get; set; }

	public required string TransactionCoin { get; set; } = "BNB";
	public required decimal TransactionFee { get; set; }

	public required PurchaseType CoinPurchaseType { get; set; }

	public string? TransactionNote { get; set; }

	public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public async Task Calculate() {
		try {
            LKRPrice.Price = await CurrencyService.GetLKRPrice();
            if (Coin.Price == 0) Coin.Price = await CurrencyService.GetCryptoCoinPrice(Coin.FromCoin, Coin.ToCoin);

			Quantity = Math.Round((USDTInvested - TransactionFee) * Coin.Price, 8);
        } catch (Exception) {
			throw;
		}
	}
}
