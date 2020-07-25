using Infrastructure.Constants;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Visualizer.Dialogs;
using Visualizer.ViewModels;
using Visualizer.Views;

namespace Visualizer
{
    public class VisualizerModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Plot3d>();
            containerRegistry.RegisterDialog<Info, InfoViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.CentralRegion, typeof(Plot3d));
            regionManager.RegisterViewWithRegion(RegionNames.TopRegion, typeof(TopMenu));
            regionManager.RegisterViewWithRegion(RegionNames.BottomRegion, typeof(StatusBar));
        }
    }
}