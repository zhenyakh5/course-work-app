using BookingService.MVVM.Models;
using BookingService.MVVM.Views;
using BookingService.Services;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BookingService.ViewModels
{
    public class AddRoomViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;
        private readonly INavigationService _navigationService;

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
        private string _validationError;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); ClearValidationError(); } }
        public string ShortDescription { get => _shortDescription; set { _shortDescription = value; OnPropertyChanged(); ClearValidationError(); } }
        public string FullDescription { get => _fullDescription; set { _fullDescription = value; OnPropertyChanged(); ClearValidationError(); } }
        public string ImagePath { get => _imagePath; set { _imagePath = value; OnPropertyChanged(); UpdateImagePreview(); ClearValidationError(); } }
        public string Category { get => _category; set { _category = value; OnPropertyChanged(); ClearValidationError(); } }
        public string Rating { get => _rating; set { _rating = value; OnPropertyChanged(); ClearValidationError(); } }
        public string Price { get => _price; set { _price = value; OnPropertyChanged(); ClearValidationError(); } }
        public string NumberOfBeds { get => _numberOfBeds; set { _numberOfBeds = value; OnPropertyChanged(); ClearValidationError(); } }
        public string Stars { get => _stars; set { _stars = value; OnPropertyChanged(); ClearValidationError(); } }
        public string Amenities { get => _amenities; set { _amenities = value; OnPropertyChanged(); ClearValidationError(); } }
        public bool HasBalcony { get => _hasBalcony; set { _hasBalcony = value; OnPropertyChanged(); } }
        public bool IsNonSmoking { get => _isNonSmoking; set { _isNonSmoking = value; OnPropertyChanged(); } }
        public BitmapImage ImagePreview { get => _imagePreview; set { _imagePreview = value; OnPropertyChanged(); } }
        public string ValidationError { get => _validationError; set { _validationError = value; OnPropertyChanged(); } }

        public string[] Categories { get; } = { "Стандарт", "Люкс", "Делюкс", "Президентский" };

        public ICommand BrowseImageCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddRoomViewModel(IRoomService roomService, INavigationService navigationService)
        {
            _roomService = roomService;
            _navigationService = navigationService;

            BrowseImageCommand = new RelayCommand(BrowseImage);
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
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

        private bool CanSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Name) &&
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
            if (!ValidateInputs()) return;

            var newRoom = new HotelRoom
            {
                Name = Name,
                ShortDescription = ShortDescription,
                FullDescription = FullDescription,
                ImagePath = ImagePath,
                Category = Category,
                Rating = double.Parse(Rating),
                Price = decimal.Parse(Price),
                NumberOfBeds = int.Parse(NumberOfBeds),
                Amenities = Amenities,
                Stars = int.Parse(Stars),
                HasBalcony = HasBalcony,
                IsNonSmoking = IsNonSmoking,
                IsAvailable = true
            };

            var success = await _roomService.AddRoomAsync(newRoom);
            if (success)
            {
                MessageBox.Show("Номер успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateToAdminPanel();
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении номера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void ClearValidationError()
        {
            ValidationError = string.Empty;
        }

        private void Cancel(object parameter)
        {
            _navigationService.NavigateToAdminPanel();
            
        }
    }
}