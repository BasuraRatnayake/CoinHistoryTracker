using CoinTrackerHistory.API.Configurations;
using CoinTrackerHistory.API.Models.DTO;
using MongoDB.Driver;

namespace CoinTrackerHistory.API.Services;

public class BuyService {
    private MongoDBConfig mongoDBConfig = new MongoDBConfig();
    private readonly string collectionName = "TransactionHistory";

    private IMongoCollection<TransactionHistory> collection = null;

    public BuyService() {
        try {
            MongoClient client = new MongoClient(mongoDBConfig.CONNECTION_STR);
            IMongoDatabase database = client.GetDatabase(mongoDBConfig.DATABASE_NAME);
            collection = database.GetCollection<TransactionHistory>(collectionName);
        } catch (Exception) {

        } 
    }

    public async Task<TransactionHistory> GetLastInserted() {
        return await collection.Find(_ => true)
            .SortByDescending(h => h.CreatedAt)
            .Limit(1)
            .FirstOrDefaultAsync();
    }

    public async Task<List<TransactionHistory>> FindAll() {
        try {
            return await collection
                .Find(c => c.CoinPurchaseType == PurchaseType.Buy)
                .SortBy(c => c.CreatedAt)
                .ToListAsync();
        } catch (Exception) {
            throw;
        }
    }

    public async Task<TransactionHistory> Add(TransactionHistory coin) {
        try {
            coin.Id = null;
            coin.CoinPurchaseType = PurchaseType.Buy;
            coin.CreatedAt = DateTime.Now;

            await coin.Calculate();
            await collection.InsertOneAsync(coin);

            return await GetLastInserted();
        } catch (Exception ex) {
            throw;
        }
    }
}
