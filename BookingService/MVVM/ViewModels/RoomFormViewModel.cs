using BookingService.MVVM.Models;
using BookingService.MVVM.Services;
using BookingService.Services;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BookingService.MVVM.ViewModels
{
    public abstract class RoomFormViewModel : ViewModelBase
    {
        protected readonly IRoomService _roomService;
        protected readonly INavigationService _navigationService;

        private string _name;
        private string _shortDescription;
        private string _fullDescription;
        private string _imagePath;
        private string _category = "Стандарт";
        private double _rating;
        private decimal _price;
        private int _numberOfBeds = 1;
        private int _stars = 3;
        private string _amenities;
        private bool _hasBalcony;
        private bool _isNonSmoking;
        private BitmapImage _imagePreview;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        public string ShortDescription { get => _shortDescription; set { _shortDescription = value; OnPropertyChanged(); } }
        public string FullDescription { get => _fullDescription; set { _fullDescription = value; OnPropertyChanged(); } }
        public string ImagePath { get => _imagePath; set { _imagePath = value; OnPropertyChanged(); UpdateImagePreview(); } }
        public string Category { get => _category; set { _category = value; OnPropertyChanged(); } }
        public double Rating { get => _rating; set { _rating = value; OnPropertyChanged(); } }
        public decimal Price { get => _price; set { _price = value; OnPropertyChanged(); } }
        public int NumberOfBeds { get => _numberOfBeds; set { _numberOfBeds = value; OnPropertyChanged(); } }
        public int Stars { get => _stars; set { _stars = value; OnPropertyChanged(); } }
        public string Amenities { get => _amenities; set { _amenities = value; OnPropertyChanged(); } }
        public bool HasBalcony { get => _hasBalcony; set { _hasBalcony = value; OnPropertyChanged(); } }
        public bool IsNonSmoking { get => _isNonSmoking; set { _isNonSmoking = value; OnPropertyChanged(); } }
        public BitmapImage ImagePreview { get => _imagePreview; set { _imagePreview = value; OnPropertyChanged(); } }

        public ICommand BrowseImageCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public RoomFormViewModel(IRoomService roomService, INavigationService navigationService)
        {
            _roomService = roomService;
            _navigationService = navigationService;

            BrowseImageCommand = new RelayCommand(BrowseImage);
            CancelCommand = new RelayCommand(Cancel);
        }

        protected virtual void BrowseImage(object parameter)
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

        protected virtual void Cancel(object parameter)
        {
            _navigationService.NavigateToAdminPanel();
        }

        protected void UpdateImagePreview()
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

        protected virtual bool ValidateInputs()
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

            if (Rating < 0 || Rating > 5)
            {
                MessageBox.Show("Рейтинг должен быть числом от 0 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (NumberOfBeds <= 0)
            {
                MessageBox.Show("Количество кроватей должно быть положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Stars < 1 || Stars > 5)
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

        protected HotelRoom CreateRoomFromData()
        {
            return new HotelRoom
            {
                Name = Name,
                ShortDescription = ShortDescription,
                FullDescription = FullDescription,
                ImagePath = ImagePath,
                Category = Category,
                Rating = Rating,
                Price = Price,
                NumberOfBeds = NumberOfBeds,
                Amenities = Amenities,
                Stars = Stars,
                HasBalcony = HasBalcony,
                IsNonSmoking = IsNonSmoking,
                IsAvailable = true
            };
        }
    }
}