using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoinTrackerHistory.API.Models.Transaction;

public class Transaction {
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public virtual decimal Investment { get; set; }

	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public virtual decimal Fee { get; set; }

	public required decimal LKR2USD { get; set; }

	public required CoinPair Coin { get; set; }

	public bool IsP2P { get; set; }

	[BsonDefaultValue("Transaction Data")]
	public string? Note { get; set; }

	public required TransactionType Type { get; set; }

	public DateTime? CreatedAt { get; set; } = DateTime.Now;

	public virtual decimal Quantity { get; set; }

	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public virtual decimal TotalExpenses { get; set; }

	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public virtual decimal BankTransferFee { get; set; }

	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public virtual decimal ExchangeConversionFee { get; set; }
}
