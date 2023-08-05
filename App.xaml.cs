using ChatApp.Interfaces;
using ChatApp.Services;
using ChatApp.ViewModels;
using CommunityToolkit.Maui;

namespace ChatApp;

public partial class App : Application
{
    public new static App Current => (App)Application.Current;
    public IServiceProvider Services { get; }

    public App()
	{
        Services = ConfigureServices();
        InitializeComponent();

        var user = Services.GetService<IUserService>().GetUser();
		MainPage = user is null ? new LoginShell() : new MainShell();
	}

    static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IUserService, UserService>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<LoginViewModel>();

        return services.BuildServiceProvider();
    }


}
