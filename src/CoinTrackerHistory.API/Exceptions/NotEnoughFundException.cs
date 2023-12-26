using System;
namespace CoinTrackerHistory.API.Exceptions;

public class NotEnoughFundException : Exception {
	public override string Message {
		get {
			return "Not Enough Coin Balance";
		}
	}
}

