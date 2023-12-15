using CoinTrackerHistory.API.Models.DTO;

namespace CoinTrackerHistory.API.Services;

public class SellService : PurchaseService {
	public SellService() : base(PurchaseType.Sell) { }
}

