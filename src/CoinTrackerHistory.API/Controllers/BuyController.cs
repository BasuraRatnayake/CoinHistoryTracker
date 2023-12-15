using System.Collections.Generic;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

[ApiController]
[Route("api/v1/buy")]
public class BuyController : Controller {
    private readonly BuyService service;

    public BuyController(BuyService service) {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get() {
        List<TransactionHistory> response = await service.FindAll();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]TransactionHistory data) {
        TransactionHistory response = await service.Add(data);
        return Ok(response);
    }
}
