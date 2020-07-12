using System;

namespace MyExtensions
{
    public static class MathEx
    {
        public static double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180);
        }
        public static double RadianToDegree(double radian)
        {
            return radian * (180 / Math.PI);
        }
    }
}
