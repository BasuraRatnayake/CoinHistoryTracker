using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinTrackerHistory.API.Models.Transactions;
using CoinTrackerHistory.API.Services;
using CoinTrackerHistory.API.Services.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers.Transactions;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class EarnTransactionController : Controller {
	private readonly EarnTransactionService EarnService;

	public EarnTransactionController(DatabaseService database) {
		EarnService = new EarnTransactionService(database.Database);
	}

	[HttpPost]
	public async Task<IActionResult> Purchase([FromBody] EarnTransaction data) {
		try {
			EarnTransaction response = await EarnService.Add(data);
			return Ok(response);
		} catch (Exception) {
			throw;
		}
	}

	[HttpGet]
	public async Task<List<EarnTransaction>> Get() {
		try {
			return await EarnService.Get();
		} catch (Exception) {
			throw;
		}
	}

	[HttpGet]
	[Route("{id}")]
	public async Task<EarnTransaction> GetById(string id) {
		try {
			return await EarnService.Get(id);
		} catch (Exception) {
			throw;
		}
	}
}
