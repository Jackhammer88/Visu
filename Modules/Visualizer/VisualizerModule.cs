using Infrastructure.Constants;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Visualizer.Views;

namespace Visualizer
{
    public class VisualizerModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Plot3d>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.CentralRegion, typeof(Plot3d));
        }
    }
}