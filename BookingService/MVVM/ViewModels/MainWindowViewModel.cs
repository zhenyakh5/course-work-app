using BookingService.MVVM.Models;
using BookingService.MVVM.Views;
using BookingService.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BookingService.MVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AppDbContext _context = new AppDbContext();
        private List<HotelRoom> _originalRooms = new List<HotelRoom>();

        public ObservableCollection<HotelRoom> Rooms { get; set; } = new ObservableCollection<HotelRoom>();

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value ?? string.Empty;
                OnPropertyChanged();
                RefreshData();
            }
        }

        private DateTime _checkInDate = DateTime.Now.Date;
        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                _checkInDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _checkOutDate = DateTime.Now.Date.AddDays(1);
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                _checkOutDate = value;
                OnPropertyChanged();
            }
        }

        private bool _onlyAvailable = false;

        public ICommand ProfileCommand { get; }
        public ICommand AdminPanelCommand { get; }
        public ICommand AlphabetSortCommand { get; }
        public ICommand CostAscSortCommand { get; }
        public ICommand CostDescSortCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand OpenRoomCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand ClearFilterCommand { get; }

        public MainWindowViewModel()
        {
            ProfileCommand = new RelayCommand(OpenProfile);
            AdminPanelCommand = new RelayCommand(OpenAdminPanel);
            AlphabetSortCommand = new RelayCommand(_ => SortAlphabet());
            CostAscSortCommand = new RelayCommand(_ => SortCostAsc());
            CostDescSortCommand = new RelayCommand(_ => SortCostDesc());
            RefreshCommand = new RelayCommand(_ => LoadRooms());
            OpenRoomCommand = new RelayCommand(OpenRoom);

            ApplyFilterCommand = new RelayCommand(_ => ApplyFilter());
            ClearFilterCommand = new RelayCommand(_ => ClearFilter());

            _context.DataChanged += () => System.Windows.Application.Current.Dispatcher.Invoke(LoadRooms);
            LoadRooms();
        }

        public void LoadRooms()
        {
            try
            {
                _originalRooms = _context.HotelRooms.AsNoTracking().ToList();
                UpdateRoomsCollection(_originalRooms);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void UpdateRoomsCollection(IEnumerable<HotelRoom> rooms)
        {
            Rooms.Clear();
            foreach (var room in rooms)
                Rooms.Add(room);
        }

        private void RefreshData()
        {
            var filter = SearchText?.ToLower() ?? string.Empty;
            var filteredRooms = string.IsNullOrEmpty(filter)
                ? _originalRooms
                : _originalRooms.Where(r => r.Name != null && r.Name.ToLower().Contains(filter));

            if (_onlyAvailable)
            {
                var checkIn = CheckInDate.Date;
                var checkOut = CheckOutDate.Date;
                if (checkOut <= checkIn)
                {
                    UpdateRoomsCollection(filteredRooms);
                    return;
                }

                filteredRooms = filteredRooms.Where(r => IsRoomAvailableForDates(r.Id, checkIn, checkOut));
            }

            UpdateRoomsCollection(filteredRooms);
        }

        private void SortAlphabet()
        {
            UpdateRoomsCollection(_originalRooms.OrderBy(r => r.Name));
        }

        private void SortCostAsc()
        {
            UpdateRoomsCollection(_originalRooms.OrderBy(r => r.Price));
        }

        private void SortCostDesc()
        {
            UpdateRoomsCollection(_originalRooms.OrderByDescending(r => r.Price));
        }

        private void OpenProfile(object parameter)
        {
            var wnd = new ProfileWindow();
            wnd.Show();
        }

        private void OpenAdminPanel(object parameter)
        {
            var wnd = new AdminPanel();
            wnd.Show();
        }

        private void OpenRoom(object parameter)
        {
            if (parameter == null) return;
            if (int.TryParse(parameter.ToString(), out var roomId))
            {
                var vm = new RoomViewModel(roomId);
                var wnd = new RoomWindow();
                wnd.DataContext = vm;
                wnd.Show();
            }
        }

        private bool IsRoomAvailableForDates(int roomId, DateTime checkIn, DateTime checkOut)
        {
            try
            {
                var overlapping = _context.Bookings.Where(b => b.RoomId == roomId && (b.Status == "Подтверждено" || b.Status == "Ожидание") && !(b.CheckOutDate <= checkIn || b.CheckInDate >= checkOut)).Any();
                return !overlapping;
            }
            catch
            {
                return false;
            }
        }

        private void ApplyFilter()
        {
            if (CheckOutDate.Date <= CheckInDate.Date)
            {
                MessageBox.Show("Дата выезда должна быть позже даты заезда.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _onlyAvailable = true;
            RefreshData();
        }

        private void ClearFilter()
        {
            _onlyAvailable = false;
            CheckInDate = DateTime.Now.Date;
            CheckOutDate = DateTime.Now.Date.AddDays(1);
            OnPropertyChanged(nameof(CheckInDate));
            OnPropertyChanged(nameof(CheckOutDate));
            RefreshData();
        }
    }
}
