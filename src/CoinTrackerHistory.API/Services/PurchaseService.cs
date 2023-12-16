using System.Text.RegularExpressions;
using CoinTrackerHistory.API.Configurations;
using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.DTO;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;
using CoinTrackerHistory.API.Interfaces;
using CoinTrackerHistory.API.Models;

namespace CoinTrackerHistory.API.Services;

public static class Validation {
	public static Regex Id = new Regex("^[a-fA-F0-9]+$");
	public static Regex Num = new Regex("^[0-9]+$");
	public static Regex CommandValue = new Regex(@"^[A-Za-z0-9_\.]{1,50}$");
	public static Regex CommandField = new Regex(@"^[A-Za-z\.]{1,50}$");
}

public class PurchaseService : IPurchaseService {
	private readonly MongoDBConfig mongoDBConfig = new MongoDBConfig();
	private readonly string collectionName = "TransactionHistory";
	public IMongoCollection<TransactionHistory> collection;

	private readonly PurchaseType purchaseType;
	private FilterTransactions filter;

	public PurchaseService(PurchaseType purchaseType) {
		try {
			MongoClient client = new MongoClient(mongoDBConfig.CONNECTION_STR);
			IMongoDatabase database = client.GetDatabase(mongoDBConfig.DATABASE_NAME);
			collection = database.GetCollection<TransactionHistory>(collectionName);

			filter = new FilterTransactions(collection);

			this.purchaseType = purchaseType;
		} catch (Exception) {
			throw;
		}
	}

	public async Task<TransactionHistory> LastInsertedRecord() {
		try {
			IMongoQueryable<TransactionHistory> query = collection.AsQueryable<TransactionHistory>();
			query = query.Where(_ => true);
			query = (IMongoQueryable<TransactionHistory>) query.OrderBy($"CreatedAt DESC");
			query = query.Take(1);

			return await query.SingleOrDefaultAsync<TransactionHistory>();
		} catch (InternalServerException) {
			throw;
		} catch (Exception) {
			throw;
		}
	}

	public async Task<List<TransactionHistory>> Get(int page, int limit, List<FilterTemplate>? filters = null) {
		try {
			List<FilterTemplate> _filters = new List<FilterTemplate>();
			_filters.AddRange(
				new List<FilterTemplate> {
					new FilterTemplate() {
						Command = FilterCommands.FindEq, Field = "CoinPurchaseType", Value = ((int)purchaseType).ToString()
					}
				});

			if (filters != null)
				_filters.AddRange(filters);

			_filters = _filters.Distinct().ToList();

			List<TransactionHistory> data = await filter.Transactions(_filters, page, limit).ToListAsync();

			if (data.Count == 0)
				throw new NotFoundException();

			return data;
		} catch (FormatException) {
			throw new BadRequestException();
		} catch (NotFoundException) {
			throw;
		} catch (InternalServerException) {
			throw;
		}
	}
	public async Task<TransactionHistory> GetById(string id) {
		try {
			if (!Validation.Id.IsMatch(id))
				throw new FormatException();

			List<FilterTemplate> filters = new List<FilterTemplate> {
				new FilterTemplate() { Command = FilterCommands.FindEq, Field = "Id", Value = id }
			};

			TransactionHistory data = await filter.Transactions(filters, 1, 1).FirstOrDefaultAsync();

			if (data == null)
				throw new NotFoundException();

			return data;
		} catch (FormatException) {
			throw new BadRequestException();
		} catch (NotFoundException) {
			throw;
		} catch (InternalServerException) {
			throw;
		}
	}

	public async Task<TransactionHistory> Add(TransactionHistory coin) {
		try {
			coin.Id = null;
			coin.CoinPurchaseType = purchaseType;
			coin.CreatedAt = DateTime.Now;

			await coin.Calculate();
			await collection.InsertOneAsync(coin);

			return await LastInsertedRecord();
		} catch (FormatException) {
			throw new BadRequestException();
		} catch (InternalServerException) {
			throw;
		}
	}
}
