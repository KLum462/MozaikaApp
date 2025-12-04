namespace Mozaika.CalculationModule
{
    public class CostCalculator
    {
        public decimal CalculateTotalMaterialCost(IEnumerable<IRecipeComponent> components)
        {
            if (components == null || !components.Any()) return 0;

            decimal totalCost = 0;
            foreach (var component in components)
            {
                // ТУТ МЫ ЛОМАЕМ КОД (комментируем проверку)
                /*
                if (component.QuantityRequired < 0 || component.CostPerUnit < 0)
                {
                    throw new ArgumentException("...");
                }
                */

                totalCost += component.QuantityRequired * component.CostPerUnit;
            }
            return totalCost;
        }
    }
}