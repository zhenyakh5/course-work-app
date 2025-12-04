using BookingService.Services;
using System.Windows.Input;

namespace BookingService.ViewModels
{
    public class AdminPanelViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ICommand AddRoomCommand { get; }
        public ICommand EditRoomCommand { get; }
        public ICommand DeleteRoomCommand { get; }

        public AdminPanelViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            AddRoomCommand = new RelayCommand(AddRoom);
            EditRoomCommand = new RelayCommand(EditRoom);
            DeleteRoomCommand = new RelayCommand(DeleteRoom);
        }

        private void AddRoom(object parameter)
        {
            _navigationService.NavigateToAddRoom();
        }

        private void EditRoom(object parameter)
        {
            _navigationService.NavigateToEditRoom();
        }

        private void DeleteRoom(object parameter)
        {
            _navigationService.NavigateToDeleteRoom();
        }
    }
}