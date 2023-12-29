using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.Transactions;
using CoinTrackerHistory.API.Services;
using CoinTrackerHistory.API.Services.Transactions.Filter;
using CoinTrackerHistory.API.Services.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers.Transactions;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class SpotTransactionController : Controller {
	private readonly SpotTransactionService SpotService;

	public SpotTransactionController(DatabaseService database) {
		SpotService = new SpotTransactionService(database.Database);
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
}
