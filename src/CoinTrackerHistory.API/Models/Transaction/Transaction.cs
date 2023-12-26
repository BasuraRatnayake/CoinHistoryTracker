using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoinTrackerHistory.API.Models.Transaction;

public class Transaction {
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id {
		get; set;
	}

	public required decimal Investment {
		get; set;
	}

	public required decimal Fee {
		get; set;
	}

	public required decimal LKR2USD {
		get; set;
	}

	public required CoinPair Coin {
		get; set;
	}

	public bool? IsP2P {
		get; set;
	}

	public string? Note {
		get; set;
	}

	public required TransactionType Type { get; set; }

	public DateTime? CreatedAt { get; set; } = DateTime.Now;

	public virtual decimal Quantity {
		get {
			return 0;
		}
	}

	public virtual decimal TotalExpenses {
		get {
			return 0;
		}
	}
}

