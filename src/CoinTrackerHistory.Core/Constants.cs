using System;
namespace CoinTrackerHistory.Core;

public enum Exchanges {
	Binance,
	ByBit,
	OKX
}

public enum TransactionType {
	SpotPurchase,
	SpotSale,

	P2PPurchase,
	P2PSale,

	Earn,

	RedPacket
}

public static class Constants {
	public static readonly int PERCENTAGE_MAX_DECIMAL_POINTS = 2;
	public static readonly int MAX_DECIMAL_POINTS = 8;
}
