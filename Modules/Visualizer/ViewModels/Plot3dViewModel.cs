using System;
using System.Linq;
using System.Reflection.PortableExecutable;
using CNCDraw.Draw;
using GcodeParser.GcodeInterpreter.Interpreter;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using Infrastructure.Abstract.EventArgs;
using Infrastructure.Abstract.GCode;
using Infrastructure.Abstract.Interfaces;
using Prism.Mvvm;
using SharpDX;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Point3D = System.Windows.Media.Media3D.Point3D;

namespace Visualizer.ViewModels
{
    public class Plot3dViewModel : BindableBase
    {
        private readonly IMachineSimulator _machineSimulator;
        private HelixToolkit.SharpDX.Core.Geometry3D _rapidGeometry;
        private HelixToolkit.SharpDX.Core.Geometry3D _linearGeometry;

        public Plot3dViewModel(IMachineSimulator machineSimulator)
        {
            _machineSimulator = machineSimulator;
            Message = "Hello World!!!";

            _machineSimulator.FrameChanged += MachineFrameChanged;

            _machineSimulator.NewFileOpened += ClearPlotModel;
            _machineSimulator.ProgramOpened += OnProgramOpened;
        }

        private void OnProgramOpened()
        {
            UpdateGeometry(LinearGeometry);
            UpdateGeometry(RapidGeometry);
        }

        private void ClearPlotModel()
        {
            RapidGeometry = new LineBuilder().ToLineGeometry3D();
            LinearGeometry = new LineBuilder().ToLineGeometry3D();
        }

        private void MachineFrameChanged(object sender, FrameChangedEventArgs e)
        {
            if (!e.Frame.Coordinate.NotEmpty()) return;
            var gModalGroup1 = _machineSimulator.ModalGCodes.Single(c => GCodeExpression.MutuallyExclusiveCodes[0].Contains(c));

            if (_machineSimulator.OldCoordinates.X != null && _machineSimulator.OldCoordinates.Y != null && _machineSimulator.OldCoordinates.Z != null)
            {
                var oldCoordinates = new Point3D(_machineSimulator.OldCoordinates.X.Value, _machineSimulator.OldCoordinates.Y.Value, _machineSimulator.OldCoordinates.Z.Value);
                var centers = new ArcCenters { I = e.Frame.IValue, J = e.Frame.JValue, K = e.Frame.KValue };
                bool absolute = !_machineSimulator.ModalGCodes.Contains(91);
                var newCoordinates = new Point3D(CalculateNewCoordinate(_machineSimulator.CurrentCoordinates.X, oldCoordinates.X, absolute),
                    CalculateNewCoordinate(_machineSimulator.CurrentCoordinates.Y, oldCoordinates.Y, absolute),
                    CalculateNewCoordinate(_machineSimulator.CurrentCoordinates.Z, oldCoordinates.Z, absolute));
                var radius = e.Frame.RValue;
                ScaleCoordinates(10, ref oldCoordinates, ref newCoordinates, ref centers, ref radius);

                Draw(gModalGroup1, oldCoordinates, newCoordinates, radius, centers, e);
            }
        }

        private void ScaleCoordinates(int scale, ref Point3D oldCoordinates, ref Point3D newCoordinates, ref ArcCenters centers, ref float? radius)
        {
            oldCoordinates.X /= scale;
            oldCoordinates.Y /= scale;
            oldCoordinates.Z /= scale;
            newCoordinates.X /= scale;
            newCoordinates.Y /= scale;
            newCoordinates.Z /= scale;
            centers.I /= scale;
            centers.J /= scale;
            centers.K /= scale;
            radius /= scale;
        }

        private double CalculateNewCoordinate(float? newCoordinate, double? oldCoordinate, bool absolute)
        {
            return absolute ? (double)(newCoordinate ?? oldCoordinate) : (double)(newCoordinate.HasValue ? newCoordinate + oldCoordinate : oldCoordinate);
        }

        private void Draw(float gModalGroup1, Point3D oldCoordinates, Point3D newCoordinates, float? radius, ArcCenters centers, FrameChangedEventArgs e)
        {
            ArcInterpolation interpolation;

            switch (gModalGroup1)
            {
                case 0:
                    //Rapid
                    AddLine(oldCoordinates.ToVector3(), newCoordinates.ToVector3(), RapidGeometry);
                    break;
                case 1:
                    //Linear
                    AddLine(oldCoordinates.ToVector3(), newCoordinates.ToVector3(), LinearGeometry);
                    break;
                case 2:
                    //Arc clockwise
                    interpolation = e.Frame.RValue.HasValue ? new ArcInterpolation(oldCoordinates, radius: radius.Value, newCoordinates, true) : new ArcInterpolation(oldCoordinates, centers, newCoordinates, true);

                    for (int i = 0; i <= 100; i += 5)
                    {
                        var point = interpolation.GetArcCoordinatesEx(i);
                        AddLine(oldCoordinates.ToVector3(), point.ToVector3(), LinearGeometry);
                        oldCoordinates = point;
                    }
                    AddLine(oldCoordinates.ToVector3(), newCoordinates.ToVector3(), LinearGeometry);
                    break;
                case 3:
                    //Arc Counter-clockwise
                    if (e.Frame.RValue.HasValue)
                        interpolation = new ArcInterpolation(oldCoordinates, radius: radius.Value, newCoordinates, false);
                    else
                        interpolation = new ArcInterpolation(oldCoordinates, centers, newCoordinates, false);
                    for (int i = 100; i >= 0; i -= 5)
                    {
                        var point = interpolation.GetArcCoordinatesEx(i);
                        AddLine(oldCoordinates.ToVector3(), point.ToVector3(), LinearGeometry);
                        oldCoordinates = point;
                    }
                    AddLine(oldCoordinates.ToVector3(), newCoordinates.ToVector3(), LinearGeometry);
                    break;
            }
        }

        private void AddLine(Vector3 old, Vector3 next, HelixToolkit.SharpDX.Core.Geometry3D geometry)
        {
            int i0 = geometry.Positions.Count;
            geometry.Positions.Add(old);
            geometry.Positions.Add(next);
            geometry.Indices.Add(i0);
            geometry.Indices.Add(i0 + 1);
        }
        private static void UpdateGeometry(HelixToolkit.SharpDX.Core.Geometry3D geometry)
        {
            geometry.UpdateVertices();
            geometry.UpdateTriangles();
            geometry.UpdateBounds();
        }

        public HelixToolkit.SharpDX.Core.Geometry3D RapidGeometry
        {
            get => _rapidGeometry;
            private set => SetProperty(ref _rapidGeometry, value);
        }
        public HelixToolkit.SharpDX.Core.Geometry3D LinearGeometry
        {
            get => _linearGeometry;
            private set => SetProperty(ref _linearGeometry, value);
        }
        public string Message { get; }
    }
}