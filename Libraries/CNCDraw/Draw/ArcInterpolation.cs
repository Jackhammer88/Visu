using CNCDraw.Resources;
using MyExtensions;
using System;
using System.Windows.Media.Media3D;

namespace CNCDraw.Draw
{
    public class ArcInterpolation
    {
        private readonly Point3D _origin;
        private readonly ArcCenters _centers;
        private readonly Point3D _end;
        private readonly Point3D _offset;
        private readonly ArcAngles _angles;
        private readonly double _radius;
        private readonly bool _clockwise;
        private readonly Plane _plane;

        public ArcInterpolation(Point3D origin, float radius, Point3D end, bool clockwise)
        {
            _origin = origin;
            _end = end;
            _clockwise = clockwise;
            _radius = radius;
            _plane = GetPlane(origin, end);
            _centers = CalculateCenters(origin, end, radius, _plane);
            _offset = RoundOffsets(CalculateOffsets(origin, _centers));
            _angles = RoundAngles(CalculateAngles(origin, end, _centers, _plane));

            if (!_clockwise && _angles.StartingAngle >= _angles.EndAngle)
                _angles.EndAngle += 360;
        }
        public ArcInterpolation(Point3D origin, ArcCenters centers, Point3D end, bool clockwise)
        {
            _origin = origin;
            _centers = centers ?? throw new ArgumentNullException(paramName: nameof(centers));
            _end = end;
            _clockwise = clockwise;

            _plane = GetPlane();
            _radius = CalculateRadius(_plane);
            _offset = CalculateOffsets(origin, centers);
            _angles = CalculateAngles(origin, end, centers, _plane);

            if (!_clockwise && _angles.StartingAngle >= _angles.EndAngle)
                _angles.EndAngle += 360;
        }

