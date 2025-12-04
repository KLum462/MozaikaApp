// Mozaika.WPF/Adapters/RecipeComponentAdapter.cs
using Mozaika.CalculationModule;
using Mozaika.DataAccess.Models;

namespace Mozaika.WPF.Adapters
{
    // Адаптер позволяет использовать данные из БД в модуле расчета
    public class RecipeComponentAdapter : IRecipeComponent
    {
        private readonly ProductMaterial _productMaterial;

        public RecipeComponentAdapter(ProductMaterial productMaterial)
        {
            _productMaterial = productMaterial;
        }

        // Получаем требуемое количество
        public decimal QuantityRequired => _productMaterial.QuantityRequired ?? 0;

        // Получаем стоимость из связанной таблицы Materials (если она загружена)
        public decimal CostPerUnit => _productMaterial.Material?.CostPerUnit ?? 0;
    }
}