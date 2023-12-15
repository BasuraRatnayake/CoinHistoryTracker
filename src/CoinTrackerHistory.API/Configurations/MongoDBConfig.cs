namespace CoinTrackerHistory.API.Configurations;

public sealed class MongoDBConfig {
    public readonly string CONNECTION_STR = "mongodb://localhost:27017";
    public readonly string DATABASE_NAME = "CoinTrackerHistory";
}
