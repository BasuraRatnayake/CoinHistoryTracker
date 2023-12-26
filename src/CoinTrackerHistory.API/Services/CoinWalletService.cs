using CoinTrackerHistory.API.Models;
using CoinTrackerHistory.API.Models.Transaction;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;

namespace CoinTrackerHistory.API.Services;

public class CoinWalletService {
	private const string COLLECTION_NAME = "CoinWallet";
	public IMongoCollection<Coin> Collection { get; set; }

	public CoinWalletService(IMongoDatabase database) {
		Collection = database.GetCollection<Coin>(COLLECTION_NAME);
	}

	public async Task<Coin> Get(string coinName) {
		try {
			coinName = coinName.ToUpper();

			IMongoQueryable<Coin> query = Collection.AsQueryable();
			query = query.Where(c => c.Name == coinName);

			return await query.SingleOrDefaultAsync();
		} catch (Exception) {
			throw;
		}
	}
	public async Task<List<Coin>> Get() {
		try {
			IMongoQueryable<Coin> query = Collection.AsQueryable();
			query = (IMongoQueryable<Coin>) query.OrderBy("Name");
			return await query.ToListAsync();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<Coin> Add(Coin data) {
		try {
			data.Id = null;
			data.Name = data.Name.ToUpper();

			await Collection.InsertOneAsync(data);

			return await Get(data.Name);
		} catch (Exception) {
			throw;
		}
	}

	public async Task<Coin> Modify(Coin data) {
		try {
			await Collection.ReplaceOneAsync(c => c.Id == data.Id, data);
			return data;
		} catch (Exception) {
			throw;
		}
	}
}
