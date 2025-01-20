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
    /// Interaction logic for BooksHistory.xaml
    /// </summary>
    public partial class BooksHistory : Window
    {
        private ObservableCollection<IssuedBook> _issuedBooks;
        private ObservableCollection<ReturnedBook> _returnedBooks;
        public BooksHistory()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            _issuedBooks = new ObservableCollection<IssuedBook>();
            _returnedBooks = new ObservableCollection<ReturnedBook>();

            var connection = DBConnectionService.GetConnection();

            try
            {
                string query = "SELECT IRBooks.id, firstName, lastName, bookInfo, issueDate, returnDate FROM IRBooks INNER JOIN Readers ON IRBooks.ReaderId = Readers.ReaderId";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var readerName = reader.GetString(1);
                        var readerSurname = reader.GetString(2);
                        var bookInfo = reader.GetString(3);
                        var issueDate = reader.GetDateTime(4);
                        var returnDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5);
                        if (returnDate == null)
                            _issuedBooks.Add(new IssuedBook(id, readerName + " " + readerSurname, bookInfo, issueDate));
                        else
                            _returnedBooks.Add(new ReturnedBook(id, readerName + " " + readerSurname, bookInfo, issueDate, returnDate.Value));
                    }

                    IssuedBooksDataGrid.ItemsSource = _issuedBooks;
                    ReturnedBooksDataGrid.ItemsSource = _returnedBooks;
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
    }
}
