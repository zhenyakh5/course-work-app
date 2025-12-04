using System.Windows;
using BookingService.MVVM.ViewModels;

namespace BookingService.MVVM.Views
{
    public partial class BookingsAdmin : Window
    {
        public BookingsAdmin()
        {
            InitializeComponent();

            if (DataContext == null)
                DataContext = new BookingsAdminViewModel();

            Loaded += BookingsAdmin_Loaded;
        }

        private void BookingsAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is BookingsAdminViewModel vm)
            {
                vm.LoadBookings();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}