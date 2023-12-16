using CoinTrackerHistory.API.Models;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using MongoDB.Driver.Linq;

namespace CoinTrackerHistory.API.Interfaces;

public interface IPurchaseService {
	public Task<TransactionHistory> LastInsertedRecord();

	public Task<List<TransactionHistory>> Get(int page, int limit, List<FilterTemplate>? filters = null);
	public Task<TransactionHistory> GetById(string id);

	public Task<TransactionHistory> Add(TransactionHistory coin);
}
