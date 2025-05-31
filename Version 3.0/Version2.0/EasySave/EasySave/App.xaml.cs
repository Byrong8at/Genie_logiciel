using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using EasySave.Core;
using EasySave.MVVM.View;
using EasySave.MVVM.ViewModel;
using EasySave.Services;
using EasySave.MVVM.Model;

namespace EasySave
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SocketServer socketServer;

        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<LanguageViewModel>();
            services.AddSingleton<ExecuteViewModel>();
            services.AddSingleton<CreateViewModel>();
            services.AddSingleton<OverviewViewModel>();
            services.AddSingleton<DeleteViewModel>();
            services.AddSingleton<CheckViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }



        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
            socketServer = new SocketServer();
            socketServer.Start();

            // (facultatif)
            MessageBox.Show("SocketServer lancé !");
        }
        protected override void OnExit(ExitEventArgs e)
        {
            socketServer?.Stop();
            base.OnExit(e);
        }
    }

}
