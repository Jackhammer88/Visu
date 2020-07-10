using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
            
        }

        private void LoadModules(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<VisualizerModule>();
        }
    }
}
