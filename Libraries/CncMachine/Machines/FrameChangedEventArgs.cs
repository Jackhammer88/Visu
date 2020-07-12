using System;

namespace CncMachine.Machines
{
    public class FrameChangedEventArgs : EventArgs
    {
        public FrameChangedEventArgs(int number)
        {
            Number = number;
        }
        public int Number { get; set; }
    }
}
