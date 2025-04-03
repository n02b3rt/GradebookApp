using System.Windows;
using System.Windows.Controls;
using GradebookApp.ViewModels;

namespace GradebookApp.Views
{
    public partial class ConnectionWindow : Window
    {
        public ConnectionWindow()
        {
            InitializeComponent();
            DataContext = new ConnectionViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConnectionViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
