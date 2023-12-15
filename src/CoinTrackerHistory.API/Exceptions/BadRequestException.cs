using System;
namespace CoinTrackerHistory.API.Exceptions;

public class BadRequestException : Exception {
	public override string Message {
		get {
			return "Invalid Data Format For Field";
		}
	}
}
