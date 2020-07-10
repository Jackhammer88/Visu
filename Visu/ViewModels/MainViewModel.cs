using Prism.Mvvm;

namespace Visu.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            Title = "Visu - CNC Visualizer";
        }

        public string Title { get; }
    }
}