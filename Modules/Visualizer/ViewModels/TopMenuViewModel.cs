using System.Windows;
using System.Windows.Input;
using Infrastructure.Abstract.Interfaces;
using Prism.Commands;

namespace Visualizer.ViewModels
{
    public class TopMenuViewModel
    {
        private readonly IAppCommands _appCommands;

        public TopMenuViewModel(IAppCommands appCommands)
        {
            _appCommands = appCommands;

            CreateCommands();
        }

        private void CreateCommands()
        {
            OpenCommand = new DelegateCommand(OpenCommandExecute);
            RefreshCommand = new DelegateCommand(RefreshCommandExecute);
            CloseCommand = new DelegateCommand(CloseCommandExecute);
            _appCommands.OpenApp.RegisterCommand(OpenCommand);
            _appCommands.RefreshApp.RegisterCommand(RefreshCommand);
            _appCommands.CloseApp.RegisterCommand(CloseCommand);
        }

        private void OpenCommandExecute()
        {
            
        }
        private void RefreshCommandExecute()
        {
            
        }
        private void CloseCommandExecute()
        {
            Application.Current.Shutdown();
        }


        public ICommand RefreshCommand { get; private set; }

        public ICommand OpenCommand { get; private set; }

        public DelegateCommand CloseCommand { get; private set; }

    }
}