using System;

namespace CookBot.Domain.Model
{
    [Serializable]
    public class Ingredient : IIngredient
    {
        public string Name { get; }

        public Ingredient(string name)
        {
            Name = name;
        }
    }
}
