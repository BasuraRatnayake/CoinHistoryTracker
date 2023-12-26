using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.Transaction;
using CoinTrackerHistory.API.Services;
using CoinTrackerHistory.API.Services.Filter;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class FilterTransactionController : Controller {
	private readonly FilterTransactionService FilterService;

	public FilterTransactionController(DatabaseService database) {
		FilterService = new FilterTransactionService(database.Database);
	}

	[HttpPost]
	[Route("{type}/{page}/{limit}")]
	public async Task<List<Transaction>> Get([FromBody] List<FilterTemplate> filters, TransactionType type, int page = 1, int limit = 5) {
		try {
			List<FilterTemplate> _filters = new List<FilterTemplate>();
			_filters.AddRange(
				new List<FilterTemplate> {
					new FilterTemplate() {
						Command = FilterCommands.FindEq,
						Field = "Type",
						Value = ((int)type).ToString()
					}
				});

			if (filters != null)
				_filters.AddRange(filters);

			_filters = _filters.Distinct().ToList();

			List<Transaction> data = await FilterService.Transactions(_filters, page, limit);

			if (data.Count == 0)
				throw new NotFoundException();

			return data;
		} catch (Exception) {
			throw;
		}
	}
}
