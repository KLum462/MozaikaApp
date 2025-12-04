using System.Windows;
using System.Windows.Input;
using Mozaika.WPF.ViewModels;
using Mozaika.DataAccess.Models; // <-- ПРОВЕРЬТЕ ЭТО: Тут должен лежать ваш класс Employee

namespace Mozaika.WPF
{
    public partial class MainWindow : Window
    {
        // Поле для хранения данных вошедшего сотрудника
        private Employee _currentUser;

        // --- НОВЫЙ КОНСТРУКТОР (Вызывается из окна входа) ---
        public MainWindow(Employee user)
        {
            InitializeComponent();

            _currentUser = user;  // Запоминаем, кто вошел
            SetupUserInterface(); // Меняем надпись "Гость" на имя сотрудника
        }

        // --- ПУСТОЙ КОНСТРУКТОР (Нужен, чтобы не ругался XAML-дизайнер) ---
        public MainWindow()
        {
            InitializeComponent();
        }

        // --- ЛОГИКА ОТОБРАЖЕНИЯ ИМЕНИ ПОЛЬЗОВАТЕЛЯ ---
        private void SetupUserInterface()
        {
            // Проверяем, что пользователь передан и TextBlock-и существуют
            if (_currentUser != null)
            {
                // Эти имена (UserNameTextBlock, UserRoleTextBlock) мы дали в XAML на прошлом шаге
                if (UserNameTextBlock != null)
                {
                    UserNameTextBlock.Text = _currentUser.FullName;
                }

                if (UserRoleTextBlock != null)
                {
                    // Если роль есть, пишем её название, иначе просто "Сотрудник"
                    UserRoleTextBlock.Text = _currentUser.Role != null ? _currentUser.Role.RoleName : "Сотрудник";
                }
            }
        }

        // --- ВАШ СТАРЫЙ КОД (РАБОТА С ТОВАРАМИ) ---

        // Метод добавления товара
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем окно для НОВОГО товара (передаем null)
            var newWindow = new ProductWindow(null);

            // Если пользователь нажал "Сохранить"
            if (newWindow.ShowDialog() == true)
            {
                RefreshData();
            }
        }

        // Метод для обновления данных на форме
        private void RefreshData()
        {
            DataContext = new ProductsViewModel();
        }

        // Метод двойного клика (редактирование)
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as ProductsViewModel;
            if (vm != null && vm.SelectedProduct != null)
            {
                var editWindow = new ProductWindow(vm.SelectedProduct);
                if (editWindow.ShowDialog() == true)
                {
                    RefreshData();
                }
            }
        }
    }
}