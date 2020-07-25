using System.Windows;
using System.Windows.Input;
using Infrastructure.Abstract.Interfaces;
using Infrastructure.Constants;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace Visualizer.ViewModels
{
    public class TopMenuViewModel
    {
        private readonly IAppCommands _appCommands;
        private readonly IMachineSimulator _machineSimulator;
        private readonly IDialogService _dialogService;

        public TopMenuViewModel(IAppCommands appCommands, IMachineSimulator machineSimulator, IDialogService dialogService)
        {
            _appCommands = appCommands;
            _machineSimulator = machineSimulator;
            _dialogService = dialogService;

            CreateCommands();
        }

        private void CreateCommands()
        {
            OpenCommand = new DelegateCommand(OpenCommandExecute);
            RefreshCommand = new DelegateCommand(RefreshCommandExecute);
            InfoCommand = new DelegateCommand(InfoCommandExecute);
            CloseCommand = new DelegateCommand(CloseCommandExecute);
            _appCommands.OpenApp.RegisterCommand(OpenCommand);
            _appCommands.RefreshApp.RegisterCommand(RefreshCommand);
            _appCommands.CloseApp.RegisterCommand(CloseCommand);
        }


        private async void OpenCommandExecute()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "tap files (*.tap)|*.tap|nc files (*.nc)|*.nc|text files (*.txt)|*.txt|cnc files (*.cnc)|*.cnc|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
            };
            var result = dialog.ShowDialog();
            if (result ?? false)
                await _machineSimulator.OpenFileAsync(dialog.FileName);
        }
        private async void RefreshCommandExecute()
        {
            await _machineSimulator.RefreshFileAsync();
        }
        private void InfoCommandExecute()
        {
            _dialogService.ShowDialog(DialogNames.Info, new DialogParameters(), (s) => { }) ;
        }

        private void CloseCommandExecute()
        {
            Application.Current.Shutdown();
        }


        public DelegateCommand RefreshCommand { get; private set; }

        public DelegateCommand OpenCommand { get; private set; }

        public DelegateCommand InfoCommand { get; private set; }

        public DelegateCommand CloseCommand { get; private set; }

    }
}