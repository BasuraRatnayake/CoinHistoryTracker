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
public class P2PTransactionController : Controller {
	private readonly P2PTransactionService P2PService;
	private readonly FilterTransactionService FilterService;

	public P2PTransactionController(DatabaseService database) {
		P2PService = new P2PTransactionService(database.Database);
		FilterService = new FilterTransactionService(database.Database);
	}

	[HttpPost]
	public async Task<IActionResult> Purchase([FromBody] P2PTransaction data) {
		try {
			P2PTransaction response = await P2PService.Purchase(data);
			return Ok(response);
		} catch (Exception) {
			throw;
		}
	}

	[HttpGet]
	public async Task<List<P2PTransaction>> Get() {
		try {
			return await P2PService.Get();
		} catch (Exception) {
			throw;
		}
	}

	[HttpGet]
	[Route("{id}")]
	public async Task<P2PTransaction> GetById(string id) {
		try {
			return await P2PService.Get(id);
		} catch (Exception) {
			throw;
		}
	}
}
