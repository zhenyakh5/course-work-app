using BookingService;
using BookingService.MVVM.Services;
using BookingService.MVVM.Views;
using System.Linq;
using System.Windows;
using System.Windows.Input;

public class LoginViewModel : ViewModelBase
{
    private string _username;
    private string _password;
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

    public System.Windows.Input.ICommand LoginCommand { get; }
    public System.Windows.Input.ICommand RegisterCommand { get; }

    public LoginViewModel()
    {
        _authService = new AuthService(new AppDbContext());

        LoginCommand = new RelayCommand(Login, CanLogin);
        RegisterCommand = new RelayCommand(Register);
    }

    private void Login(object parameter)
    {
        var user = _authService.Authenticate(Username, Password);

        if (user != null)
        {
            SessionManager.SetCurrentUser(user);
            var mainWindow = new MainWindow();
            mainWindow.Show();

            if (user.IsAdmin)
                mainWindow.ShowAdminPanel();

            Application.Current.Windows.OfType<LoginWindow>().First().Close();
        }
        else
        {
            MessageBox.Show("Неверный логин или пароль", "Ошибка",
                          MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanLogin(object parameter) =>
        !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

    private void Register(object parameter)
    {
        var registerWindow = new RegisterWindow();
        registerWindow.Show();
        Application.Current.Windows.OfType<LoginWindow>().First().Close();
    }
}