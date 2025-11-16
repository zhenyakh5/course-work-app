using BookingService.MVVM.Services;
using BookingService.Services;
using BookingService.ViewModels;
using System.Windows;

namespace BookingService.MVVM.Views
{
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
            var context = new AppDbContext();
            var roomService = new RoomService(context);
            var navigationService = new NavigationService();
            navigationService.SetCurrentWindow(this);
            DataContext = new AdminPanelViewModel(navigationService);
        }
    }
}