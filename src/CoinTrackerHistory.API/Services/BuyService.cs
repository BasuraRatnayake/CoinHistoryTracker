using CoinTrackerHistory.API.Models.DTO;

namespace CoinTrackerHistory.API.Services;

public class BuyService : PurchaseService {
	public BuyService() : base(PurchaseType.Buy) { }
}
