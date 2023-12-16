using System.Text.RegularExpressions;
using CoinTrackerHistory.API.Configurations;
using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.DTO;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Dynamic.Core;
using CoinTrackerHistory.API.Interfaces;
using System;

namespace CoinTrackerHistory.API.Services;

public enum FilterCommands {
	OrderByDesc,
	OrderByAsc,
	FindEq,
	FindGt,
	FindLt,
	FindGtE,
	FindLtE,
	FindLike
}

public class FilterTemplate {
	public FilterCommands Command {
		get; set;
	}
	public string Field {
		get; set;
	}
	public string Value {
		get; set;
	}
}

public static class Validation {
	public static Regex Id = new Regex("^[a-fA-F0-9]+$");
	public static Regex Num = new Regex("^[0-9]+$");
	public static Regex CommandValue = new Regex(@"^[A-Za-z0-9_\.]{2,50}$");
	public static Regex CommandField = new Regex("^[A-Za-z]{2,10}$");
}

public class PurchaseService : IPurchaseService {
	private readonly MongoDBConfig mongoDBConfig = new MongoDBConfig();
	private readonly string collectionName = "TransactionHistory";
	public IMongoCollection<TransactionHistory> collection;

	private readonly PurchaseType purchaseType;
	private readonly string[] allowedFilterCommands = Enum.GetNames(typeof(FilterCommands));

	public PurchaseService(PurchaseType purchaseType) {
		try {
			MongoClient client = new MongoClient(mongoDBConfig.CONNECTION_STR);
			IMongoDatabase database = client.GetDatabase(mongoDBConfig.DATABASE_NAME);
			collection = database.GetCollection<TransactionHistory>(collectionName);

			this.purchaseType = purchaseType;
		} catch (Exception) {
			throw;
		}
	}

	public void ValidateFilters(int page, int limit, List<FilterTemplate>? filters = null) {
		if (page <= 0 || limit <= 0)
			throw new FormatException();
		if (filters == null)
			return;

		int filterCount = filters.Count;
		for (int i = 0; i < filterCount; i++) {
			FilterTemplate filter = filters[i];

			bool isCommandInValid = string.IsNullOrWhiteSpace(
				allowedFilterCommands
				.AsQueryable()
				.Where(c => c == Enum.GetName(filter.Command))
				.FirstOrDefault()
			);
			if (!isCommandInValid) throw new BadRequestException();
			if (
				!Validation.CommandValue.IsMatch(filter.Value) ||
				!Validation.CommandField.IsMatch(filter.Field)
			) throw new BadRequestException();
		}
	}
	public IMongoQueryable<TransactionHistory> Filter(List<FilterTemplate> filters, int page, int limit) {
		try {
			ValidateFilters(page, limit, filters);

			IMongoQueryable<TransactionHistory> query = collection.AsQueryable<TransactionHistory>();

			if (filters != null) {
				int filterCount = filters.Count;
				for (int i = 0; i < filterCount; i++) {
					FilterTemplate filter = filters[i];

					switch (filter.Command) {
						case FilterCommands.FindEq:
							query = (IMongoQueryable<TransactionHistory>) query.Where(filter.Field + " == " + filter.Value);
							break;
						case FilterCommands.FindGt:
							query = (IMongoQueryable<TransactionHistory>) query.Where(filter.Field + " > " + filter.Value);
							break;
						case FilterCommands.FindLt:
							query = (IMongoQueryable<TransactionHistory>) query.Where(filter.Field + " < " + filter.Value);
							break;
						case FilterCommands.FindGtE:
							query = (IMongoQueryable<TransactionHistory>) query.Where(filter.Field + " >= " + filter.Value);
							break;
						case FilterCommands.FindLtE:
							query = (IMongoQueryable<TransactionHistory>) query.Where(filter.Field + " <= " + filter.Value);
							break;
						case FilterCommands.FindLike:
							query = (IMongoQueryable<TransactionHistory>) query.Where("c => c." + filter.Field + ".Contains(" + filter.Value + ")");
							break;
						case FilterCommands.OrderByDesc:
							query = (IMongoQueryable<TransactionHistory>) query.OrderBy($"{filter.Field} DESC");
							break;
						case FilterCommands.OrderByAsc:
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

	public async Task<List<TransactionHistory>> Get(int page, int limit) {
		try {
			List<FilterTemplate> filters = new List<FilterTemplate> {
				new FilterTemplate() { Command = FilterCommands.FindEq, Field = "CoinPurchaseType", Value = ((int)purchaseType).ToString()},
				new FilterTemplate() { Command = FilterCommands.OrderByDesc, Field = "CreatedAt" }
			}.Distinct().ToList();

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
	public async Task<TransactionHistory> GetById(string id) {
		try {
			if (!Validation.Id.IsMatch(id))
				throw new FormatException();

			List<FilterTemplate> filters = new List<FilterTemplate> {
				new FilterTemplate() { Command = FilterCommands.FindEq, Field = "Id", Value = id }
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
