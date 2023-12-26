using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinTrackerHistory.API.Models;
using CoinTrackerHistory.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class CoinController : Controller {
	private readonly CoinWalletService CoinWalletService;

	public CoinController(DatabaseService database) {
		CoinWalletService = new CoinWalletService(database.Database);
	}

	[HttpGet]
	public async Task<List<Coin>> Get() {
		return await CoinWalletService.Get();
	}

	[HttpGet("{name}")]
	public async Task<Coin> Get(string name) {
		return await CoinWalletService.Get(name);
	}
}
