using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models;
using CoinTrackerHistory.API.Models.Transaction;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;

namespace CoinTrackerHistory.API.Services;

public class SpotTransactionService {
	private const string COLLECTION_NAME = "Transactions";
	private IMongoCollection<Transaction> Collection { get; set; }

	private readonly CoinWalletService CoinWalletService;

	public SpotTransactionService(IMongoDatabase database) {
		Collection = database.GetCollection<Transaction>(COLLECTION_NAME);
		CoinWalletService = new(database);
	}

	public async Task<Transaction> LastInsertedRecord() {
		try {
			IMongoQueryable<Transaction> query = Collection.AsQueryable();
			query = query.Where(t => t.IsP2P == false);
			query = query.Take(1);

			return await query.SingleOrDefaultAsync();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<List<Transaction>> Get() {
		try {
			IMongoQueryable<Transaction> query = Collection.AsQueryable();
			query = query.Where(t => t.IsP2P == false);
			query = (IMongoQueryable<Transaction>) query.OrderBy("CreatedAt DESC");
			return await query.ToListAsync();
		} catch (Exception) {
			throw;
		}
	}
	public async Task<Transaction> Get(string id) {
		try {
			IMongoQueryable<Transaction> query = Collection.AsQueryable();
			query = query.Where(t => t.Id == id && t.IsP2P == false);
			return await query.SingleAsync();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<Transaction> Add(Transaction data) {
		try {
			data.Id = null;
			data.Type = PurchaseType.Buy;
			data.CreatedAt = DateTime.Now;
			data.IsP2P = false;

			Coin payCoin = await CoinWalletService.Get(data.Coin.From);
			if (payCoin == null) throw new NotEnoughFundException();

			payCoin.Quantity = payCoin.Quantity - data.Investment;

			if (payCoin.Quantity < 0) throw new NotEnoughFundException();

			Coin coinData = await CoinWalletService.Get(data.Coin.To);
			if (coinData == null) coinData = await CoinWalletService.Add(new Coin() { Name = data.Coin.To, Quantity = 0 });

			coinData.Quantity += data.Quantity;

			await CoinWalletService.Modify(payCoin);
			await CoinWalletService.Modify(coinData);

			await Collection.InsertOneAsync(data);

			return await LastInsertedRecord();
		} catch (Exception) {
			throw;
		}
	}
}
