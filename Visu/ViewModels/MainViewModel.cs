using System.Windows.Input;
using Infrastructure.Abstract.Interfaces;
using Prism.Mvvm;

namespace Visu.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IAppCommands _appCommands;

        public MainViewModel(IAppCommands appCommands)
        {
            _appCommands = appCommands;
            Title = "Visu - CNC Visualizer";
        }

        public string Title { get; }
        public ICommand OpenAppCommand => _appCommands.OpenApp;
        public ICommand RefreshAppCommand => _appCommands.RefreshApp;
        public ICommand CloseAppCommand => _appCommands.CloseApp;
    }
}