using System.Windows;
using System.Windows.Controls;

namespace BookingService.MVVM.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            DataContext = new RegisterViewModel();

            PasswordBox.PasswordChanged += (s, e) =>
            {
                if (DataContext is RegisterViewModel vm)
                    vm.Password = PasswordBox.Password;
            };
        }
    }
}