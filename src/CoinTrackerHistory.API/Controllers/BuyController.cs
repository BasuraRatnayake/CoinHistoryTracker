using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

[ApiController]
[Route("api/v1/buy")]
[Produces("application/json")]
public class BuyController : Controller {
    private readonly BuyService service;

    public BuyController(BuyService service) {
        this.service = service;
	}

    [HttpGet]
	public async Task<IActionResult> Get(int? page = 1, int? limit = 5) {
        try {
			List<TransactionHistory> response = await service.Get();
			return Ok(response);
		} catch (NotFoundException ex) {
			return NotFound(ex.Message);
		}
	}
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetById([FromRoute]string id) {
        try {
			TransactionHistory response = await service.GetById(id);
			return Ok(response);
		} catch (BadRequestException ex) {
			return BadRequest();
		} catch (NotFoundException ex) {
			return NotFound(ex.Message);
		}
	}
    [HttpGet]
    [Route("filter")]
    public async Task<IActionResult> GetByColumns([FromBody] List<FilterColumn> filters, int? page = 1, int? limit = 5) {
        List<TransactionHistory> response = await service.GetByFilter(filters);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]TransactionHistory data) {
        TransactionHistory response = await service.Add(data);
        return Ok(response);
    }
}
