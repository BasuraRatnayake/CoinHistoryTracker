using System;
namespace CoinTrackerHistory.API.Exceptions;

public class InternalServerException : Exception {
	public override string Message {
		get {
			return "Internal Server Error Occurred";
		}
	}
}

