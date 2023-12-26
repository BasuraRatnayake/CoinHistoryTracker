using System;
namespace CoinTrackerHistory.API.Models;

public class CoinPair {
	/*
		FromCoin/ToCoin = Price
		BTC/USDT = 40000
		LKR/USD = 309
		LKR/USDT = 310
	*/

	public required string From {
		get; set;
	}
	public required string To {
		get; set;
	}
	public required decimal Price { get; set; } = 0;
}
