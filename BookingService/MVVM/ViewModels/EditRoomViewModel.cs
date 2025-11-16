using BookingService.MVVM.Models;
using BookingService.MVVM.Views;
using BookingService.Services;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BookingService.ViewModels
{
    public class EditRoomViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<HotelRoom> _searchResults;
        private HotelRoom _selectedRoom;
        private string _searchText;
        private bool _isFormVisible;
        private bool _isSearchPopupOpen;
        private string _name;
        private string _shortDescription;
        private string _fullDescription;
        private string _imagePath;
        private string _category = "Стандарт";
        private string _rating;
        private string _price;
        private string _numberOfBeds;
        private string _stars;
        private string _amenities;
        private bool _hasBalcony;
        private bool _isNonSmoking;
        private BitmapImage _imagePreview;

        public ObservableCollection<HotelRoom> SearchResults
        {
            get => _searchResults;
            set { _searchResults = value; OnPropertyChanged(); }
        }

        public HotelRoom SelectedRoom
        {
            get => _selectedRoom;
            set { _selectedRoom = value; OnPropertyChanged(); LoadRoomData(); }
        }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); SearchRooms(); }
        }

        public bool IsFormVisible
        {
            get => _isFormVisible;
            set { _isFormVisible = value; OnPropertyChanged(); }
        }

        public bool IsSearchPopupOpen
        {
            get => _isSearchPopupOpen;
            set { _isSearchPopupOpen = value; OnPropertyChanged(); }
        }

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string ShortDescription { get => _shortDescription; set { _shortDescription = value; OnPropertyChanged(); } }
        public string FullDescription { get => _fullDescription; set { _fullDescription = value; OnPropertyChanged(); } }
        public string ImagePath { get => _imagePath; set { _imagePath = value; OnPropertyChanged(); UpdateImagePreview(); } }
        public string Category { get => _category; set { _category = value; OnPropertyChanged(); } }
        public string Rating { get => _rating; set { _rating = value; OnPropertyChanged(); } }
        public string Price { get => _price; set { _price = value; OnPropertyChanged(); } }
        public string NumberOfBeds { get => _numberOfBeds; set { _numberOfBeds = value; OnPropertyChanged(); } }
        public string Stars { get => _stars; set { _stars = value; OnPropertyChanged(); } }
        public string Amenities { get => _amenities; set { _amenities = value; OnPropertyChanged(); } }
        public bool HasBalcony { get => _hasBalcony; set { _hasBalcony = value; OnPropertyChanged(); } }
        public bool IsNonSmoking { get => _isNonSmoking; set { _isNonSmoking = value; OnPropertyChanged(); } }
        public BitmapImage ImagePreview { get => _imagePreview; set { _imagePreview = value; OnPropertyChanged(); } }

        public string[] Categories { get; } = { "Стандарт", "Люкс", "Делюкс", "Президентский" };

        public ICommand BrowseImageCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditRoomViewModel(IRoomService roomService, INavigationService navigationService)
        {
            _roomService = roomService;
            _navigationService = navigationService;
            SearchResults = new ObservableCollection<HotelRoom>();

            BrowseImageCommand = new RelayCommand(BrowseImage);
            SaveCommand = new RelayCommand(Save, CanSave);
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

        private void LoadRoomData()
        {
            if (SelectedRoom == null) return;

            Name = SelectedRoom.Name;
            ShortDescription = SelectedRoom.ShortDescription;
            FullDescription = SelectedRoom.FullDescription;
            ImagePath = SelectedRoom.ImagePath;
            Category = SelectedRoom.Category;
            Rating = SelectedRoom.Rating.ToString();
            Price = SelectedRoom.Price.ToString();
            NumberOfBeds = SelectedRoom.NumberOfBeds.ToString();
            Stars = SelectedRoom.Stars.ToString();
            Amenities = SelectedRoom.Amenities;
            HasBalcony = SelectedRoom.HasBalcony;
            IsNonSmoking = SelectedRoom.IsNonSmoking;

            UpdateImagePreview();
            IsFormVisible = true;
            IsSearchPopupOpen = false;

            // Notify property changes
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(ShortDescription));
            OnPropertyChanged(nameof(FullDescription));
            OnPropertyChanged(nameof(ImagePath));
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(Rating));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(NumberOfBeds));
            OnPropertyChanged(nameof(Stars));
            OnPropertyChanged(nameof(Amenities));
            OnPropertyChanged(nameof(HasBalcony));
            OnPropertyChanged(nameof(IsNonSmoking));
        }

        private void BrowseImage(object parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
            }
        }

        private void UpdateImagePreview()
        {
            try
            {
                if (File.Exists(ImagePath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(ImagePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ImagePreview = bitmap;
                }
                else
                {
                    ImagePreview = null;
                }
            }
            catch
            {
                ImagePreview = null;
            }
        }

        private bool CanSave(object parameter)
        {
            return SelectedRoom != null &&
                   !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(ShortDescription) &&
                   !string.IsNullOrWhiteSpace(FullDescription) &&
                   !string.IsNullOrWhiteSpace(ImagePath) &&
                   !string.IsNullOrWhiteSpace(Rating) &&
                   !string.IsNullOrWhiteSpace(Price) &&
                   !string.IsNullOrWhiteSpace(NumberOfBeds) &&
                   !string.IsNullOrWhiteSpace(Stars) &&
                   !string.IsNullOrWhiteSpace(Amenities);
        }

        private async void Save(object parameter)
        {
            if (SelectedRoom == null) return;

            if (!ValidateInputs()) return;

            var updatedRoom = new HotelRoom
            {
                Id = SelectedRoom.Id,
                Name = Name,
                ShortDescription = ShortDescription,
                FullDescription = FullDescription,
                ImagePath = ImagePath,
                Category = Category,
                Rating = double.Parse(Rating),
                Price = decimal.Parse(Price),
                NumberOfBeds = int.Parse(NumberOfBeds),
                Stars = int.Parse(Stars),
                Amenities = Amenities,
                HasBalcony = HasBalcony,
                IsNonSmoking = IsNonSmoking,
                IsAvailable = true
            };

            var success = await _roomService.UpdateRoomAsync(updatedRoom);
            if (success)
            {
                MessageBox.Show("Изменения сохранены успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateToAdminPanel();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении изменений", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Пожалуйста, введите название номера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ShortDescription))
            {
                MessageBox.Show("Пожалуйста, введите краткое описание", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(FullDescription))
            {
                MessageBox.Show("Пожалуйста, введите полное описание", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ImagePath) || !File.Exists(ImagePath))
            {
                MessageBox.Show("Пожалуйста, выберите корректное изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!double.TryParse(Rating, out double rating) || rating < 0 || rating > 5)
            {
                MessageBox.Show("Рейтинг должен быть числом от 0 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(Price, out decimal price) || price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(NumberOfBeds, out int beds) || beds <= 0)
            {
                MessageBox.Show("Количество кроватей должно быть положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(Stars, out int stars) || stars < 1 || stars > 5)
            {
                MessageBox.Show("Количество звёзд должно быть от 1 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Amenities))
            {
                MessageBox.Show("Пожалуйста, укажите удобства", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void Cancel(object parameter)
        {
            _navigationService.NavigateToAdminPanel();
        }
    }
}