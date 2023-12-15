using System.Collections.Generic;
using System.Text.RegularExpressions;
using CoinTrackerHistory.API.Configurations;
using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.DTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace CoinTrackerHistory.API.Services;

public class FilterColumn {
	public string? Column { get; set; }
	public string? Value { get; set; }
}

public static class Validation {
	public static Regex Id = new Regex("^[a-fA-F0-9]+$");
	public static Regex Num = new Regex("^[0-9]+$");
}

public class BuyService {
	private readonly MongoDBConfig mongoDBConfig = new MongoDBConfig();
	private readonly string collectionName = "TransactionHistory";

	private IMongoCollection<TransactionHistory> collection;

	public BuyService() {
		try {
			MongoClient client = new MongoClient(mongoDBConfig.CONNECTION_STR);
			IMongoDatabase database = client.GetDatabase(mongoDBConfig.DATABASE_NAME);
			collection = database.GetCollection<TransactionHistory>(collectionName);
		} catch (Exception) {
			throw;
		}
	}

	public async Task<TransactionHistory> GetLastInserted() {
		try {
			return await collection
			.Find(_ => true)
			.SortByDescending(h => h.CreatedAt)
			.Limit(1)
			.FirstOrDefaultAsync();
        } catch (Exception) {
            throw;
        }
	}

	public async Task<List<TransactionHistory>> Get(int page, int limit) {
		try {
			if (page <= 0 || limit <= 0) throw new FormatException();

			List<TransactionHistory> data = await collection
				.Find(c => c.CoinPurchaseType == PurchaseType.Buy)
				.SortBy(c => c.CreatedAt)
				.Skip((page - 1) * limit)
				.Limit(limit)
				.ToListAsync();

			if (data.Count == 0) throw new NotFoundException();

			return data;
		} catch (FormatException) {
			throw new BadRequestException();
		} catch (NotFoundException) {
			throw;
		}
	}
	public async Task<TransactionHistory> GetById(string id) {
		try {
			if (!Validation.Id.IsMatch(id))
				throw new FormatException();

			TransactionHistory data = await collection
				.Find(c => c.Id == id)
				.FirstOrDefaultAsync();

			if (data == null) throw new NotFoundException();

			return data;
		} catch (FormatException) {
			throw new BadRequestException();
		} catch (NotFoundException) {
			throw;
		} catch (Exception) {
			throw;
		}
	}
	public async Task<List<TransactionHistory>> GetByFilter(List<FilterColumn> filters) {
		try {
			return new List<TransactionHistory>();
		} catch (Exception) {
			throw;
		}
	}

	public async Task<TransactionHistory> Add(TransactionHistory coin) {
		try {
			coin.Id = null;
			coin.CoinPurchaseType = PurchaseType.Buy;
			coin.CreatedAt = DateTime.Now;

			await coin.Calculate();
			await collection.InsertOneAsync(coin);

			return await GetLastInserted();
		} catch (Exception) {
			throw;
		}
	}
}
