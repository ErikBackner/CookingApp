using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CookingApp.Models
{
    public class Recipe
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string CustomId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<int> Ratings { get; set; } = new List<int>();

        public List<string> Comments { get; set; } = new List<string>();

        public Recipe()
        {
            CustomId = Guid.NewGuid().ToString();
        }
    }
}