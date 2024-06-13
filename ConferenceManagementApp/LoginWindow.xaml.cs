using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConferenceManagementApp
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            if (ValidateCredentials(username, password))
            {
                // Если авторизация успешна, создаем и открываем главное окно
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                errorLabel.Content = "Неправильное имя пользователя или пароль";
            }
            if (ValidateCredentials2(username, password))
            {
                // Если авторизация успешна, создаем и открываем главное окно
                var mainWindow = new IVANWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                errorLabel.Content = "Неправильное имя пользователя или пароль";
            }
            if (ValidateCredentials3(username, password))
            {
                // Если авторизация успешна, создаем и открываем главное окно
                var mainWindow = new IVANWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                errorLabel.Content = "Неправильное имя пользователя или пароль";
            }
            if (ValidateCredentials4(username, password))
            {
                // Если авторизация успешна, создаем и открываем главное окно
                var mainWindow = new YCHWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                errorLabel.Content = "Неправильное имя пользователя или пароль";
            }
        }

        private bool ValidateCredentials(string username, string password)
        {
            // Вставьте здесь логику проверки данных (база данных, API, etc.)
            // В этом примере используем простую проверку:
            if (username == "Глава" && password == "123")
            {
                return true;
            }
            return false;
        }
        private bool ValidateCredentials2(string username, string password)
        {
            // Вставьте здесь логику проверки данных (база данных, API, etc.)
            // В этом примере используем простую проверку:
            if (username == "Иван" && password == "123")
            {
                return true;
            }
            return false;
        }
        private bool ValidateCredentials3(string username, string password)
        {
            // Вставьте здесь логику проверки данных (база данных, API, etc.)
            // В этом примере используем простую проверку:
            if (username == "Петр" && password == "123")
            {
                return true;
            }
            return false;
        }
        private bool ValidateCredentials4(string username, string password)
        {
            // Вставьте здесь логику проверки данных (база данных, API, etc.)
            // В этом примере используем простую проверку:
            if (username == "Участник" && password == "123")
            {
                return true;
            }
            return false;
        }
    }
}
