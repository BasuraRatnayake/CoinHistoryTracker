using System.Linq.Dynamic.Core;
using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CoinTrackerHistory.API.Models;

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
	public required FilterCommands Command {
		get; set;
	}
	public required string Field {
		get; set;
	}
	public string Value {
		get; set;
	}
}

public class FilterTransactions {
	private readonly string[] allowedFilterCommands = Enum.GetNames(typeof(FilterCommands));
	private IMongoCollection<TransactionHistory> collection;


	public FilterTransactions(IMongoCollection<TransactionHistory> collection) {
		this.collection = collection;
	}

	public void ValidateFilters(int page, int limit, List<FilterTemplate>? filters = null) {
		if (page <= 0 || limit <= 0) throw new FormatException();
		if (filters == null) return;

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
				(!Validation.CommandValue.IsMatch(filter.Value) && !isFilterNonValueAllowed) ||
				!Validation.CommandField.IsMatch(filter.Field)
			)
				throw new BadRequestException();
		}
	}

	public IMongoQueryable<TransactionHistory> Transactions(List<FilterTemplate> filters, int page, int limit) {
		try {
			ValidateFilters(page, limit, filters);

			IMongoQueryable<TransactionHistory> query = collection.AsQueryable<TransactionHistory>();

			if (filters != null) {
				int filterCount = filters.Count;
				for (int i = 0; i < filterCount; i++) {
					FilterTemplate filter = filters[i];

					switch (filter.Command) {
						case FilterCommands.FindEq:
							query = (IMongoQueryable<TransactionHistory>) query.Where($"{filter.Field} = @0", filter.Value);
							break;
						case FilterCommands.FindGt:
							query = (IMongoQueryable<TransactionHistory>) query.Where($"{filter.Field} > @0", decimal.Parse(filter.Value));
							break;
						case FilterCommands.FindLt:
							query = (IMongoQueryable<TransactionHistory>) query.Where($"{filter.Field} < @0", decimal.Parse(filter.Value));
							break;
						case FilterCommands.FindLtE:
							query = (IMongoQueryable<TransactionHistory>) query.Where($"{filter.Field} <= @0", decimal.Parse(filter.Value));
							break;
						case FilterCommands.FindGtE:
							query = (IMongoQueryable<TransactionHistory>) query.Where($"{filter.Field} >= @0", decimal.Parse(filter.Value));
							break;
						case FilterCommands.FindLike:
							query = (IMongoQueryable<TransactionHistory>) query.Where($"{filter.Field}.Contains(@0)", filter.Value);
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
}
