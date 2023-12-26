using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.Transaction;
using CoinTrackerHistory.API.Services;
using CoinTrackerHistory.API.Services.Filter;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class SpotTransactionController : Controller {
	private readonly SpotTransactionService SpotService;
	private readonly FilterTransactionService FilterService;

	public SpotTransactionController(DatabaseService database) {
		SpotService = new SpotTransactionService(database.Database);
		FilterService = new FilterTransactionService(database.Database);
	}

	[HttpPost]
	public async Task<IActionResult> Purchase([FromBody] SpotTransaction data) {
		try {
			SpotTransaction response = await SpotService.Purchase(data);
			return Ok(response);
		} catch (Exception) {
			throw;
		}
	}

	[HttpGet]
	public async Task<List<SpotTransaction>> Get() {
		try {
			return await SpotService.Get();
		} catch (Exception) {
			throw;
		}
	}

	[HttpGet]
	[Route("{id}")]
	public async Task<SpotTransaction> GetById(string id) {
		try {
			return await SpotService.Get(id);
		} catch (Exception) {
			throw;
		}
	}

	[HttpPost]
	[Route("filter/{type}/{page}/{limit}")]
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
