using BookingService.ViewModels;
using BookingService.Services;
using System.Windows;

namespace BookingService.MVVM.Views
{
    public partial class EditRoom : Window
    {
        public EditRoom()
        {
            InitializeComponent();
            var context = new AppDbContext();
            var roomService = new RoomService(context);
            var navigationService = new NavigationService();
            navigationService.SetCurrentWindow(this);
            DataContext = new EditRoomViewModel(roomService, navigationService);
        }
    }
}