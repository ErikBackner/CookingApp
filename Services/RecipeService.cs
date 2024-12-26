using MongoDB.Bson;
using MongoDB.Driver;
using CookingApp.Models;
using System;
using System.Collections.Generic;

namespace CookingApp.Services
{
    public class RecipeService
    {
        private readonly IMongoCollection<Recipe> _recipes;

        public RecipeService()
        {
            string connectionString = Environment.GetEnvironmentVariable("MONGO_DB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("MongoDB connection string is not set.");
            }

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("CookingApp");
            _recipes = database.GetCollection<Recipe>("Recipes");

            var existingIndexes = _recipes.Indexes.List().ToList();
            bool indexExists = existingIndexes.Any(idx => idx["name"] == "CustomId_1");

            if (!indexExists)
            {
                var indexKeysDefinition = Builders<Recipe>.IndexKeys.Ascending(r => r.CustomId);
                _recipes.Indexes.CreateOne(new CreateIndexModel<Recipe>(indexKeysDefinition));
            }
        }

        public List<Recipe> GetAllRecipes()
        {
            return _recipes.Find(recipe => true).ToList();
        }

        public Recipe GetRecipeById(string customId)
        {
            return _recipes.Find(recipe => recipe.CustomId == customId).FirstOrDefault();
        }

        public void AddRecipe(Recipe recipe)
        {
            _recipes.InsertOne(recipe);
        }

        public void UpdateRecipe(string customId, Recipe updatedRecipe)
        {
            var recipe = _recipes.Find(r => r.CustomId == customId).FirstOrDefault();
            if (recipe == null)
            {
                Console.WriteLine("Receptet med angivet ID finns inte.");
                return;
            }

            var result = _recipes.ReplaceOne(r => r.CustomId == customId, updatedRecipe);
            if (result.ModifiedCount == 0)
            {
                throw new Exception("Receptet kunde inte uppdateras.");
            }
        }

        public void DeleteRecipe(string customId)
        {
            var recipe = _recipes.Find(r => r.CustomId == customId).FirstOrDefault();
            if (recipe == null)
            {
                Console.WriteLine("Receptet med angivet ID finns inte.");
                return;
            }

            _recipes.DeleteOne(r => r.CustomId == customId);
            Console.WriteLine("Receptet har tagits bort.");
        }

        public void AddRatingToRecipe(string customId, int rating)
        {
            var recipe = _recipes.Find(r => r.CustomId == customId).FirstOrDefault();
            if (recipe != null)
            {
                recipe.Ratings.Add(rating);
                UpdateRecipe(customId, recipe);
            }
            else
            {
                Console.WriteLine("Receptet med angivet ID finns inte.");
            }
        }

        public void AddCommentToRecipe(string customId, string comment)
        {
            var recipe = _recipes.Find(r => r.CustomId == customId).FirstOrDefault();
            if (recipe != null)
            {
                recipe.Comments.Add(comment);
                UpdateRecipe(customId, recipe);
            }
            else
            {
                Console.WriteLine("Receptet med angivet ID finns inte.");
            }
        }
    }
}
