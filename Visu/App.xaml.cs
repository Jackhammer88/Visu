using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GcodeProcessorService;
using Infrastructure;
using Infrastructure.Abstract.Interfaces;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Serilog;
using Visu.Views;
using Visualizer;

namespace Visu
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            CreateLogger();

            Current.DispatcherUnhandledException += DispatchUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += DomainUnhandledException;
            Dispatcher.UnhandledExceptionFilter += DispatcherUnhandledExceptionFilter;
        }

        private void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(path: "general.log")
                .CreateLogger();
        }

        private void DispatcherUnhandledExceptionFilter(object sender, System.Windows.Threading.DispatcherUnhandledExceptionFilterEventArgs e)
        {
            ShowMessageAndLeave(e);
            Container.Resolve<ILoggerFacade>().Log($"{e.Exception.ToString()}\n{e.Exception.StackTrace}", Category.Exception, Priority.High);
        }

        private void ShowMessageAndLeave(object e)
        {
            MessageBox.Show("Something went wrong. Sorry.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            Shutdown();
        }

        private void DomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Container.Resolve<ILoggerFacade>().Log($"{e.ExceptionObject.ToString()}", Category.Exception, Priority.High);
            ShowMessageAndLeave(e);
        }

        private void DispatchUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Container.Resolve<ILoggerFacade>().Log($"{e.Exception.Message}\n{e.Exception.StackTrace}", Category.Exception, Priority.High);
            ShowMessageAndLeave(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSerilog();
            containerRegistry.RegisterSingleton<Main>();
            containerRegistry.RegisterSingleton<IAppCommands, AppCommands>();
        }

        protected override Window CreateShell() => Container.Resolve<Main>();

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            LoadServices(moduleCatalog);
            LoadModules(moduleCatalog);
        }

        private void LoadServices(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<GcodeProcessorServiceModule>();
        }

        private void LoadModules(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<VisualizerModule>();
        }
    }
}
