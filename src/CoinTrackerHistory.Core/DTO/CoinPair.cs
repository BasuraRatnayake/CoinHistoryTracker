using System;
namespace CoinTrackerHistory.Core.DTO;

public class CoinPair {
	public string From { get; set; }
	public string To { get; set; }
	public decimal Price { get; set; }
}
