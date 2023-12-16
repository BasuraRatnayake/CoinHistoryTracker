using CoinTrackerHistory.API.Models;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Interfaces;

public interface IPurchaseController {
	public Task<IActionResult> Get(int page = 1, int limit = 5);
	public Task<ActionResult> GetById([FromRoute] string id);
	public Task<ActionResult> GetByFilter([FromBody] List<FilterTemplate> filters, int page = 1, int limit = 5);

	public Task<IActionResult> Add([FromBody] TransactionHistory data);
}