        public Point3D GetArcCoordinatesEx(float percentage)
        {
            Point3D result = default;
            percentage /= 100;
            switch (_plane)
            {
                case Plane.XY:
                    if (_clockwise)
                    {
                        var alpha = _angles.StartingAngle;
                        var beta = _angles.EndAngle;
                        NormalizeAngleClockwise(ref alpha, ref beta);
                        var delta = alpha - beta;
                        var convertedAngle = (delta <= 0) ? delta + 360 : delta;
                        result.X = _radius * Math.Cos(MathEx.DegreeToRadian(alpha - convertedAngle * percentage)) + _offset.X;
                        result.Y = _radius * Math.Sin(MathEx.DegreeToRadian(alpha - convertedAngle * percentage)) + _offset.Y;
                        result.Z = _origin.Z;
                    }
                    else
                    {
                        var alpha = _angles.StartingAngle;
                        var beta = _angles.EndAngle;

                        var delta = beta - alpha;
                        result.X = _radius * Math.Cos(MathEx.DegreeToRadian(beta - delta * percentage)) + _offset.X;
                        result.Y = _radius * Math.Sin(MathEx.DegreeToRadian(beta - delta * percentage)) + _offset.Y;
                        result.Z = _origin.Z;
                    }
                    break;
                case Plane.XZ:
                    if (_clockwise)
                    {
                        var alpha = _angles.StartingAngle;
                        var beta = _angles.EndAngle;
                        NormalizeAngleClockwise(ref alpha, ref beta);
                        var delta = alpha - beta;
                        var convertedAngle = (delta <= 0) ? delta + 360 : delta;
                        result.X = _radius * Math.Cos(MathEx.DegreeToRadian(alpha - convertedAngle * percentage)) + _offset.X;
                        result.Z = _radius * Math.Sin(MathEx.DegreeToRadian(alpha - convertedAngle * percentage)) + _offset.Z;
                        result.Y = _origin.Y;
                    }
                    else
                    {
                        var alpha = _angles.StartingAngle;
                        var beta = _angles.EndAngle;

                        var delta = beta - alpha;
                        result.X = _radius * Math.Cos(MathEx.DegreeToRadian(beta - delta * percentage)) + _offset.X;
                        result.Z = _radius * Math.Sin(MathEx.DegreeToRadian(beta - delta * percentage)) + _offset.Z;
                        result.Y = _origin.Y;
                    }
                    break;
                case Plane.YZ:
                    if (_clockwise)
                    {
                        var alpha = _angles.StartingAngle < 0 ? _angles.StartingAngle + 360 : _angles.StartingAngle;
                        var beta = _angles.EndAngle;
                        var delta = alpha - beta;
                        var convertedAngle = (delta <= 0) ? delta + 360 : delta;
                        result.Y = _radius * Math.Cos(MathEx.DegreeToRadian(alpha - convertedAngle * percentage)) + _offset.Y;
                        result.Z = _radius * Math.Sin(MathEx.DegreeToRadian(alpha - convertedAngle * percentage)) + _offset.Z;
                        result.X = _origin.X;
                    }
                    else
                    {
                        var alpha = _angles.StartingAngle;
                        var betta = _angles.EndAngle;

                        var delta = betta - alpha;
                        result.Y = _radius * Math.Cos(MathEx.DegreeToRadian(betta - delta * percentage)) + _offset.Y;
                        result.Z = _radius * Math.Sin(MathEx.DegreeToRadian(betta - delta * percentage)) + _offset.Z;
                        result.X = _origin.X;
                    }
                    break;
                case Plane.Unknown:
                default:
                    break;
            }
            return result;
        }
        private ArcAngles RoundAngles(ArcAngles arcAngles)
        {
            arcAngles.StartingAngle = Math.Round(arcAngles.StartingAngle, 3);
            arcAngles.EndAngle = Math.Round(arcAngles.EndAngle, 3);
            return arcAngles;
        }
        private Point3D RoundOffsets(Point3D point3D)
        {
            point3D.X = Math.Round(point3D.X, 3);
            point3D.Y = Math.Round(point3D.Y, 3);
            point3D.Z = Math.Round(point3D.Z, 3);
            return point3D;
        }
        private ArcCenters CalculateCenters(Point3D origin, Point3D end, float radius, Plane plane)
        {
            ArcCenters result;
            double a1, a2, b1, b2;
            switch (plane)
            {
                case Plane.XY:
                    a1 = origin.X;
                    a2 = end.X;
                    b1 = origin.Y;
                    b2 = end.Y;
                    var ij = CalculateCentersInternal(ref a1, ref a2, ref b1, ref b2, radius);
                    result = new ArcCenters { I = ij.Item1, J = ij.Item2 };
                    break;
                case Plane.XZ:
                    a1 = origin.X;
                    a2 = end.X;
                    b1 = origin.Z;
                    b2 = end.Z;
                    var ik = CalculateCentersInternal(ref a1, ref a2, ref b1, ref b2, radius);
                    result = new ArcCenters { I = ik.Item1, K = ik.Item2 };
                    break;
                case Plane.YZ:
                    a1 = origin.Y;
                    a2 = end.Y;
                    b1 = origin.Z;
                    b2 = end.Z;
                    var jk = CalculateCentersInternal(ref a1, ref a2, ref b1, ref b2, radius);
                    result = new ArcCenters { J = jk.Item1, K = jk.Item2 };
                    break;
                default:
                case Plane.Unknown:
                    throw new InvalidOperationException(ResourcesStrings.UnknownPlane);
            }
            return result;
        }

