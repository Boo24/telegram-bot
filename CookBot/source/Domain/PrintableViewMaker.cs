using source.Domain.Model;

namespace source.Domain
{
    public static class PrintableViewMaker
    {
        public static string GetPrintableView(this Recipe recipe)
        {
            string result = $"*Название: {recipe.Name}\n*Рецепт: {recipe.Description}\n*Ингридиенты:\n";
            foreach (var component in recipe.Components)
                result += $"-{component.Key.Name} {component.Value.Count} {component.Value.MeasureUnit}\n";
            return result;
        }
    }
}
