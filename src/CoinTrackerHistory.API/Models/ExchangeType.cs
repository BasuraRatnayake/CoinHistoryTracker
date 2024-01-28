using System;
namespace CoinTrackerHistory.API.Models;

public enum ExchangeType {
	Binance,
	ByBit,
	Okx
};

public static class Binance {
	static internal class Regular {
		private static readonly decimal Discount = 0.0075M;
		private static readonly decimal Fee = 0.01M;

		public static decimal GetDiscountedFee(decimal quantity, bool isDiscounted = false, decimal BNB2Coin = 0) {
			if (isDiscounted) {
				decimal discount = quantity * Discount;
				return discount / BNB2Coin;
			}

			return quantity * Fee;
		}
	}
}