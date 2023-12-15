using System;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Interfaces;

public interface IPurchaseController {
	public Task<IActionResult> Get(int page = 1, int limit = 5);
	public Task<ActionResult> GetById([FromRoute] string id);
	public Task<IActionResult> GetByColumns([FromBody] List<RecordFilter> filters, int page = 1, int limit = 5);

	public Task<IActionResult> Add([FromBody] TransactionHistory data);
}
