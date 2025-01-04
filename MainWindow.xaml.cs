using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace library_management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TxtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text.Equals("Username"))
            {
                txtUsername.Clear();
            }
        }
        
        private void TxtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Equals("Password"))
            {
                txtPassword.Clear();
            }
        }

        private void ShowPasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            string password = txtPassword.Password;
            MessageBox.Show($"The entered password is: {password}");
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {

            MySqlConnection connection = DBConnectionService.GetConnection();

            try
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM Users WHERE username = @username and password = @password";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

                dataAdapter.SelectCommand.Parameters.AddWithValue("@username", txtUsername.Text);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@password", txtPassword.Password);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Hide();
                    MessageBox.Show("Logged in successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information );
                } 
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckRowExistence( "Users", ("username", txtUsername.Text)))
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

                command.Parameters.AddWithValue("@username", txtUsername.Text);
                command.Parameters.AddWithValue("@password", txtPassword.Password);

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
            catch(Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }

        private bool CheckRowExistence(string tableName, params (string columnName, string columnValue)[] pairs)
        {
            var connection = DBConnectionService.GetConnection();

            try
            {
                string query = $"SELECT * FROM {tableName} WHERE ";
                int n = pairs.Length;
                for (int i = 0; i < n; i++)
                {
                    query += 
                        i == n - 1 
                        ?
                        $"{pairs[i].columnName} = '{pairs[i].columnValue}'" 
                        :
                        $"{pairs[i].columnName} = '{pairs[i].columnValue}' and ";
                }

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds.Tables[0].Rows.Count > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }
    }
}