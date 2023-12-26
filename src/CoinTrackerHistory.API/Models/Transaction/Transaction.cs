using CoinTrackerHistory.API.Configurations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoinTrackerHistory.API.Models.Transaction;

public enum PurchaseType {
	Buy,
	Sell
}

public class CoinPair {
	/*
		FromCoin/ToCoin = Price
		BTC/USDT = 40000
		LKR/USD = 309
		LKR/USDT = 310
	*/

	public required string From { get; set; }
	public required string To { get; set; }
	public required decimal Price { get; set; } = 0;
}

public class Transaction {
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	public required decimal Investment { get; set; }

	public required decimal LKR2USD { get; set; }
	public required CoinPair Coin { get; set; }

	public required decimal Fee { get; set; }

	public string? Note { get; set; }

	public required PurchaseType Type { get; set; } = PurchaseType.Buy;

	public DateTime? CreatedAt { get; set; } = DateTime.Now;

	public bool? IsP2P { get; set; }

	public virtual decimal Quantity {
		get {
			return Math.Round((Investment * Coin.Price) - Fee, Constants.DECIMAL_PLACES);
		}
	}

	public virtual decimal TotalExpenses {
		get {
			return Math.Round((Coin.Price / LKR2USD) * Fee, Constants.DECIMAL_PLACES);
		}
	}
}
