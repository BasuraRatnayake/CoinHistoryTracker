using CoinTrackerHistory.API.Models.Transactions;
using CoinTrackerHistory.API.Services;
using CoinTrackerHistory.API.Services.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers.Transactions;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class RedPacketTransactionController : Controller {
	private readonly RedPacketTransactionService RedPacketService;

	public RedPacketTransactionController(DatabaseService database) {
		RedPacketService = new RedPacketTransactionService(database.Database);
	}

	[HttpPost]
	public async Task<IActionResult> Add([FromBody] RedPacketTransaction data) {
		try {
			RedPacketTransaction response = await RedPacketService.Add(data);
			return Ok(response);
		} catch (Exception) {
			throw;
		}
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

