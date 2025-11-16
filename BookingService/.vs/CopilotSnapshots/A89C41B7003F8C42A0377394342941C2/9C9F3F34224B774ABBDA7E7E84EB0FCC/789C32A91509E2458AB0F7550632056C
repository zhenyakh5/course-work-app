using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Windows.Controls;
using System;

namespace BookingService
{
    public partial class EditRoom : Window
    {
        private readonly AppDbContext _context;
        private HotelRoom _roomToEdit;

        public static RoutedCommand SearchCommand { get; } = new RoutedCommand();
        public static RoutedCommand BrowseCommand { get; } = new RoutedCommand();
        public static RoutedCommand SaveCommand { get; } = new RoutedCommand();
        public static RoutedCommand CancelCommand { get; } = new RoutedCommand();

        public EditRoom(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            CategoryComboBox.SelectedIndex = 0;

            CommandBindings.Add(new CommandBinding(SearchCommand, ExecuteSearchCommand));
            CommandBindings.Add(new CommandBinding(BrowseCommand, ExecuteBrowseCommand));
            CommandBindings.Add(new CommandBinding(SaveCommand, ExecuteSaveCommand));
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
                _roomToEdit = selectedRoom;
                LoadRoomData(selectedRoom.Id);
                SearchPopup.IsOpen = false;
                EditFormScrollViewer.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Visible;
            }
        }

        private void LoadRoomData(int roomId)
        {
            _roomToEdit = _context.HotelRooms.Find(roomId);
            if (_roomToEdit == null) return;

            NameTextBox.Text = _roomToEdit.Name;
            ShortDescriptionTextBox.Text = _roomToEdit.ShortDescription;
            FullDescriptionTextBox.Text = _roomToEdit.FullDescription;
            ImagePathTextBox.Text = _roomToEdit.ImagePath;
            UpdateImagePreview(_roomToEdit.ImagePath);

            foreach (ComboBoxItem item in CategoryComboBox.Items)
            {
                if (item.Content.ToString() == _roomToEdit.Category)
                {
                    CategoryComboBox.SelectedItem = item;
                    break;
                }
            }

            RatingTextBox.Text = _roomToEdit.Rating.ToString();
            PriceTextBox.Text = _roomToEdit.Price.ToString();
            NumberOfBedsTextBox.Text = _roomToEdit.NumberOfBeds.ToString();
            StarsTextBox.Text = _roomToEdit.Stars.ToString();
            AmenitiesTextBox.Text = _roomToEdit.Amenities;
            HasBalconyCheckBox.IsChecked = _roomToEdit.HasBalcony;
            IsNonSmokingCheckBox.IsChecked = _roomToEdit.IsNonSmoking;
        }

        private void ExecuteBrowseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = openFileDialog.FileName;
                UpdateImagePreview(openFileDialog.FileName);
            }
        }

        private void ImagePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateImagePreview(ImagePathTextBox.Text);
        }

        private void UpdateImagePreview(string imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ImagePreview.Source = bitmap;
                }
                else
                {
                    ImagePreview.Source = null;
                }
            }
            catch
            {
                ImagePreview.Source = null;
            }
        }

        private void ExecuteSaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!ValidateInputs() || _roomToEdit == null) return;

            var editedRoom = new HotelRoom
            {
                Id = _roomToEdit.Id,
                Name = NameTextBox.Text,
                ShortDescription = ShortDescriptionTextBox.Text,
                FullDescription = FullDescriptionTextBox.Text,
                ImagePath = ImagePathTextBox.Text,
                Category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Rating = double.Parse(RatingTextBox.Text),
                Price = (double)decimal.Parse(PriceTextBox.Text),
                NumberOfBeds = int.Parse(NumberOfBedsTextBox.Text),
                Amenities = AmenitiesTextBox.Text,
                Stars = int.Parse(StarsTextBox.Text),
                HasBalcony = HasBalconyCheckBox.IsChecked ?? false,
                IsNonSmoking = IsNonSmokingCheckBox.IsChecked ?? false,
                IsAvailable = true
            };

            var command = new EditRoomCommand(_context, _roomToEdit, editedRoom);
            _context.ExecuteCommand(command);

            MessageBox.Show("Изменения сохранены успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите название номера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ShortDescriptionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите краткое описание", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(FullDescriptionTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите полное описание", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ImagePathTextBox.Text) || !File.Exists(ImagePathTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, выберите корректное изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!double.TryParse(RatingTextBox.Text, out double rating) || rating < 0 || rating > 5)
            {
                MessageBox.Show("Рейтинг должен быть числом от 0 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(NumberOfBedsTextBox.Text, out int beds) || beds <= 0)
            {
                MessageBox.Show("Количество кроватей должно быть положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(StarsTextBox.Text, out int stars) || stars < 1 || stars > 5)
            {
                MessageBox.Show("Количество звёзд должно быть от 1 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(AmenitiesTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, укажите удобства", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ExecuteCancelCommand(object sender, ExecutedRoutedEventArgs e)
        {
            new AdminPanel().Show();
            this.Close();
        }
    }
}
