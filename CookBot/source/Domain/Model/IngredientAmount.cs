using System;

namespace CookBot.Domain.Model
{
    [Serializable]
    public class IngredientAmount : IIngredientAmount
    {
        public string MeasureUnit { get; private set; }
        public double Count { get; private set; }

        public IngredientAmount(double count, string measureUnit)
        {
            MeasureUnit = measureUnit;
            Count = count;
        }
    }
}
