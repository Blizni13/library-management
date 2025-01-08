using library_management.Model;
using library_management.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Reflection.Metadata.BlobBuilder;

namespace library_management.View
{
    /// <summary>
    /// Interaction logic for ViewReaders.xaml
    /// </summary>
    public partial class ViewReaders : Window
    {
        private ObservableCollection<Reader> readers;
        public ViewReaders()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReaders();
        }
        private void LoadReaders(string query = "SELECT * FROM Readers")
        {
            readers = new ObservableCollection<Reader>();

            var connection = DBConnectionService.GetConnection();

            try
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var readerId = reader.GetInt32(0);
                        var firstName = reader.GetString(1);
                        var lastName = reader.GetString(2);
                        var dateOfBirth = reader.GetDateTime(3);
                        var gender = reader.GetString(4);
                        var address = reader.GetString(5);
                        var membershipStartDate = reader.GetDateTime(6);
                        readers.Add(
                            new Reader(readerId, firstName, lastName, dateOfBirth, gender, address, membershipStartDate)
                        );
                    }
                }

                DataGrid.ItemsSource = readers;

            }
            catch (Exception ex)
            {
                MessageBoxService.ShowErrorBox($"An error occured: {ex.Message}");
            }
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedReader = DataGrid.SelectedItem as Reader;

            if (selectedReader != null)
            {
                ChangeReaderDetails changeReaderDetailsWindow = new ChangeReaderDetails(
                    selectedReader.ReaderId,
                    selectedReader.FirstName,
                    selectedReader.LastName,
                    selectedReader.DateOfBirth,
                    selectedReader.Gender,
                    selectedReader.Address,
                    selectedReader.MembershipStartDate)
                {
                    Owner = this
                };
                changeReaderDetailsWindow.ShowDialog();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadReaders($"SELECT * FROM Readers WHERE FirstName LIKE '{TextBox.Text}%'");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadReaders();
        }
    }
}
