using System.Windows;
using System.Windows.Controls;

namespace BookingService.MVVM.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();

            PasswordBox.PasswordChanged += (s, e) =>
            {
                if (DataContext is LoginViewModel vm)
                    vm.Password = PasswordBox.Password;
            };
        }
    }
}
