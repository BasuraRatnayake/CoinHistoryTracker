using System;
namespace CoinTrackerHistory.API.Services.Filter;

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
