using System;

namespace source.Domain.Model
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
