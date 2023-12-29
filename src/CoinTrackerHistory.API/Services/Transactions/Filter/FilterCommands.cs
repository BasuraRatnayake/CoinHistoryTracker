using System;
namespace CoinTrackerHistory.API.Services.Transactions.Filter;

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
