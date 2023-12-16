using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Interfaces;
using CoinTrackerHistory.API.Models;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

[Produces("application/json")]
public class PurchaseController : Controller, IPurchaseController {
	public readonly IPurchaseService service;

	public PurchaseController(IPurchaseService service) {
		this.service = service;
	}

	[HttpGet]
	public async Task<IActionResult> Get(int page = 1, int limit = 5) {
		try {
			List<TransactionHistory> response = await service.Get(page, limit);
			return Ok(response);
		} catch (BadRequestException ex) {
			return BadRequest(ex.Message);
		} catch (NotFoundException ex) {
			return NotFound(ex.Message);
		} catch (InternalServerException ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet]
	[Route("{id}")]
	public async Task<ActionResult> GetById([FromRoute] string id) {
		try {
			TransactionHistory response = await service.GetById(id);
			return Ok(response);
		} catch (BadRequestException ex) {
			return BadRequest(ex.Message);
		} catch (NotFoundException ex) {
			return NotFound(ex.Message);
		}
	}
	[HttpPost]
	[Route("filter")]
	public async Task<ActionResult> GetByFilter([FromBody] List<FilterTemplate> filters, int page = 1, int limit = 5) {
		try {
			List<TransactionHistory> response = await service.Get(page, limit, filters);
			return Ok(response);
		} catch (BadRequestException ex) {
			return BadRequest(ex.Message);
		} catch (NotFoundException ex) {
			return NotFound(ex.Message);
		}
	}

	[HttpPost]
	public async Task<IActionResult> Add([FromBody] TransactionHistory data) {
		try {
			TransactionHistory response = await service.Add(data);
			return Ok(response);
		} catch (BadRequestException ex) {
			return BadRequest(ex.Message);
		} catch (InternalServerException ex) {
			return BadRequest(ex.Message);
		}
	}
}
