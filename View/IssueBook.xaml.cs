using library_management.DTO;
using library_management.Model;
using library_management.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for IssueBook.xaml
    /// </summary>
    public partial class IssueBook : Window
    {
        public ObservableCollection<BookDTO> Books { get; set; } 

        public IssueBook()
        {
            DataContext = this;
            LoadBooksFromDb();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputFields())
                return;
            AddIssuedBookToDb();
        }

        private bool ValidateInputFields()
        {
            bool result = false;

            if (String.IsNullOrEmpty(ReaderIdTextBox.Text))
                MessageBoxService.ShowErrorBox("You have to provide reader id.");
            else if (!DoesReaderExist(ReaderIdTextBox.Text))
                MessageBoxService.ShowErrorBox("Select correct reader id. This reader does not exist.");
            else if (BooksComboBox.SelectedItem == null)
                MessageBoxService.ShowErrorBox("Select book that you want to issue.");
            else if (IsBookIssued(BooksComboBox.SelectedItem.ToString()))
                MessageBoxService.ShowErrorBox("Selected book is already issued.");
            else if (BookIssueDatePicker.SelectedDate == null)
                MessageBoxService.ShowErrorBox("Select date.");
            else
                result = true;

            return result;
        }

        private bool DoesReaderExist(string readerId)
        {
            return DBOperationService.CheckRowExistence("Readers", ("ReaderId", readerId));
        }

        private bool IsBookIssued(string bookInfo)
        {
            return DBOperationService.CheckRowExistence("IssuedBooks", ("BookInfo", bookInfo));
        }

        private void LoadBooksFromDb()
        {
            var connection = DBConnectionService.GetConnection();

            try
            {
                string query = "SELECT title, author FROM Books";

                MySqlCommand command = new MySqlCommand(query, connection);

                Books = new ObservableCollection<BookDTO>();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var title = reader.GetString(0);
                        var author = reader.GetString(1);
                        Books.Add(new BookDTO(title, author));
                    }
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

        private void AddIssuedBookToDb()
        {
            var connection = DBConnectionService.GetConnection();

            try
            {
                string query = "INSERT INTO IssuedBooks (ReaderId, BookInfo, IssueDate) VALUES (@ReaderId, @BookInfo, @IssueDate)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReaderId", ReaderIdTextBox.Text);
                command.Parameters.AddWithValue("@BookInfo", BooksComboBox.SelectedItem);
                command.Parameters.AddWithValue("@IssueDate", BookIssueDatePicker.SelectedDate);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBoxService.ShowSuccessBox("Book successfully issued.");
                }
                else
                {
                    MessageBoxService.ShowErrorBox("Book couldn't be successfully added, try again.");
                }
            } 
            catch (Exception ex)
            {
                MessageBoxService.ShowErrorBox($"An error occured {ex.Message}");
            } 
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }
    }
}
