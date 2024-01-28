using CoinTrackerHistory.Core.DTO;

namespace CoinTrackerHistory.Core;

public class ProfitAndLoss {
	private Transaction transaction;
	private decimal currentPricePerCoin;
	private decimal USD2LKR;

	public ProfitAndLoss(Transaction transaction, decimal USD2LKR) {
		this.transaction = transaction;
		this.USD2LKR = USD2LKR;
	}

	public decimal AmountInUSD {
		get {
			return (transaction.NowCoin.Price - transaction.ThenCoin.Price) * transaction.Quantity;
		}
	}
	public decimal AmountInLKR {
		get {
			decimal profitNLoss = AmountInUSD * USD2LKR;
			return Math.Round(profitNLoss, Constants.MAX_DECIMAL_POINTS);
		}
	}

	public decimal PercentageInUSD {
		get {
			decimal profitNLossPercentage = (AmountInUSD / transaction.InvestedAmount) * 100;
			return Math.Round(profitNLossPercentage, Constants.PERCENTAGE_MAX_DECIMAL_POINTS);
		}
	}
	public decimal PercentageInLKR {
		get {
			decimal investedAmtInLKR = transaction.InvestedAmount * USD2LKR;
			decimal profitNLossPercentage = (AmountInLKR / investedAmtInLKR) * 100;
			profitNLossPercentage = Math.Round(profitNLossPercentage, Constants.PERCENTAGE_MAX_DECIMAL_POINTS);

			return profitNLossPercentage;
		}
	}

	public void CalculatePNL(decimal investedAmount, decimal fee, decimal boughtPrice, decimal currentPrice) {
		// Calculate Quantity
		decimal quantity = (investedAmount - fee) / boughtPrice;

		// Calculate PNL in USD
		decimal pnlUSD = (currentPrice - boughtPrice) * quantity;

		// Calculate PNL Percentage
		decimal pnlPercentage = (pnlUSD / investedAmount) * 100;

		Console.WriteLine($"Quantity: {Math.Round(quantity, 2)} coins");
		Console.WriteLine($"PNL in USD: {Math.Round(pnlUSD, 2)} USD");
		Console.WriteLine($"PNL Percentage: {Math.Round(pnlPercentage, 2)}%");
	}
}
