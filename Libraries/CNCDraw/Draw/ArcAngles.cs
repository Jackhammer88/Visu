namespace CNCDraw.Draw
{
    public class ArcAngles
    {
        public ArcAngles(double startingAngle, double endAngle)
        {
            StartingAngle = startingAngle;
            EndAngle = endAngle;
        }
        public double StartingAngle { get; set; }
        public double EndAngle { get; set; }
    }
}
