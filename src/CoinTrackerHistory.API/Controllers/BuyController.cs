﻿using CoinTrackerHistory.API.Exceptions;
using CoinTrackerHistory.API.Models.DTO;
using CoinTrackerHistory.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinTrackerHistory.API.Controllers;

[ApiController]
[Route("api/v1/buy")]
public class BuyController : PurchaseController {

    public BuyController(BuyService service) : base (service) { }
}
