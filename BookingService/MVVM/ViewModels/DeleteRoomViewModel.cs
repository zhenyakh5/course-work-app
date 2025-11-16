using BookingService.MVVM.Models;
using BookingService.MVVM.Views;
using BookingService.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingService.ViewModels
{
    public class DeleteRoomViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<HotelRoom> _searchResults;
        private HotelRoom _selectedRoom;
        private string _searchText;
        private bool _isSearchPopupOpen;

        public ObservableCollection<HotelRoom> SearchResults
        {
            get => _searchResults;
            set { _searchResults = value; OnPropertyChanged(); }
        }

        public HotelRoom SelectedRoom
        {
            get => _selectedRoom;
            set { _selectedRoom = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsRoomSelected)); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); SearchRooms(); }
        }

        public bool IsSearchPopupOpen
        {
            get => _isSearchPopupOpen;
            set { _isSearchPopupOpen = value; OnPropertyChanged(); }
        }

        public bool IsRoomSelected => SelectedRoom != null;

        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        public DeleteRoomViewModel(IRoomService roomService, INavigationService navigationService)
        {
            _roomService = roomService;
            _navigationService = navigationService;
            SearchResults = new ObservableCollection<HotelRoom>();

            DeleteCommand = new RelayCommand(DeleteRoom, CanDeleteRoom);
            CancelCommand = new RelayCommand(Cancel);
        }

        private async void SearchRooms()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchResults.Clear();
                IsSearchPopupOpen = false;
                return;
            }

            var results = await _roomService.SearchRoomsAsync(SearchText);
            SearchResults.Clear();
            foreach (var room in results)
            {
                SearchResults.Add(room);
            }

            IsSearchPopupOpen = SearchResults.Any();
        }

        private bool CanDeleteRoom(object parameter)
        {
            return IsRoomSelected;
        }

        private async void DeleteRoom(object parameter)
        {
            if (SelectedRoom == null) return;

            var result = MessageBox.Show($"Вы уверены, что хотите удалить номер '{SelectedRoom.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var success = await _roomService.DeleteRoomAsync(SelectedRoom.Id);
                if (success)
                {
                    MessageBox.Show("Номер успешно удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    _navigationService.NavigateToAdminPanel();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении номера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Cancel(object parameter)
        {
            _navigationService.NavigateToAdminPanel();
        }
    }
}