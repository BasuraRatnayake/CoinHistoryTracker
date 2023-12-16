using System;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using MongoDB.Driver.Linq;

namespace CoinTrackerHistory.API.Interfaces;

public interface IPurchaseService {
	public Task<TransactionHistory> LastInsertedRecord();

	public void ValidateFilters(int page, int limit, List<FilterTemplate> filters = null);
	public IMongoQueryable<TransactionHistory> Filter(List<FilterTemplate> filters, int page, int limit);

	public Task<List<TransactionHistory>> Get(int page, int limit);
	public Task<TransactionHistory> GetById(string id);

	public Task<TransactionHistory> Add(TransactionHistory coin);
}
