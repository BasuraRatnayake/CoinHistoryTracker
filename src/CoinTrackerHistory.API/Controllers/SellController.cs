using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

[ApiController]
[Route("api/v1/sell")]
public class SellController : PurchaseController {

    public SellController(SellService service) : base (service) { }
}