        private Tuple<double, double> CalculateCentersInternal(ref double a1, ref double a2, ref double b1, ref double b2, float radius)
        {
            double radsq = radius * radius;
            double q = Math.Sqrt(((a2 - a1) * (a2 - a1)) + ((b2 - b1) * (b2 - b1)));

            double x3 = (a1 + a2) / 2;
            double xc;
            if (_clockwise)
                xc = x3 - Math.Sqrt(radsq - ((q / 2) * (q / 2))) * ((b1 - b2) / q);
            else
                xc = x3 + Math.Sqrt(radsq - ((q / 2) * (q / 2))) * ((b1 - b2) / q);

            double y3 = (b1 + b2) / 2;
            double yc;
            if (_clockwise)
                yc = y3 - Math.Sqrt(radsq - ((q / 2) * (q / 2))) * ((a2 - a1) / q);
            else
                yc = y3 + Math.Sqrt(radsq - ((q / 2) * (q / 2))) * ((a2 - a1) / q);


            return new Tuple<double, double>(xc - a1, yc - b1);
        }
        private static void NormalizeAngleClockwise(ref double alpha, ref double beta)
        {
            if (alpha < 0 && beta < 0)
            {
                alpha += 360;
                beta += 360;
            }

        }
        private Plane GetPlane()
        {
            Plane result;
            if (_centers.I.HasValue && _centers.J.HasValue)
                result = Plane.XY;
            else if (_centers.I.HasValue && _centers.K.HasValue)
                result = Plane.XZ;
            else if (_centers.J.HasValue && _centers.K.HasValue)
                result = Plane.YZ;
            else
                throw new InvalidOperationException(ResourcesStrings.UnknownPlane);

            return result;
        }
        private Plane GetPlane(Point3D origin, Point3D end)
        {
            Plane plane;
            if (origin.X == end.X && (origin.Y != end.Y) && (origin.Z != end.Z))
                plane = Plane.YZ;
            else if (origin.Y == end.Y && (origin.X != end.X) && (origin.Z != end.Z))
                plane = Plane.XZ;
            else if (origin.Z == end.Z && (origin.X != end.X) && (origin.Y != end.Y))
                plane = Plane.XY;
            else
                plane = Plane.Unknown;
            return plane;
        }
        private double CalculateRadius(Plane plane)
        {
            double a, b;
            switch (plane)
            {
                case Plane.XY:
                    a = _centers.I ?? 0;
                    b = _centers.J ?? 0;
                    break;
                case Plane.XZ:
                    a = _centers.I ?? 0;
                    b = _centers.K ?? 0;
                    break;
                case Plane.YZ:
                    a = _centers.J ?? 0;
                    b = _centers.K ?? 0;
                    break;
                case Plane.Unknown:
                default:
                    throw new InvalidOperationException(ResourcesStrings.UnknownPlane);
            }

            return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }
        private Point3D CalculateOffsets(Point3D start, ArcCenters centers)
        {
            Point3D result = default;
            if (centers.I.HasValue) result.X = centers.I.Value + start.X;
            if (centers.J.HasValue) result.Y = centers.J.Value + start.Y;
            if (centers.K.HasValue) result.Z = centers.K.Value + start.Z;
            return result;
        }
        private ArcAngles CalculateAngles(Point3D start, Point3D end, ArcCenters centers, Plane plane)
        {
            ArcAngles result;
            double catetA, catetB, angleStart, angleEnd;

            switch (plane)
            {
                case Plane.XY:
                    catetA = start.X - (centers.I.Value + start.X);
                    catetB = start.Y - (centers.J.Value + start.Y);
                    angleStart = Math.Atan2(catetB, catetA) * (180 / Math.PI);

                    catetA = end.X - (centers.I.Value + start.X);
                    catetB = end.Y - (centers.J.Value + start.Y);
                    angleEnd = Math.Atan2(catetB, catetA) * (180 / Math.PI);
                    break;
                case Plane.XZ:
                    catetA = start.X - (centers.I.Value + start.X);
                    catetB = start.Z - (centers.K.Value + start.Z);
                    angleStart = Math.Atan2(catetB, catetA) * (180 / Math.PI);

                    catetA = end.X - (centers.I.Value + start.X);
                    catetB = end.Z - (centers.K.Value + start.Z);
                    angleEnd = Math.Atan2(catetB, catetA) * (180 / Math.PI);
                    break;
                case Plane.YZ:
                    catetA = start.Y - (centers.J.Value + start.Y);
                    catetB = start.Z - (centers.K.Value + start.Z);
                    angleStart = Math.Atan2(catetB, catetA) * (180 / Math.PI);

                    catetA = end.Y - (centers.J.Value + start.Y);
                    catetB = end.Z - (centers.K.Value + start.Z);
                    angleEnd = Math.Atan2(catetB, catetA) * (180 / Math.PI);
                    break;
                case Plane.Unknown:
                default:
                    throw new InvalidOperationException(ResourcesStrings.UnknownPlane);
            }


            result = new ArcAngles(angleStart, angleEnd);
            return result;
        }
    }
}
