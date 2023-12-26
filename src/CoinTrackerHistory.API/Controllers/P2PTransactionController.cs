﻿using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.Transaction;
using CoinTrackerHistory.API.Services;
using CoinTrackerHistory.API.Services.Filter;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

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