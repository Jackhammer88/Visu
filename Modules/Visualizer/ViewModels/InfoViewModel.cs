using System;
using System.Diagnostics;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Visualizer.Resources;

namespace Visualizer.ViewModels
{
    public class InfoViewModel : BindableBase, IDialogAware
    {
        public InfoViewModel()
        {
            Title = VizualizerStrings.InfoDialogTitle;
            NavigateToWebCommand = new DelegateCommand<string>(ExecuteMethod);
            CloseDialogCommand = new DelegateCommand(()=> RequestClose?.Invoke(new DialogResult()));
        }


        private void ExecuteMethod(object parameter)
        {
            if (parameter is string uri)
            {
                var psi = new ProcessStartInfo
                {
                    FileName = uri,
                    UseShellExecute = true
                };
                try
                {
                    Process.Start(psi);
                }
                catch(Exception) { }
            }
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public string Title { get; }
        public DelegateCommand<string> NavigateToWebCommand { get; }
        public DelegateCommand CloseDialogCommand { get; }
        public event Action<IDialogResult> RequestClose;
    }
}