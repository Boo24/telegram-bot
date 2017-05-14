namespace CookBot.Domain.Model
{
    public interface IIngredientAmount
    {
        string MeasureUnit { get; }
        double Count { get; }
    }
}
