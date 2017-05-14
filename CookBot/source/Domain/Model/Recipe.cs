using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace CookBot.Domain.Model
{
    public class Recipe
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<IIngredient, IIngredientAmount> Components = new Dictionary<IIngredient, IIngredientAmount>();

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
