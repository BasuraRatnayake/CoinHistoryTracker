using System;
namespace CoinTrackerHistory.API.Models.Transaction;

public enum TransactionType {
	SpotBuy,
	SpotSell,

	P2PBuy,
	P2PSell
}
