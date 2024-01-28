using System;
namespace CoinTrackerHistory.Core.DTO;

public class Transaction {
	public decimal InvestedAmount {
		get; set;
	}
	public decimal Fee {
		get; set;
	}
	public CoinPair ThenCoin {
		get; set;
	}
	public CoinPair NowCoin {
		get; set;
	}

	public Exchanges Exchange {
		get; set;
	}

	public TransactionType Type {
		get; set;
	}

	public DateTime CreatedAt { get; set; } = DateTime.Now;

	public decimal Quantity {
		get {
			return (InvestedAmount - Fee) / ThenCoin.Price;
		}
	}
}
