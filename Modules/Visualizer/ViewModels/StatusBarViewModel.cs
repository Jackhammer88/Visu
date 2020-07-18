using System;
using System.Threading.Tasks;
using System.Windows;
using Infrastructure.Abstract.Interfaces;
using Prism.Mvvm;

namespace Visualizer.ViewModels
{
    public class StatusBarViewModel : BindableBase
    {
        private readonly IMachineSimulator _machineSimulator;
        private Visibility _statusBarVisibility;
        private double _progress;

        public StatusBarViewModel(IMachineSimulator machineSimulator)
        {
            _machineSimulator = machineSimulator;
            _machineSimulator.LoadingProgressChanged += MachineSimulatorProgressChanged;

            StatusBarVisibility = Visibility.Collapsed;
        }

        private void MachineSimulatorProgressChanged(float progress)
        {
            Progress = progress;
            if (Math.Abs(Progress - 100) < 0.001)
            {
                StatusBarVisibility = Visibility.Collapsed;
                Progress = 0;
            }
            else
            {
                if (StatusBarVisibility == Visibility.Collapsed) StatusBarVisibility = Visibility.Visible;
            }
        }

        public Visibility StatusBarVisibility
        {
            get => _statusBarVisibility;
            private set => SetProperty(ref _statusBarVisibility, value);
        }

        public double Progress
        {
            get => _progress;
            private set => SetProperty(ref _progress, value);
        }
    }
}