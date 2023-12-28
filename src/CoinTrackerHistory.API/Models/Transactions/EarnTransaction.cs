using System;
using CoinTrackerHistory.API.Configurations;
using MongoDB.Bson.Serialization.Attributes;

namespace CoinTrackerHistory.API.Models.Transactions;

public class EarnTransaction : Transaction {
	public EarnTransaction() {
		IsP2P = false;
		Note = "Staking Rewards";
		BankTransferFee = 0;
		ExchangeConversionFee = 0;
		Investment = 0;
		Fee = 0;
		Type = TransactionType.Earn;
	}

	public override decimal Quantity {
		get {
			return Math.Round(RealTimeAPRReward + BonusTieredAPRReward, Constants.DECIMAL_PLACES);
		}
	}
}
