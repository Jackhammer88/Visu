using Infrastructure.Abstract.GCode;

namespace Infrastructure.Abstract.EventArgs
{
    public class FrameChangedEventArgs : System.EventArgs
    {
        public FrameChangedEventArgs(int number, GFrame frame)
        {
            Frame = frame;
            Number = number;
        }
        public int Number { get; }
        public GFrame Frame { get; }
    }
}
