using BookingService;
using BookingService.MVVM.Services;
using BookingService.MVVM.Views;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

public class RegisterViewModel : ViewModelBase
{
    private string _username;
    private string _password;
    private string _email;
    private readonly AuthService _authService;

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    public System.Windows.Input.ICommand RegisterCommand { get; }
    public System.Windows.Input.ICommand CancelCommand { get; }

    public RegisterViewModel()
    {
        _authService = new AuthService(new AppDbContext());

        RegisterCommand = new RelayCommand(Register, CanRegister);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Register(object parameter)
    {
        if (!IsValidPassword(Password))
        {
            MessageBox.Show("Пароль должен содержать 6+ символов и заглавную букву",
                           "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!IsValidEmail(Email))
        {
            MessageBox.Show("Введите корректный email", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (_authService.Register(Username, Password, Email))
        {
            MessageBox.Show("Регистрация успешна!");
            new LoginWindow().Show();
            Application.Current.Windows.OfType<RegisterWindow>().First().Close();
        }
        else
        {
            MessageBox.Show("Логин или email уже занят", "Ошибка",
                          MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanRegister(object parameter) =>
        !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email);

    private void Cancel(object parameter)
    {
        new LoginWindow().Show();
        Application.Current.Windows.OfType<RegisterWindow>().First().Close();
    }

    private bool IsValidPassword(string password)
    {
        return Regex.IsMatch(password, @"^(?=.*[A-Z]).{6,}$");
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}