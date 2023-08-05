using AsyncAwaitBestPractices;
using ChatApp.ViewModels;

namespace ChatApp.Views;

public partial class MainPage : ContentPage
{
    MainViewModel ViewModel => BindingContext as MainViewModel;

    public MainPage()
	{
		InitializeComponent();
		BindingContext = App.Current.Services.GetService<MainViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ViewModel?.LoadData().SafeFireAndForget();
    }
}

