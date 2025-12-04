using Microsoft.EntityFrameworkCore;
using Mozaika.DataAccess.Models;
using System;
using System.Linq;
using System.Windows;

namespace Mozaika.WPF
{
    public partial class ProductWindow : Window
    {
        public Product CurrentProduct { get; private set; }

        public ProductWindow(Product product)
        {
            InitializeComponent();

            if (product == null)
            {
                CurrentProduct = new Product();
                DeleteBtn.Visibility = Visibility.Collapsed;
                Title = "Новый товар";
            }
            else
            {
                CurrentProduct = product;
                Title = "Редактирование: " + product.ProductName;
                DeleteBtn.Visibility = Visibility.Visible; // <--- А ТУТ ПОКАЗЫВАЕТ
            }

            DataContext = CurrentProduct;
            LoadCategories();
        }

        private void LoadCategories()
        {
            using (var context = App.CreateDbContext())
            {
                var categories = context.ProductCategories.ToList();
                CategoryCombo.ItemsSource = categories;

                // ИСПРАВЛЕНО: CategoryId вместо CategoryID
                if (CurrentProduct.CategoryId != null)
                {
                    CategoryCombo.SelectedValue = CurrentProduct.CategoryId;
                }
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentProduct.ProductName))
            {
                MessageBox.Show("Введите название товара!");
                return;
            }

            if (CategoryCombo.SelectedValue != null)
            {
                // ИСПРАВЛЕНО: CategoryId вместо CategoryID
                CurrentProduct.CategoryId = (int)CategoryCombo.SelectedValue;
            }

            using (var context = App.CreateDbContext())
            {
                // ИСПРАВЛЕНО: ProductId вместо ProductID
                if (CurrentProduct.ProductId == 0)
                {
                    context.Products.Add(CurrentProduct);
                }
                else
                {
                    context.Products.Update(CurrentProduct);
                }

                try
                {
                    context.SaveChanges();
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка сохранения: " + ex.Message);
                }
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы точно хотите удалить этот товар?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using (var context = App.CreateDbContext())
                {
                    context.Products.Remove(CurrentProduct);
                    context.SaveChanges();
                    DialogResult = true;
                }
            }
        }
    }
}