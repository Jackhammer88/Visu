using System.Windows;
using Infrastructure.Abstract.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace Visualizer.ViewModels
{
    public class StatusBarViewModel : BindableBase
    {
        private Visibility _statusBarVisibility;

        public StatusBarViewModel()
        {
        }

        public Visibility StatusBarVisibility
        {
            get => _statusBarVisibility;
            private set => SetProperty(ref _statusBarVisibility, value);
        }
    }
}