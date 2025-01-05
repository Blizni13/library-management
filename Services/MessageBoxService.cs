using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace library_management.Services
{
    public class MessageBoxService
    {
        public static void ShowSuccessBox(string message, string? title = null)
        {
            MessageBox.Show(message, title ?? "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        public static void ShowErrorBox(string message, string? title = null)
        {
            MessageBox.Show(message, title ?? "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        public static void ShowWarningBox(string message, string? title = null)
        {
            MessageBox.Show(message, title ?? "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

    }
}
