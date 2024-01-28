using CoinTrackerHistory.Core;
using CoinTrackerHistory.Core.DTO;

decimal USD2LKR = 321.87m;

List<Transaction> transactions = new List<Transaction>();
//transactions.Add(new Transaction() {
//	ThenCoin = new CoinPair() {
//		From = "USDT",
//		To = "ADA",
//		Price = 0.624m
//	},
//	InvestedAmount = 5.0577m,
//	Fee = 0.0081m,
//	NowCoin = new CoinPair() {
//		From = "USDT",
//		To = "ADA",
//		Price = 0.5438m
//	}
//});
transactions.Add(new Transaction() {
	ThenCoin = new CoinPair() {
		From = "USDT",
		To = "SOL",
		Price = 97.00m
	},
	InvestedAmount = 39.77000000m,
	Fee = 0.00041000m,
	NowCoin = new CoinPair() {
		From = "USDT",
		To = "SOL",
		Price = 95.76m
	},
});

new ProfitAndLoss(transactions[0], USD2LKR).CalculatePNL(39.77041m, 0.00041m, 97.00m, 95.76m);


