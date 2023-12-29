using System;
using CoinTrackerHistory.API.Models.Transactions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;
using CoinTrackerHistory.API.Models;

namespace CoinTrackerHistory.API.Services.Transactions;

public class RedPacketTransactionService {
	private const string COLLECTION_NAME = "Transactions";
	private IMongoCollection<RedPacketTransaction> Collection { get; set; }

	private readonly CoinWalletService CoinWalletService;

	public RedPacketTransactionService(IMongoDatabase database) {
		Collection = database.GetCollection<RedPacketTransaction>(COLLECTION_NAME);
		CoinWalletService = new(database);
	}

	public async Task<RedPacketTransaction> LastInsertedRecord() {
		try {
			IMongoQueryable<RedPacketTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.Type == TransactionType.RedPacket);
			query = (IMongoQueryable<RedPacketTransaction>) query.OrderBy("CreatedAt DESC");
			query = query.Take(1);

			return await query.SingleOrDefaultAsync();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<RedPacketTransaction> Add(RedPacketTransaction data) {
		try {
			data.Id = null; 
			data.Type = TransactionType.RedPacket;
			data.CreatedAt = DateTime.Now;
			data.IsP2P = false;
			data.BankTransferFee = 0;
			data.ExchangeConversionFee = 0;
			data.RealTimeAPRReward = 0;
			data.BonusTieredAPRReward = 0;
			data.Investment = 0;
			data.Fee = 0;
			data.TotalExpenses = 0;

			Coin redPacketCoin = await CoinWalletService.Get(data.Coin.From);
			if (redPacketCoin == null)
				redPacketCoin = new Coin() { Name = data.Coin.From, Quantity = 0 };

			redPacketCoin.Quantity += data.Quantity;

			await CoinWalletService.Modify(redPacketCoin);

			await Collection.InsertOneAsync(data);

			return await LastInsertedRecord();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<List<RedPacketTransaction>> Get() {
		try {
			IMongoQueryable<RedPacketTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.Type == TransactionType.RedPacket);
			query = (IMongoQueryable<RedPacketTransaction>) query.OrderBy("CreatedAt DESC");
			return await query.ToListAsync();
		} catch (Exception) {
			throw;
		}
	}
	public async Task<RedPacketTransaction> Get(string id) {
		try {
			IMongoQueryable<RedPacketTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.Type == TransactionType.RedPacket && t.Id == id);
			return await query.SingleAsync();
		} catch (Exception) {
			throw;
		}
	}
}
