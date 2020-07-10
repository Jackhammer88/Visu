using Prism.Mvvm;

namespace Visualizer.ViewModels
{
    public class Plot3dViewModel : BindableBase
    {
        public Plot3dViewModel()
        {
            Message = "Hello World!!!";
        }

        public string Message { get; }
    }
}