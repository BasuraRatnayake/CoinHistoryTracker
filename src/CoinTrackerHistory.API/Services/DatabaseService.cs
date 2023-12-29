using MongoDB.Driver;

namespace CoinTrackerHistory.API.Services;

public sealed class DatabaseService {
	private const string CONNECTION_STR = "mongodb://localhost:27017";
	private const string DATABASE_NAME = "CoinTrackerHistory";

	public IMongoDatabase Database { get; set; }

	public DatabaseService() {
		try {
			MongoClient client = new MongoClient(CONNECTION_STR);
			Database = client.GetDatabase(DATABASE_NAME);
		} catch (Exception) {
			throw;
		}
	}
}
