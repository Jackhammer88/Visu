using Infrastructure.Abstract.Interfaces;
using System;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CncMachine.Machines;
using GcodeProcessorService.Annotations;

namespace GcodeProcessorService
{
    public class MachineSimulator : IMachineSimulator
    {
        public MachineSimulator()
        {
            Machine = new MillMachine();
        }
        public MillMachine Machine { get; }


        public async Task OpenFileAsync(string path)
        {
            var progress = new Progress<float>(v => LoadingProgressChanged(v));
            await Machine.LoadProgramAsync(path, progress);
        }

        public event Action<float> LoadingProgressChanged = delegate { };
    }


}