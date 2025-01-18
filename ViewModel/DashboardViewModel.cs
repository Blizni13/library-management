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
        private ViewBooks? _viewBooksWindow;
        private AddReader? _addReaderWindow;
        private ViewReaders? _viewReadersWindow;
        private IssueBook? _issueBookWindow;
        private ReturnBook? _returnBookWindow;

        // commands
        public ICommand ExitCommand { get; }
        public ICommand OpenAddBookCommand { get; }
        public ICommand OpenViewBooksCommand { get; }
        public ICommand OpenAddReaderCommand { get; }
        public ICommand OpenViewReadersCommand { get; }
        public ICommand OpenIssueBookCommand { get; }
        public ICommand OpenReturnBookCommand { get; }

        public DashboardViewModel()
        {
            _addBookWindow = null;
            ExitCommand = new RelayCommand(Exit);
            OpenAddBookCommand = new RelayCommand(OpenAddBook);
            OpenViewBooksCommand = new RelayCommand(OpenViewBooks);
            OpenAddReaderCommand = new RelayCommand(OpenAddReader);
            OpenViewReadersCommand = new RelayCommand(OpenViewReaders);
            OpenIssueBookCommand = new RelayCommand(OpenIssueBook);
            OpenReturnBookCommand = new RelayCommand(OpenReturnBook);
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

        public void OpenViewBooks()
        {
            if (!WindowService.IsWindowOpen<ViewBooks>())
            {
                _viewBooksWindow = new ViewBooks();
                _viewBooksWindow.Show();
            }
            else
            {
                _viewBooksWindow?.Activate();
            }
        }

        public void OpenAddReader()
        {
            if (_addReaderWindow == null)
            {
                _addReaderWindow = new AddReader()
                {
                    DataContext = new AddReaderViewModel(this)
                };
                _addReaderWindow.Show();
            }
            else
            {
                _addReaderWindow.Activate();
            }
        }

        public void OpenViewReaders()
        {
            if (!WindowService.IsWindowOpen<ViewReaders>())
            {
                _viewReadersWindow = new ViewReaders();
                _viewReadersWindow.Show();
            }
            else
            {
                _viewReadersWindow?.Activate();
            }
        }
        
        public void OpenIssueBook()
        {
            if (!WindowService.IsWindowOpen<IssueBook>())
            {
                _issueBookWindow = new IssueBook();
                _issueBookWindow.Show();
            }
            else
            {
                _issueBookWindow?.Activate();
            }
        }
        
        public void OpenReturnBook()
        {
            if (!WindowService.IsWindowOpen<ReturnBook>())
            {
                _returnBookWindow = new ReturnBook();
                _returnBookWindow.Show();
            }
            else
            {
                _returnBookWindow?.Activate();
            }
        }

        public void CloseAddBook()
        {
            _addBookWindow?.Close();
            _addBookWindow = null;
        }

        public void CloseAddReader()
        {
            _addReaderWindow?.Close();
            _addReaderWindow = null;
        }
    }
}
