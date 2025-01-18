using library_management.Model;
using library_management.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for ViewBooks.xaml
    /// </summary>
    public partial class ViewBooks : Window
    {
        private ObservableCollection<Book> books;
        public ViewBooks()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }

        private void LoadBooks(string query = "SELECT * FROM Books")
        {
            books = new ObservableCollection<Book>();

            var connection = DBConnectionService.GetConnection();

            try
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var title = reader.GetString(1);
                        var author = reader.GetString(2);
                        var publication = reader.GetString(3);
                        var price = reader.GetDecimal(4);
                        books.Add(new Book(id, title, author, publication, price));
                    }
                }

                BooksDataGrid.ItemsSource = books;
                
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

        private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedBook = BooksDataGrid.SelectedItem as Book;

            if (selectedBook != null)
            {
                ChangeBookDetails changeBookDetailsWindow = new ChangeBookDetails(
                    selectedBook.ID,
                    selectedBook.Title,
                    selectedBook.Author,
                    selectedBook.Publication,
                    selectedBook.Price)
                {
                    Owner = this
                };
                changeBookDetailsWindow.ShowDialog();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadBooks($"SELECT * FROM Books WHERE title LIKE '{BookTextBox.Text}%'");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }
    }
}
