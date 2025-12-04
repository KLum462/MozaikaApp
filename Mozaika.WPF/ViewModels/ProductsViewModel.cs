using Microsoft.EntityFrameworkCore;
using Mozaika.CalculationModule;
using Mozaika.DataAccess.Models;
using Mozaika.WPF.Adapters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Windows;

namespace Mozaika.WPF.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        // Полный список продуктов (из него будем фильтровать)
        private List<Product> _allProducts;

        // Список, который отображается на экране
        public ObservableCollection<Product> Products { get; set; }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                CalculateCost();
            }
        }

        private decimal _calculatedCost;
        public decimal CalculatedCost
        {
            get => _calculatedCost;
            set
            {
                _calculatedCost = value;
                OnPropertyChanged();
            }
        }

        // Текст поиска
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterProducts(); // При вводе буквы сразу фильтруем
            }
        }

        private readonly CostCalculator _calculator;

        public ProductsViewModel()
        {
            Products = new ObservableCollection<Product>();
            _allProducts = new List<Product>();
            _calculator = new CostCalculator();

            LoadProductsAsync();
        }

        private async void LoadProductsAsync() // void для fire-and-forget в конструкторе
        {
            try
            {
                using (var context = App.CreateDbContext())
                {
                    // Загружаем данные
                    var productsFromDb = await context.Products
                        .Include(p => p.Category)
                        .Include(p => p.ProductMaterials)
                            .ThenInclude(pm => pm.Material)
                        .ToListAsync();

                    // Сохраняем в полный список
                    _allProducts = productsFromDb;

                    // И отображаем всё
                    UpdateProductsList(_allProducts);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void FilterProducts()
        {
            // Если поиск пустой - показываем всё
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                UpdateProductsList(_allProducts);
                return;
            }

            // Ищем ТОЛЬКО по названию (так как описания нет в БД)
            var filtered = _allProducts.Where(p =>
                p.ProductName.ToLower().Contains(SearchText.ToLower()))
                .ToList();

            UpdateProductsList(filtered);
        }

        private void UpdateProductsList(IEnumerable<Product> list)
        {
            Products.Clear();
            foreach (var p in list)
            {
                Products.Add(p);
            }
        }

        private void CalculateCost()
        {
            if (SelectedProduct == null)
            {
                CalculatedCost = 0;
                return;
            }

            try
            {
                var components = SelectedProduct.ProductMaterials
                    .Select(pm => new RecipeComponentAdapter(pm))
                    .ToList();

                CalculatedCost = _calculator.CalculateTotalMaterialCost(components);
            }
            catch
            {
                CalculatedCost = 0;
            }
        }
    }
}