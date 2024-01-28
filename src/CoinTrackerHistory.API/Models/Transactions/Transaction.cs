using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoinTrackerHistory.API.Models.Transactions;

public class Transaction {
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public decimal Investment { get; set; }

	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public decimal Fee { get; set; }

	public decimal LKR2USD { get; set; }

	public required CoinPair Coin { get; set; }

	public bool IsP2P { get; set; }

	[BsonDefaultValue("Transaction Data")]
	public string? Note { get; set; }

	public required TransactionType Type { get; set; }

	public DateTime? CreatedAt { get; set; } = DateTime.Now;

	public virtual decimal Quantity { get; set; }

	public required ExchangeType Exchange { get; set; }

	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public virtual decimal TotalExpenses { get; set; }

	#region Spot
	public bool HasTransactionDiscount { get; set; } = false;
	#endregion

	#region Staking
	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public decimal RealTimeAPRReward  { get; set; }

	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public decimal BonusTieredAPRReward  { get; set; }
	#endregion

	#region P2P
	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public virtual decimal BankTransferFee { get; set; }

	[BsonDefaultValue(0)]
	[BsonIgnoreIfDefault]
	public virtual decimal ExchangeConversionFee { get; set; }
	#endregion
}
