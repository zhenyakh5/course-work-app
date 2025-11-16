using BookingService.MVVM.Views;
using System.Windows;

namespace BookingService.Services
{
    public class NavigationService : INavigationService
    {
        private Window _currentWindow;

        public void SetCurrentWindow(Window window)
        {
            _currentWindow = window;
        }

        public void NavigateToAdminPanel()
        {
            var adminPanel = new AdminPanel();
            adminPanel.Show();
            CloseWindow();
        }

        public void NavigateToAddRoom()
        {
            var addRoom = new AddRoom();
            addRoom.Show();
            CloseWindow();
        }

        public void NavigateToEditRoom()
        {
            var editRoom = new EditRoom();
            editRoom.Show();
            CloseWindow();
        }

        public void NavigateToDeleteRoom()
        {
            var deleteRoom = new DeleteRoom();
            deleteRoom.Show();
            CloseWindow();
        }

        public void CloseWindow()
        {
            _currentWindow?.Close();
        }
    }
}