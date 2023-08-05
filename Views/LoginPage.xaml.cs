using AsyncAwaitBestPractices;
using ChatApp.ViewModels;
using Microsoft.Maui.Controls;

namespace ChatApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = App.Current.Services.GetService<LoginViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PlayAnimation().SafeFireAndForget();
        }

        async Task PlayAnimation()
        {
            await Task.Delay(1000);

            WelcomeLabel.FadeTo(1, 300).SafeFireAndForget();
            await WelcomeLabel.TranslateTo(0, WelcomeLabel.TranslationY - 600, 1000, Easing.Linear);
            await Task.Delay(500);
            await WelcomeLabel.FadeTo(0, 200);

            LoginStack.IsVisible = true;
        }
    }
}
