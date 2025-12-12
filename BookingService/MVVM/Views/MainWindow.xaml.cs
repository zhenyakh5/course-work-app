using System.Windows;
using BookingService.MVVM.ViewModels;

namespace BookingService.MVVM.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainWindowViewModel();
            DataContext = vm;

            AdminPanelButton.Visibility = (SessionManager.CurrentUser?.IsAdmin == true)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public void ShowAdminPanel()
        {
            AdminPanelButton.Visibility = Visibility.Visible;
        }
    }
}