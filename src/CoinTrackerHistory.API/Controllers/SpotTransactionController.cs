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
public class SpotTransactionController : Controller {
	private readonly SpotTransactionService SpotService;

	public SpotTransactionController(DatabaseService database) {
		SpotService = new SpotTransactionService(database.Database);
	}

	[HttpPost]
	public async Task<IActionResult> Purchase([FromBody] Transaction data) {
		try {
			Transaction response = await SpotService.Add(data);
			return Ok(response);
		} catch (Exception) {
			throw;
		}
	}

	[HttpGet]
	public async Task<List<Transaction>> Get() {
		try {
			return await SpotService.Get();
		} catch (Exception) {
			throw;
		}
	}

	[HttpGet]
	[Route("{id}")]
	public async Task<Transaction> GetById(string id) {
		try {
			return await SpotService.Get(id);
		} catch (Exception) {
			throw;
		}
	}
}

