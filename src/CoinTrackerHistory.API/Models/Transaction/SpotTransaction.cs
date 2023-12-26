using CoinTrackerHistory.API.Configurations;
using MongoDB.Bson.Serialization.Attributes;

namespace CoinTrackerHistory.API.Models.Transaction;

public class SpotTransaction : Transaction {

	public SpotTransaction() {
		IsP2P = false;
		Note = "Spot via Wallet";
		BankTransferFee = 0;
		ExchangeConversionFee = 0;
	}

	[BsonIgnore]
	public override decimal BankTransferFee { get; set; }

	[BsonIgnore]
	public override decimal ExchangeConversionFee { get; set; }

	public override decimal Quantity {
		get {
			return Math.Round((Investment * Coin.Price) - Fee, Constants.DECIMAL_PLACES);
		}
	}

	public override decimal TotalExpenses {
		get {
			return Math.Round((Coin.Price / LKR2USD) * Fee, Constants.DECIMAL_PLACES);
		}
	}
}
