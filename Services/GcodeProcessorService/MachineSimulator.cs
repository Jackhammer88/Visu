using Infrastructure.Abstract.Interfaces;
using System;
using System.Collections;
using System.Threading.Tasks;
using CncMachine.Machines;
using Infrastructure.Abstract.EventArgs;
using System.Collections.Generic;
using System.IO;
using Infrastructure.Abstract.GCode;

namespace GcodeProcessorService
{
    public class MachineSimulator : IMachineSimulator
    {
        private string _path;
        bool isOpening;

        public MachineSimulator()
        {
            Machine = new MillMachine();
        }


        public async Task OpenFileAsync(string path)
        {
            if(isOpening) return;
            lock (this) isOpening = true;
            _path = path;
            NewFileOpened();
            var progress = new Progress<float>(v => LoadingProgressChanged(v));
            await Machine.LoadProgramAsync(path, progress);
            await Task.Run(() => Machine.Rewind(Machine.Program.Count - 1));
            ProgramOpened();
            lock (this) isOpening = false;
        }

        public async Task RefreshFileAsync()
        {
            if (isOpening) return;
            lock (this) isOpening = true;
            if (!string.IsNullOrEmpty(_path))
            {
                NewFileOpened();
                var progress = new Progress<float>(v => LoadingProgressChanged(v));
                await Machine.LoadProgramAsync(_path, progress);
                await Task.Run(() => Machine.Rewind(Machine.Program.Count - 1));
                ProgramOpened();
            }
            lock (this) isOpening = false;
        }
        public MillMachine Machine { get; }
        public IEnumerable<float> ModalGCodes => Machine.ModalGCodes;
        public IEnumerable<float> ModalMCodes => Machine.ModalMCodes;
        public AxisCoordinates OldCoordinates => Machine.OldCoordinates;
        public AxisCoordinates CurrentCoordinates => Machine.CurrentCoordinates;

        public string ProgramName => string.IsNullOrEmpty(_path)? string.Empty : Path.GetFileNameWithoutExtension(_path);

        public event Action NewFileOpened = delegate { };
        public event Action<float> LoadingProgressChanged = delegate { };
        public event EventHandler<FrameChangedEventArgs> FrameChanged
        {
            add => Machine.FrameChanged += value;
            remove => Machine.FrameChanged -= value;
        }
        public event Action ProgramOpened = delegate { };
    }

}