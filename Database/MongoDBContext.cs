using MongoDB.Bson;
using MongoDB.Driver;
using CookingApp.Models;
using System;

namespace CookingApp.Database
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext()
        {
            string connectionString = Environment.GetEnvironmentVariable("MONGO_DB_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("MongoDB connection string is not set.");
            }

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("CookingApp");

            try
            {
                client.ListDatabases();
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Timeout vid anslutning: {ex.Message}");
            }
            catch (MongoException ex)
            {
                Console.WriteLine($"MongoDB fel vid anslutning: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid anslutning till databasen: {ex.Message}");
            }
        }

        public IMongoCollection<Recipe> Recipes => _database.GetCollection<Recipe>("Recipes");
    }
}