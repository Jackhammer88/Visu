using Infrastructure.Abstract.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace GcodeProcessorService
{
    public class GcodeProcessorServiceModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMachineSimulator, MachineSimulator>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }
    }
}
