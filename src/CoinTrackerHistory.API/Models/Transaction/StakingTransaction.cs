using System;
using MongoDB.Bson.Serialization.Attributes;

namespace CoinTrackerHistory.API.Models.Transaction;

public class StakingTransaction : Transaction {
	public StakingTransaction() {
		IsP2P = false;
		Note = "Staking Rewards";
		BankTransferFee = 0;
		ExchangeConversionFee = 0;
		Investment = 0;
		Fee = 0;
		Type = TransactionType.Earn;
	}

	public override decimal Quantity { get; set; }

	[BsonIgnore]
	public override decimal Investment { get; set; } = 0;

	[BsonIgnore]
	public override decimal Fee { get; set; } = 0;

	[BsonIgnore]
	public override decimal BankTransferFee { get; set; } = 0;

	[BsonIgnore]
	public override decimal ExchangeConversionFee { get; set; } = 0;

	[BsonIgnore]
	public override decimal TotalExpenses { get; set; } = 0;
}
