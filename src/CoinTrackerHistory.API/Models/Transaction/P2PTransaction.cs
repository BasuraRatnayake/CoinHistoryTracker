using CoinTrackerHistory.API.Configurations;

namespace CoinTrackerHistory.API.Models.Transaction;

public class P2PTransaction : Transaction {
	public required decimal BankTransferFee { get; set; } = 30;
	public decimal ExchangeConversionFee { get; set; }

	public P2PTransaction() {
		IsP2P = true;
		Type = PurchaseType.Buy;
		Note = "P2P via Bank";
	}

	private void CalculateExchangeConversionFee() {
		ExchangeConversionFee = (Investment / LKR2USD) - (Investment / Coin.Price);
		ExchangeConversionFee = ExchangeConversionFee * LKR2USD;
		ExchangeConversionFee = Math.Round(ExchangeConversionFee, Constants.DECIMAL_PLACES);
	}

	public override decimal Quantity {
		get {
			return Math.Round(Investment / Coin.Price, Constants.DECIMAL_PLACES);
		}
	}

	public override decimal TotalExpenses {
		get {
			CalculateExchangeConversionFee();
			return Math.Round(BankTransferFee + ExchangeConversionFee, Constants.DECIMAL_PLACES);
		}
	}
}
