using System;
namespace CoinTrackerHistory.API.Models.Transactions;

public enum TransactionType {
	SpotPurchase,
	SpotSale,

	P2PPurchase,
	P2PSale,

	Earn,

	RedPacket
}
