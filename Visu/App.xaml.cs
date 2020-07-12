using System.Windows;
using GcodeProcessorService;
using Infrastructure;
using Infrastructure.Abstract.Interfaces;
using Prism.Ioc;
using Prism.Modularity;
using Visu.Views;
using Visualizer;

namespace Visu
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
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
