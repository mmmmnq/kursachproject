using Npgsql;
using System;
using System.Windows;

namespace kursovaya.Views
{
    public partial class RegisterForm : Window
    {
        public RegisterForm()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные от пользователя
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Password;
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string phone_number = numberTextBox.Text;

            // Проверка на заполнение всех полей
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(phone_number))
            {
                MessageBox.Show("Заполните все поля!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Подключаемся к базе данных
            string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL-запрос для добавления нового пользователя
                    string query = "INSERT INTO users (username, password, role, firstname, lastname, phonenumber) " +
                                   "VALUES (@username, @password, 'user', @firstname, @lastname, @phonenumber)";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        // Передаем значения параметров
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("password", password);
                        cmd.Parameters.AddWithValue("firstname", firstName);
                        cmd.Parameters.AddWithValue("lastname", lastName);
                        cmd.Parameters.AddWithValue("phonenumber", phone_number);

                        // Выполняем запрос
                        cmd.ExecuteNonQuery();

                        // Сообщение об успехе
                        MessageBox.Show("Регистрация успешно выполнена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close(); // Закрываем форму регистрации
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
