using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Windows.Controls;
using System;

namespace BookingService
{
    public partial class DeleteRoom : Window
    {
        private readonly AppDbContext _context;
        private HotelRoom _selectedRoom;

        public static RoutedCommand SearchCommand { get; } = new RoutedCommand();
        public static RoutedCommand DeleteCommand { get; } = new RoutedCommand();
        public static RoutedCommand CancelCommand { get; } = new RoutedCommand();

        public DeleteRoom(AppDbContext context)
        {
            InitializeComponent();
            _context = context;

            CommandBindings.Add(new CommandBinding(SearchCommand, ExecuteSearchCommand));
            CommandBindings.Add(new CommandBinding(DeleteCommand, ExecuteDeleteCommand));
            CommandBindings.Add(new CommandBinding(CancelCommand, ExecuteCancelCommand));
        }

        private void ExecuteSearchCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SearchRooms(SearchTextBox.Text);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchPopup.IsOpen = false;
                return;
            }
            SearchRooms(SearchTextBox.Text);
        }

        private void SearchRooms(string searchText)
        {
            var results = _context.HotelRooms
                .Where(r => r.Name.Contains(searchText) || r.ShortDescription.Contains(searchText))
                .ToList();

            SearchResultsListBox.ItemsSource = results;
            SearchPopup.IsOpen = results.Any();
        }

        private void SearchResultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchResultsListBox.SelectedItem is HotelRoom selectedRoom)
            {
                _selectedRoom = selectedRoom;
                SelectedRoomName.Text = selectedRoom.Name;
                SelectedRoomDescription.Text = selectedRoom.ShortDescription;
                RoomInfoBorder.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                SearchPopup.IsOpen = false;
            }
        }

        private void ExecuteDeleteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (_selectedRoom == null) return;

            var result = MessageBox.Show($"Вы уверены, что хотите удалить номер '{_selectedRoom.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var command = new DeleteRoomCommand(_context, _selectedRoom);
                _context.ExecuteCommand(command);

                MessageBox.Show("Номер успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void ExecuteCancelCommand(object sender, ExecutedRoutedEventArgs e)
        {
            new AdminPanel().Show();
            this.Close();
        }
    }
}
