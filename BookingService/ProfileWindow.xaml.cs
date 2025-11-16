using System;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;

namespace BookingService
{
    public partial class ProfileWindow : Window
    {
        private readonly AppDbContext _context = new AppDbContext();
        private User _currentUser;

        public static RoutedCommand SaveCommand { get; } = new RoutedCommand();
        public static RoutedCommand LanguageChange { get; } = new RoutedCommand();
        public static RoutedCommand ThemeChange { get; } = new RoutedCommand();

        public ProfileWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(SaveCommand, ExecuteSaveCommand));
            LoadUserData();
        }

        private void LoadUserData()
        {
            _currentUser = _context.Users.FirstOrDefault(u => u.Id == SessionManager.CurrentUser.Id);
            if (_currentUser != null)
            {
                UsernameTextBox.Text = _currentUser.Username;
            }
        }

        private void ExecuteSaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {

                _currentUser.Username = UsernameTextBox.Text;
                _context.SaveChanges();

                MessageBox.Show("Изменения сохранены успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}