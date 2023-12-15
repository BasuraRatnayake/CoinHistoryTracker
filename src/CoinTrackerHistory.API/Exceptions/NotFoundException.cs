using System;
namespace CoinTrackerHistory.API.Exceptions;

public class NotFoundException : Exception {
    public override string Message {
        get {
            return "No Such Resource Found";
        }
    }
}

