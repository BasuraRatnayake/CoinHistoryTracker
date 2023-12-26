using System.Linq.Dynamic.Core;
using CoinTrackerHistory.API.Configurations;
using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.Transaction;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CoinTrackerHistory.API.Services.Filter;

public class FilterTransactionService {
	private const string COLLECTION_NAME = "Transactions";
	private readonly string[] allowedFilterCommands = Enum.GetNames(typeof(FilterCommands));
	private IMongoCollection<Transaction> collection;

	public FilterTransactionService(IMongoDatabase database) {
		collection = database.GetCollection<Transaction>(COLLECTION_NAME);
	}

	public void ValidateFilters(int page, int limit, List<FilterTemplate>? filters = null) {
		if (page <= 0 || limit <= 0)
			throw new FormatException();
		if (filters == null)
			return;

		int filterCount = filters.Count;
		for (int i = 0; i < filterCount; i++) {
			FilterTemplate filter = filters[i];

			filter.Value = string.IsNullOrEmpty(filter.Value) ? "" : filter.Value;

			bool isCommandInValid = string.IsNullOrWhiteSpace(
				allowedFilterCommands
				.AsQueryable()
				.Where(c => c == Enum.GetName(filter.Command))
				.FirstOrDefault()
			);
			if (isCommandInValid)
				throw new BadRequestException();

			bool isFilterNonValueAllowed = filter.Command == FilterCommands.OrderByAsc || filter.Command == FilterCommands.OrderByDesc;

			if (
				(!Constants.COMMAND_VALUE.IsMatch(filter.Value) && !isFilterNonValueAllowed) ||
				!Constants.COMMAND_FIELD.IsMatch(filter.Field)
			)
				throw new BadRequestException();
		}
	}

	public async Task<List<Transaction>> Transactions(List<FilterTemplate> filters, int page, int limit) {
		try {
			ValidateFilters(page, limit, filters);

			IMongoQueryable<Transaction> query = collection.AsQueryable<Transaction>();

			if (filters != null) {
				int filterCount = filters.Count;
				for (int i = 0; i < filterCount; i++) {
					FilterTemplate filter = filters[i];

					switch (filter.Command) {
						case FilterCommands.FindEq:
							query = (IMongoQueryable<Transaction>) query.Where($"{filter.Field} = @0", filter.Value);
							break;
						case FilterCommands.FindGt:
							query = (IMongoQueryable<Transaction>) query.Where($"{filter.Field} > @0", decimal.Parse(filter.Value));
							break;
						case FilterCommands.FindLt:
							query = (IMongoQueryable<Transaction>) query.Where($"{filter.Field} < @0", decimal.Parse(filter.Value));
							break;
						case FilterCommands.FindLtE:
							query = (IMongoQueryable<Transaction>) query.Where($"{filter.Field} <= @0", decimal.Parse(filter.Value));
							break;
						case FilterCommands.FindGtE:
							query = (IMongoQueryable<Transaction>) query.Where($"{filter.Field} >= @0", decimal.Parse(filter.Value));
							break;
						case FilterCommands.FindLike:
							query = (IMongoQueryable<Transaction>) query.Where($"{filter.Field}.Contains(@0)", filter.Value);
							break;
						case FilterCommands.OrderByDesc:
							query = (IMongoQueryable<Transaction>) query.OrderBy($"{filter.Field} DESC");
							break;
						case FilterCommands.OrderByAsc:
							query = (IMongoQueryable<Transaction>) query.OrderBy($"{filter.Field}");
							break;
					}
				}
			}

			query = query.Skip((page - 1) * limit);
			query = query.Take(limit);

			return await query.ToListAsync();
		} catch (FormatException) {
			throw;
		} catch (NotFoundException) {
			throw;
		} catch (InternalServerException) {
			throw;
		}
	}
}
