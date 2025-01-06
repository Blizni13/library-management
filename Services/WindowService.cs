using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace library_management.Services
{
    public class WindowService
    {
        public static bool IsWindowOpen<T>() where T: Window
        {
            return Application.Current.Windows.OfType<T>().Any();
        }
    }
}
