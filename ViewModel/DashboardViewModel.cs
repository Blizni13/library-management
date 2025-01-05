using library_management.Services;
using library_management.View;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace library_management.ViewModel
{
    class DashboardViewModel
    {
        // windows
        private AddBook? _addBookWindow;

        // commands
        public ICommand ExitCommand { get; }
        public ICommand OpenAddBookCommand { get; }

        public DashboardViewModel()
        {
            _addBookWindow = null;
            ExitCommand = new RelayCommand(Exit);
            OpenAddBookCommand = new RelayCommand(OpenAddBook);
        }

        public void Exit()
        {
            MessageBoxResult result
                = MessageBox.Show("Are you sure you want to exit?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
            
        }

        public void OpenAddBook()
        {
            if (_addBookWindow == null)
            {
                _addBookWindow = new AddBook()
                {
                    DataContext = new AddBookViewModel(this)
                };
                _addBookWindow.Show();
            }
            else
            {
                _addBookWindow.Activate();
            }
        }

        public void CloseAddBook()
        {
            _addBookWindow?.Close();
        }
    }
}
