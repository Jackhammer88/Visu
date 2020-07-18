using System.Windows;
using System.Windows.Input;
using Infrastructure.Abstract.Interfaces;
using Microsoft.Win32;
using Prism.Commands;

namespace Visualizer.ViewModels
{
    public class TopMenuViewModel
    {
        private readonly IAppCommands _appCommands;
        private readonly IMachineSimulator _machineSimulator;

        public TopMenuViewModel(IAppCommands appCommands, IMachineSimulator machineSimulator)
        {
            _appCommands = appCommands;
            _machineSimulator = machineSimulator;

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

        private async void OpenCommandExecute()
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            var result = dialog.ShowDialog();
            if (result ?? false)
                await _machineSimulator.OpenFileAsync(dialog.FileName);
        }
        private async void RefreshCommandExecute()
        {
            await _machineSimulator.RefreshFileAsync();
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