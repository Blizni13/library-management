using library_management.ViewModel;
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

namespace library_management.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void TxtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.UsernameGotFocusCommand.Execute(null);
            }
        }
        
        private void TxtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.PasswordGotFocusCommand.Execute(null);
                txtPassword.Password = viewModel.Password;
                    
            }
        }

        private void ShowPasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            string password = txtPassword.Password;
            MessageBox.Show($"The entered password is: {password}");
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.Password = txtPassword.Password;
            }
        }
    }
}