using library_management.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace library_management.ViewModel
{
    internal class AddBookViewModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publication { get; set; }
        public decimal Price { get; set; }

        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly DashboardViewModel _parentViewModel;
        public AddBookViewModel(DashboardViewModel parentViewModel)
        {
            _parentViewModel = parentViewModel;
            Title = String.Empty;
            Author = String.Empty;
            Publication = String.Empty;
            AddCommand = new RelayCommand(Add);
            CancelCommand = new RelayCommand(Cancel);
        }
        public void Add()
        {
            if (!ValidateFields())
            {
                MessageBoxService.ShowErrorBox("Please fill in all required fields with the correct data.");
                return;
            }
            // check if this book wasn't already added
            if (DBOperationService.CheckRowExistence(
                    "Books",
                    [
                        ("title", Title),
                        ("author", Author),
                        ("publication", Publication),
                        ("price", Price.ToString(CultureInfo.InvariantCulture)),
                    ]
                ))
            {
                MessageBoxService.ShowErrorBox("This book was already added.");
                return;
            }

            // add book
            var connection = DBConnectionService.GetConnection();

            try
            {
                string query 
                  = "INSERT INTO Books (title, author, publication, price) " +
                    "VALUES (@title, @author, @publication, @price)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@title", Title);
                command.Parameters.AddWithValue("@author", Author);
                command.Parameters.AddWithValue("@publication", Publication);
                command.Parameters.AddWithValue("@price", Price);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBoxService.ShowSuccessBox("Book added successfully");
                }
                else
                {
                    MessageBoxService.ShowErrorBox("There was an issue connecting to the database. Please try again later.");
                }
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowErrorBox($"An error occured: {ex.Message}", "Error");
            }
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }
        
        public void Cancel()
        {
            _parentViewModel.CloseAddBook();
        }
        private bool ValidateFields()
        {
            if (String.IsNullOrEmpty(Title) || String.IsNullOrEmpty(Author) || String.IsNullOrEmpty(Publication))
                return false;
            else if (Price <= 0)
                return false;
            return true;
        }
    }
}
