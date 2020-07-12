using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Infrastructure.Abstract.Interfaces
{
    public interface IMachineSimulator
    {
        Task OpenFileAsync(string path);
        event Action<float> LoadingProgressChanged;
    }

}