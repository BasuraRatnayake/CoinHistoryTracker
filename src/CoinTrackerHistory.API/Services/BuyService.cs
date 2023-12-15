using System.Text.RegularExpressions;
using CoinTrackerHistory.API.Configurations;
using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.DTO;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;

namespace CoinTrackerHistory.API.Services;

public enum RecordCommand {
	OrderByDesc,
	OrderByAsc,
	FindEqual,
	FindLike
}
public class RecordFilter {
	public RecordCommand Command { get; set; }
	public string Field { get; set; }
	public string Value { get; set; }
}

public static class Validation {
	public static Regex Id = new Regex("^[a-fA-F0-9]+$");
	public static Regex Num = new Regex("^[0-9]+$");
}

public class BuyService {
	private readonly MongoDBConfig mongoDBConfig = new MongoDBConfig();
	private readonly string collectionName = "TransactionHistory";
	private IMongoCollection<TransactionHistory> collection;

	private readonly PurchaseType purchaseType = PurchaseType.Buy;

	public BuyService() {
		try {
			MongoClient client = new MongoClient(mongoDBConfig.CONNECTION_STR);
			IMongoDatabase database = client.GetDatabase(mongoDBConfig.DATABASE_NAME);
			collection = database.GetCollection<TransactionHistory>(collectionName);
		} catch (Exception) {
			throw;
		}
	}

	public async Task<TransactionHistory> LastInsertedRecord() {
		try {
			IMongoQueryable<TransactionHistory> query = collection.AsQueryable<TransactionHistory>();
			query = query.Where(_ => true);
			query = (IMongoQueryable<TransactionHistory>)query.OrderBy($"CreatedAt DESC");
			query = query.Take(1);

			return await query.SingleOrDefaultAsync<TransactionHistory>();
		} catch (InternalServerException) {
			throw;
		} catch (Exception) {
			throw;
		}
	}

	public void IsPaginationValid(int page, int limit) {
		if (page <= 0 || limit <= 0)
			throw new FormatException();
	}

	public IMongoQueryable<TransactionHistory> Filter(List<RecordFilter> filters, int page, int limit) {
		try {
			IsPaginationValid(page, limit);

			IMongoQueryable<TransactionHistory> query = collection.AsQueryable<TransactionHistory>();

			if (filters != null) {
				int filterCount = filters.Count;
				for (int i = 0; i < filterCount; i++) {
					RecordFilter filter = filters[i];

					switch (filter.Command) {
						case RecordCommand.FindEqual:
							query = (IMongoQueryable<TransactionHistory>) query.Where(filter.Field + " == " + filter.Value);
							break;
						case RecordCommand.OrderByDesc:
							query = (IMongoQueryable<TransactionHistory>) query.OrderBy($"{filter.Field} DESC");
							break;
						case RecordCommand.OrderByAsc:
							query = (IMongoQueryable<TransactionHistory>) query.OrderBy($"{filter.Field}");
							break;
					}
				}
			}

			query = query.Skip((page - 1) * limit);
			query = query.Take(limit);

			return query;
		} catch (FormatException) {
			throw;
		} catch (NotFoundException) {
			throw;
		} catch (InternalServerException) {
			throw;
		}
	}

	public async Task<List<TransactionHistory>> Get(int page, int limit) {
        try {
			List<RecordFilter> filters = new List<RecordFilter> {
				new RecordFilter() { Command = RecordCommand.FindEqual, Field = "CoinPurchaseType", Value = ((int)purchaseType).ToString()},
                new RecordFilter() { Command = RecordCommand.OrderByDesc, Field = "CreatedAt" }
            }.Distinct().ToList();

            List<TransactionHistory> data = await Filter(filters, page, limit).ToListAsync();

			if (data.Count == 0)
				throw new NotFoundException();

			return data;
		} catch (FormatException) {
			throw new BadRequestException();
		} catch (NotFoundException) {
			throw;
		} catch (InternalServerException ex) {
			throw;
		}
	}
	public async Task<TransactionHistory> GetById(string id) {
		try {
			if (!Validation.Id.IsMatch(id))
				throw new FormatException();

			List<RecordFilter> filters = new List<RecordFilter> {
				new RecordFilter() { Command = RecordCommand.FindEqual, Field = "Id", Value = id }
            };

			TransactionHistory data = await Filter(filters, 1, 1).FirstOrDefaultAsync();

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
	public async Task<List<TransactionHistory>> GetByFilter(List<RecordFilter> filters, int page, int limit) {
		try {
			IsPaginationValid(page, limit);

			List<TransactionHistory> data = await Filter(filters, page, limit).ToListAsync();

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
