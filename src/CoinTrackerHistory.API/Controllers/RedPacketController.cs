using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinTrackerHistory.API.Models.Transactions;
using CoinTrackerHistory.API.Services;
using CoinTrackerHistory.API.Services.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class RedPacketController : Controller {
	private readonly RedPacketTransactionService RedPacketService;

	public RedPacketController(DatabaseService database) {
		RedPacketService = new RedPacketTransactionService(database.Database);
	}

	[HttpGet]
	public async Task<List<RedPacketTransaction>> Get() {
		try {
			return await RedPacketService.Get();
		} catch (Exception) {
			throw;
		}
	}

	[HttpGet]
	[Route("{id}")]
	public async Task<RedPacketTransaction> GetById(string id) {
		try {
			return await RedPacketService.Get(id);
		} catch (Exception) {
			throw;
		}
	}
}

