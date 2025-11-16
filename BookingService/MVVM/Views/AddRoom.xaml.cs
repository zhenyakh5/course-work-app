using BookingService.ViewModels;
using BookingService.Services;
using System.Windows;

namespace BookingService.MVVM.Views
{
    public partial class AddRoom : Window
    {
        public AddRoom()
        {
            InitializeComponent();
            var context = new AppDbContext();
            var roomService = new RoomService(context);
            var navigationService = new NavigationService();
            navigationService.SetCurrentWindow(this);
            DataContext = new AddRoomViewModel(roomService, navigationService);
        }
    }
}