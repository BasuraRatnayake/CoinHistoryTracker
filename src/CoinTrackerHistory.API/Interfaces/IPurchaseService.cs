using System;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using MongoDB.Driver.Linq;

namespace CoinTrackerHistory.API.Interfaces;

public interface IPurchaseService {
	public Task<TransactionHistory> LastInsertedRecord();

	public void IsPaginationValid(int page, int limit);

	public IMongoQueryable<TransactionHistory> Filter(List<RecordFilter> filters, int page, int limit);

	public Task<List<TransactionHistory>> Get(int page, int limit);
	public Task<TransactionHistory> GetById(string id);
	public Task<List<TransactionHistory>> GetByFilter(List<RecordFilter> filters, int page, int limit);

	public Task<TransactionHistory> Add(TransactionHistory coin);
}
