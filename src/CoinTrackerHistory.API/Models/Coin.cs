using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoinTrackerHistory.API.Models;

public class Coin {
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	public required string Name { get; set; }
	public required decimal Quantity { get; set; }
}
