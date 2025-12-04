
using Microsoft.EntityFrameworkCore;
using Mozaika.DataAccess.Models;
using System.Windows;
using System;

namespace Mozaika.WPF
{
    public partial class App : Application
    {
        // Настройки для подключения к БД
        private static DbContextOptions<MozaikaContext> _options;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            var connectionString = "Server=localhost;Database=MozaikaBD;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            // 2. Настраиваем опции для EF Core
            var optionsBuilder = new DbContextOptionsBuilder<MozaikaContext>();
            _options = optionsBuilder.UseSqlServer(connectionString).Options;   

            // 3. Проверка подключения к БД при запуске
            try
            {
                using (var context = CreateDbContext())
                {
                    context.Database.CanConnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось подключиться к базе данных: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }

            private void Application_Startup(object sender, StartupEventArgs e)
            {
                // Сначала открываем окно авторизации
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
            }
        

        public static MozaikaContext CreateDbContext()
        {
            return new MozaikaContext(_options);
        }
    }
}