using BookingService.MVVM.Models;
using BookingService.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingService.MVVM.ViewModels
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly INavigationService _navigationService;

        private HotelRoom _room;
        public HotelRoom Room
        {
            get => _room;
            set
            {
                _room = value;
                OnPropertyChanged();
                UpdateAvailabilityInfo();
                OnPropertyChanged(nameof(RoomHasBalconyText));
                OnPropertyChanged(nameof(RoomIsNonSmokingText));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string RoomIsAvailableText => Room != null ? (IsRoomAvailableForDates(Room.Id, CheckInDate, CheckOutDate) ? "Да" : "Нет") : string.Empty;
        public string RoomHasBalconyText => Room != null ? (Room.HasBalcony ? "Да" : "Нет") : string.Empty;
        public string RoomIsNonSmokingText => Room != null ? (!Room.IsNonSmoking ? "Да" : "Нет") : string.Empty;

        private DateTime _checkInDate = DateTime.Now.Date;
        private DateTime _checkOutDate = DateTime.Now.Date.AddDays(1);

        private string _overlappingBookingMessage = string.Empty;
        public string OverlappingBookingMessage
        {
            get => _overlappingBookingMessage;
            set { _overlappingBookingMessage = value; OnPropertyChanged(); }
        }

        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                _checkInDate = value;
                OnPropertyChanged();
                UpdateAvailabilityInfo();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                _checkOutDate = value;
                OnPropertyChanged();
                UpdateAvailabilityInfo();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool IsBookable => Room != null && SessionManager.CurrentUser != null && CheckOutDate > CheckInDate && IsRoomAvailableForDates(Room.Id, CheckInDate, CheckOutDate);

        public ICommand BookCommand { get; }

        public RoomViewModel(int roomId)
        {
            LoadRoom(roomId);
            BookCommand = new RelayCommand(ExecuteBook, CanExecuteBook);
        }

        private void LoadRoom(int roomId)
        {
            try
            {
                Room = _context.HotelRooms.FirstOrDefault(r => r.Id == roomId);
            }
            catch (Exception)
            {
                Room = null;
            }
        }

        private void UpdateAvailabilityInfo()
        {
            OnPropertyChanged(nameof(RoomIsAvailableText));

            if (Room == null)
            {
                OverlappingBookingMessage = string.Empty;
                return;
            }

            var overlapping = FindOverlappingBooking(Room.Id, CheckInDate, CheckOutDate);
            if (overlapping != null)
            {
                OverlappingBookingMessage = $"Выбранный Вами номер забронирован с {overlapping.CheckInDate:dd/MM/yyyy} по {overlapping.CheckOutDate:dd/MM/yyyy}.";
            }
            else
            {
                OverlappingBookingMessage = string.Empty;
            }

            CommandManager.InvalidateRequerySuggested();
            OnPropertyChanged(nameof(IsBookable));
        }

        private Booking FindOverlappingBooking(int roomId, DateTime checkIn, DateTime checkOut)
        {
            try
            {
                var booking = _context.Bookings
                    .Where(b => b.RoomId == roomId && b.Status == "Подтверждено" && !(b.CheckOutDate <= checkIn || b.CheckInDate >= checkOut))
                    .OrderBy(b => b.CheckInDate)
                    .FirstOrDefault();

                return booking;
            }
            catch
            {
                return null;
            }
        }

        private bool IsRoomAvailableForDates(int roomId, DateTime checkIn, DateTime checkOut)
        {
            try
            {
                var overlapping = _context.Bookings.Where(b => b.RoomId == roomId && b.Status == "Подтверждено" && !(b.CheckOutDate <= checkIn || b.CheckInDate >= checkOut)).Any();
                return !overlapping;
            }
            catch
            {
                return false;
            }
        }

        private bool CanExecuteBook(object parameter)
        {
            return IsBookable;
        }

        private void ExecuteBook(object parameter)
        {
            if (SessionManager.CurrentUser == null)
            {
                MessageBox.Show("Пожалуйста, войдите в систему, чтобы забронировать номер.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CheckOutDate <= CheckInDate)
            {
                MessageBox.Show("Дата выезда должна быть позже даты заезда.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsRoomAvailableForDates(Room.Id, CheckInDate, CheckOutDate))
            {
                MessageBox.Show("Номер недоступен на выбранные даты.", "Недоступно", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите забронировать номер?", "Подтвердите бронь", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                var days = (decimal)(CheckOutDate - CheckInDate).TotalDays;
                if (days <= 0) days = 1;

                var booking = new Booking
                {
                    UserId = SessionManager.CurrentUser.Id,
                    RoomId = Room.Id,
                    CheckInDate = CheckInDate,
                    CheckOutDate = CheckOutDate,
                    Status = "Ожидание",
                    TotalPrice = Room.Price * days,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Bookings.Add(booking);
                _context.SaveChanges();

                _context.NotifyDataChanged();

                MessageBox.Show("Бронь создана и отправлена на подтверждение.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateAvailabilityInfo();
                _navigationService.CloseWindow();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании брони: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}