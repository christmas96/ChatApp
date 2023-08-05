using ChatApp.Interfaces;
using ChatApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        [ObservableProperty]
        private string login;

        [ObservableProperty]
        private string password;

        public IAsyncRelayCommand LoginCommand { get; }

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        async Task LoginAsync()
        {
            User newUser = new()
            {
                Login = Login,
                Password = Password
            };

            await _userService.SaveUser(newUser);
            App.Current.MainPage = new MainShell();
        }
    }
}
