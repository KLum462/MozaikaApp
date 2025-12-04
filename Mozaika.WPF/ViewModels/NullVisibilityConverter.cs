using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mozaika.WPF.ViewModels
{
    /// <summary>
    /// Конвертер, который показывает элемент, если значение NULL, и скрывает, если значение есть.
    /// Используется для показа надписи "Выберите продукт".
    /// </summary>
    public class NullVisibilityConverter : IValueConverter
    {
        // Создаем статический экземпляр, чтобы удобно использовать в XAML (x:Static)
        public static NullVisibilityConverter Instance { get; } = new NullVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если значение NULL (ничего не выбрано) -> Показываем (Visible)
            // Если значение есть (продукт выбран) -> Скрываем (Collapsed)
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}