using System;

namespace source.Domain.Model
{
    [Serializable]
    public class IngredientAmount : IIngredientAmount
    {
        public string MeasureUnit { get; }
        public double Count { get; }

        public IngredientAmount(double count, string measureUnit)
        {
            MeasureUnit = measureUnit;
            Count = count;
        }
    }
}
