using Microsoft.EntityFrameworkCore;
using Mozaika.DataAccess; // Убедитесь, что подключили пространство имен вашей БД
using Mozaika.DataAccess.Models;
using System.Linq;
using System.Windows;   
namespace Mozaika.WPF
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Используем ваш контекст базы данных
            using (var context = new MozaikaContext())
            {
                // Ищем пользователя в БД, включая его Роль
                var user = context.Employees
                                  .Include(u => u.Role)
                                  .FirstOrDefault(u => u.Login == login && u.Password == password);

                if (user != null)
                {
                    // Успешная авторизация
                    MainWindow mainWindow = new MainWindow(user); // Передаем пользователя в главное окно
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}