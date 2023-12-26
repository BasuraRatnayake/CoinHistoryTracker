using System;
namespace CoinTrackerHistory.API.Models.Transaction;

public enum TransactionType {
	SpotPurchase,
	SpotSale,

	P2PPurchase,
	P2PSale
}
