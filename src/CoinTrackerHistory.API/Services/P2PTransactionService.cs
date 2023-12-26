using CoinTrackerHistory.API.Models;
using CoinTrackerHistory.API.Models.Transaction;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;

namespace CoinTrackerHistory.API.Services;

public class P2PTransactionService {
	private const string COLLECTION_NAME = "Transactions";
	private IMongoCollection<P2PTransaction> Collection { get; set; }

	private readonly CoinWalletService CoinWalletService;

	public P2PTransactionService(IMongoDatabase database) {
		Collection = database.GetCollection<P2PTransaction>(COLLECTION_NAME);
		CoinWalletService = new(database);
	}

	public async Task<P2PTransaction> LastInsertedRecord() {
		try {
			IMongoQueryable<P2PTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.IsP2P == true);
			query = query.Take(1);

			return await query.SingleOrDefaultAsync();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<List<P2PTransaction>> Get() {
		try {
			IMongoQueryable<P2PTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.IsP2P == true);
			query = (IMongoQueryable<P2PTransaction>) query.OrderBy("CreatedAt DESC");
			return await query.ToListAsync();
		} catch (Exception) {
			throw;
		}
	}
	public async Task<P2PTransaction> Get(string id) {
		try {
			IMongoQueryable<P2PTransaction> query = Collection.AsQueryable();
			query = query.Where(t => t.Id == id && t.IsP2P == true);
			return await query.SingleAsync();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<P2PTransaction> Add(P2PTransaction data) {
		try {
			data.Id = null;
			data.Type = PurchaseType.Buy;
			data.CreatedAt = DateTime.Now;

			Coin coinData = await CoinWalletService.Get(data.Coin.To);
			if (coinData == null) {
				coinData = await CoinWalletService.Add(new Coin() { Name = data.Coin.To, Quantity = 0 });
			}

			coinData.Quantity += data.Quantity;

			await CoinWalletService.Modify(coinData);
			await Collection.InsertOneAsync(data);

			return await LastInsertedRecord();
		} catch (Exception) {
			throw;
		}
	}
}
