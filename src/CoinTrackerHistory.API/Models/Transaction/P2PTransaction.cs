using CoinTrackerHistory.API.Configurations;

namespace CoinTrackerHistory.API.Models.Transaction;

public class P2PTransaction : Transaction {
	public P2PTransaction() {
		IsP2P = true;
		Note = "P2P via Bank";
	}

	public override decimal Quantity {
		get {
			return Math.Round(Investment / Coin.Price, Constants.DECIMAL_PLACES);
		}
	}

	public override decimal TotalExpenses {
		get {
			ExchangeConversionFee = (Investment / LKR2USD) - (Investment / Coin.Price);
			ExchangeConversionFee = ExchangeConversionFee * LKR2USD;
			ExchangeConversionFee = Math.Round(ExchangeConversionFee, Constants.DECIMAL_PLACES);

			return Math.Round(BankTransferFee + ExchangeConversionFee, Constants.DECIMAL_PLACES);
		}
	}
}
