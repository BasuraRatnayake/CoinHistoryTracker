using System;
using MongoDB.Bson.Serialization.Attributes;

namespace CoinTrackerHistory.API.Models;

public class CoinPair {
	/*
		FromCoin/ToCoin = Price
		BTC/USDT = 40000
		LKR/USD = 309
		LKR/USDT = 310
	*/

	[BsonDefaultValue("")]
	[BsonIgnoreIfDefault]
	public string From { get; set; } = "";

	[BsonDefaultValue("")]
	[BsonIgnoreIfDefault]
	public string To { get; set; } = "";

	[BsonDefaultValue("0")]
	[BsonIgnoreIfDefault]
	public decimal Price { get; set; } = 0;
}
