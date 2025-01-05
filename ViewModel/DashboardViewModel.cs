using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace library_management.ViewModel
{
    class DashboardViewModel
    {
        public ICommand ExitCommand { get; }

        public DashboardViewModel()
        {
            ExitCommand = new RelayCommand(Exit);
        }

        public void Exit()
        {
            MessageBoxResult result
                = MessageBox.Show("Are you sure you want to exit?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
            
        }
    }
}
