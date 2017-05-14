using System.Collections.Generic;
using System.Linq;
using CookBot.Domain.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace CookBot.Domain.Databases
{
    public class MongoDatabase : IDatabase
    {
        IMongoCollection<Recipe> collection;

        public MongoDatabase(string databaseName, string collectionName)
        {
            var client = new MongoClient();
            var db = client.GetDatabase(databaseName);
            collection = db.GetCollection<Recipe>(collectionName);

            BsonClassMap.RegisterClassMap<IngredientAmount>();
            BsonClassMap.RegisterClassMap<Ingredient>();
        }

        public Recipe GetRecipeByName(string name) =>
            collection.Find(recipe => recipe.Name == name).First();

        public IEnumerable<Recipe> GetRecipesByIngredient(IIngredient ingredient) =>
            collection.Find(recipe => recipe.Components.ContainsKey(ingredient)).ToEnumerable();

        // сделал для удобства, потом надо будет убрать
        public void InsertRecipe(Recipe r) =>
            collection.InsertOne(r);
    }
}
