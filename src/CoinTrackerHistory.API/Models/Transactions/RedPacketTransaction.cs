using System;
using CoinTrackerHistory.API.Configurations;

namespace CoinTrackerHistory.API.Models.Transactions;

public class RedPacketTransaction : Transaction {
	public RedPacketTransaction() {
		IsP2P = false;
		Note = "RedPacket Rewards";
		BankTransferFee = 0;
		ExchangeConversionFee = 0;
		Investment = 0;
		Fee = 0;
		BonusTieredAPRReward = 0;
		RealTimeAPRReward = 0;
		Type = TransactionType.RedPacket;
	}

	public override decimal Quantity { get; set; }
}
