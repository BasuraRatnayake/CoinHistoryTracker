using CoinTrackerHistory.API.Configurations;

namespace CoinTrackerHistory.API.Models.Transaction;

public class SpotTransaction : Transaction {

	public SpotTransaction() {
		IsP2P = false;
		Type = TransactionType.SpotBuy;
		Note = "Spot via Wallet";
	}

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
