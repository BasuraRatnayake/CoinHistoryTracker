using System;
using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models;
using CoinTrackerHistory.API.Models.Transactions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;

namespace CoinTrackerHistory.API.Services.Transactions;

public class EarnTransactionService {
	private const string COLLECTION_NAME = "Transactions";
	private IMongoCollection<EarnTransaction> Collection { get; set; }

	private readonly CoinWalletService CoinWalletService;

	public EarnTransactionService(IMongoDatabase database) {
		Collection = database.GetCollection<EarnTransaction>(COLLECTION_NAME);
		CoinWalletService = new(database);
	}

	public async Task<EarnTransaction> LastInsertedRecord() {
		try {
			IMongoQueryable<EarnTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.Type == TransactionType.Earn);
			query = (IMongoQueryable<EarnTransaction>)query.OrderBy("CreatedAt DESC");
			query = query.Take(1);

			return await query.SingleOrDefaultAsync();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<List<EarnTransaction>> Get() {
		try {
			IMongoQueryable<EarnTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.Type == TransactionType.Earn);
			query = (IMongoQueryable<EarnTransaction>) query.OrderBy("CreatedAt DESC");
			return await query.ToListAsync();
		} catch (Exception) {
			throw;
		}
	}
	public async Task<EarnTransaction> Get(string id) {
		try {
			IMongoQueryable<EarnTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.Type == TransactionType.Earn && t.Id == id);
			return await query.SingleAsync();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<EarnTransaction> Add(EarnTransaction data) {
		try {
			data.Id = null;
			data.Type = TransactionType.Earn;
			data.CreatedAt = DateTime.Now;
			data.IsP2P = false;
			data.BankTransferFee = 0;
			data.ExchangeConversionFee = 0;
			data.Investment = 0;
			data.Fee = 0;
			data.TotalExpenses = 0;

			Coin earnCoin = await CoinWalletService.Get(data.Coin.From);
			if (earnCoin == null)
				earnCoin = new Coin() { Name = data.Coin.From, Quantity = 0 };

			earnCoin.Quantity += data.BankTransferFee + data.BonusTieredAPRReward;

			await CoinWalletService.Modify(earnCoin);

			await Collection.InsertOneAsync(data);

			return await LastInsertedRecord();
		} catch (Exception) {
			throw;
		}
	}
}
