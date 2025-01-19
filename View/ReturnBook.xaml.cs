using library_management.DTO;
using library_management.Model;
using library_management.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
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

namespace library_management.View
{
    /// <summary>
    /// Interaction logic for ReturnBook.xaml
    /// </summary>
    public partial class ReturnBook : Window
    {

        private ObservableCollection<IssuedBook> _books;

        public ReturnBook()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(SearchByReaderIdTextBox.Text))
                LoadBooks($"SELECT * FROM IRBooks WHERE ReaderId = {SearchByReaderIdTextBox.Text} AND ReturnDate IS NULL");
            else
                LoadBooks();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }
        
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }
            else if (!DBOperationService.CheckRowExistence("IRBooks", ("Id", IdTextBox.Text)))
            {
                MessageBoxService.ShowErrorBox("Data with this id does not exist. Please check provided id and try again.");
                return;
            }

            var connection = DBConnectionService.GetConnection();

            try
            {
                string query = "UPDATE IRBooks SET ReturnDate = @ReturnDate WHERE Id = @Id";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReturnDate", ReturnDatePicker.SelectedDate);
                command.Parameters.AddWithValue("@Id", IdTextBox.Text);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBoxService.ShowSuccessBox("Book returned successfully.");
                } 
                else
                {
                    MessageBoxService.ShowErrorBox("An error occurred while updating your data. Please try again.");
                }

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

        private void LoadBooks(string query = "SELECT * FROM IRBooks WHERE ReturnDate IS NULL")
        {
            _books = new ObservableCollection<IssuedBook>();

            var connection = DBConnectionService.GetConnection();

            try
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var readerId = reader.GetInt32(1);
                        var bookInfo = reader.GetString(2);
                        var issueDate = reader.GetDateTime(3);
                        _books.Add(new IssuedBook(id, readerId, bookInfo, issueDate));
                    }
                }

                DataGrid.ItemsSource = _books;

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

        private bool ValidateFields()
        {
            if (String.IsNullOrEmpty(IdTextBox.Text))
            {
                MessageBoxService.ShowErrorBox("Please, provide id field.");
                return false;
            }
            else if (ReturnDatePicker.SelectedDate == null)
            {
                MessageBoxService.ShowErrorBox("Please, provide return date field.");
                return false;
            } else if (IsReturnDateBeforeIssueDate(IdTextBox.Text, ReturnDatePicker.SelectedDate.Value))
            {
                MessageBoxService.ShowErrorBox("Provided return date is incorrect. Return date is earlier than issue date.");
                return false;
            }
            return true;
        }

        private bool IsReturnDateBeforeIssueDate(string id, DateTime returnDate)
        {
            foreach (var book in _books)
                if (book.ID.Equals(int.Parse(id)))
                    return returnDate < book.IssueDate;

            return false;
        }
    }
}
