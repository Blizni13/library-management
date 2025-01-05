using library_management.Services;
using library_management.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace library_management.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _username;
        public string Username 
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Password { get; set; }

        public ICommand UsernameGotFocusCommand { get; }
        public ICommand PasswordGotFocusCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainWindowViewModel()
        {
            Username = "Username";
            Password = "Password";
            UsernameGotFocusCommand = new RelayCommand(UsernameGotFocus);
            PasswordGotFocusCommand = new RelayCommand(PasswordGotFocus);
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
        }

        public void UsernameGotFocus()
        {
            if (Username.Equals("Username"))
                Username = String.Empty;
        }
        
        public void PasswordGotFocus()
        {
            if (Password.Equals("Password"))
                Password = String.Empty;
        }

        public void Login()
        {
            MySqlConnection connection = DBConnectionService.GetConnection();

            try
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Users WHERE username = @username and password = @password";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

                dataAdapter.SelectCommand.Parameters.AddWithValue("@username", Username);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@password", Password);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Logged in successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // open the next window
                    Dashboard dashboard = new Dashboard();
                    dashboard.Show();

                    // close the login-register window
                    Application.Current.Windows[0]?.Close();

                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }

        public void Register()
        {
            if (DBOperationService.CheckRowExistence("Users", ("username", Username)))
            {
                MessageBox.Show(
                    "The username is already in use. Please try a different one.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            MySqlConnection connection = DBConnectionService.GetConnection();

            try
            {
                string query = "INSERT INTO Users (username, password) VALUES (@username, @password)";
                MySqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@username", Username);
                command.Parameters.AddWithValue("@password", Password);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Registration successful! You can now log in with your new account.");
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again later.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }
    }
}
