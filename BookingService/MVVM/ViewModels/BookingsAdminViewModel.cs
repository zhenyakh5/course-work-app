using BookingService.MVVM.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.Entity;

namespace BookingService.MVVM.ViewModels
{
    public class BookingsAdminViewModel : ViewModelBase
    {
        private readonly AppDbContext _context = new AppDbContext();

        public ObservableCollection<Booking> Bookings { get; set; } = new ObservableCollection<Booking>();

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CloseCommand { get; }

        public BookingsAdminViewModel(ICommand closeCommand = null)
        {
            LoadBookings();
            ConfirmCommand = new RelayCommand(ConfirmBooking);
            CancelCommand = new RelayCommand(CancelBooking);
            CloseCommand = closeCommand ?? new RelayCommand(o => {});
        }

        public void LoadBookings()
        {
            try
            {
                var bookings = _context.Bookings.Include("User").Include("Room").ToList();
                Bookings.Clear();
                foreach (var b in bookings)
                    Bookings.Add(b);
            }
            catch
            {
            }
        }

        private void ConfirmBooking(object parameter)
        {
            if (parameter is Booking booking)
            {
                try
                {
                    var b = _context.Bookings.FirstOrDefault(x => x.Id == booking.Id);
                    if (b != null)
                    {
                        b.Status = "Подтверждено";

                        _context.SaveChanges();
                        LoadBookings();
                        _context.NotifyDataChanged();
                        MessageBox.Show("Бронь подтверждена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelBooking(object parameter)
        {
            if (parameter is Booking booking)
            {
                try
                {
                    var b = _context.Bookings.FirstOrDefault(x => x.Id == booking.Id);
                    if (b != null)
                    {
                        b.Status = "Отменено";

                        _context.SaveChanges();
                        LoadBookings();
                        _context.NotifyDataChanged();
                        MessageBox.Show("Бронь отменена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}