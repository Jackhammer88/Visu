using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Abstract.EventArgs;
using Infrastructure.Abstract.GCode;

namespace Infrastructure.Abstract.Interfaces
{
    public interface IMachineSimulator
    {
        Task OpenFileAsync(string path);
        Task RefreshFileAsync();

        IEnumerable<float> ModalGCodes { get; }
        IEnumerable<float> ModalMCodes { get; }
        AxisCoordinates OldCoordinates { get; }
        AxisCoordinates CurrentCoordinates { get; }
        
        event Action NewFileOpened;
        event Action<float> LoadingProgressChanged;
        event EventHandler<FrameChangedEventArgs> FrameChanged;
        event Action ProgramOpened;
    }

}