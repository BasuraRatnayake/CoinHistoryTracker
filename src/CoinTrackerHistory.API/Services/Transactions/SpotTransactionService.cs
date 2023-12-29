using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models;
using CoinTrackerHistory.API.Models.Transactions;
using CoinTrackerHistory.API.Services.Transactions.Filter;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;

namespace CoinTrackerHistory.API.Services.Transactions;

public class SpotTransactionService {
	private const string COLLECTION_NAME = "Transactions";
	private IMongoCollection<SpotTransaction> Collection { get; set; }

	private readonly CoinWalletService CoinWalletService;

	public SpotTransactionService(IMongoDatabase database) {
		Collection = database.GetCollection<SpotTransaction>(COLLECTION_NAME);
		CoinWalletService = new(database);
	}

	public async Task<SpotTransaction> LastInsertedRecord() {
		try {
			IMongoQueryable<SpotTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.IsP2P == false);
			query = (IMongoQueryable<SpotTransaction>) query.OrderBy("CreatedAt DESC");
			query = query.Take(1);

			return await query.SingleOrDefaultAsync();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<List<SpotTransaction>> Get() {
		try {
			IMongoQueryable<SpotTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.IsP2P == false);
			query = (IMongoQueryable<SpotTransaction>) query.OrderBy("CreatedAt DESC");
			return await query.ToListAsync();
		} catch (Exception) {
			throw;
		}
	}
	public async Task<SpotTransaction> Get(string id) {
		try {
			IMongoQueryable<SpotTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.Id == id && t.IsP2P == false);
			return await query.SingleAsync();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<SpotTransaction> Purchase(SpotTransaction data) {
		try {
			data.Id = null;
			data.Type = TransactionType.SpotPurchase;
			data.CreatedAt = DateTime.Now;
			data.IsP2P = false;
			data.BankTransferFee = 0;
			data.ExchangeConversionFee = 0;

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
