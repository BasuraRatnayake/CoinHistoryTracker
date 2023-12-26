using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinTrackerHistory.API.Models.Transaction;
using CoinTrackerHistory.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class P2PTransactionController : Controller {
	private readonly P2PTransactionService P2PService;

	public P2PTransactionController(DatabaseService database) {
		P2PService = new P2PTransactionService(database.Database);
	}

	[HttpPost]
	public async Task<IActionResult> Purchase([FromBody] P2PTransaction data) {
		try {
			P2PTransaction response = await P2PService.Add(data);
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
