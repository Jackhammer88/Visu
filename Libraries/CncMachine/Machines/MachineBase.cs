using GcodeParser;
using GcodeParser.GcodeInterpreter;
using GcodeParser.GcodeInterpreter.Interpreter;
using Infrastructure.Abstract.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Abstract.GCode;

namespace CncMachine.Machines
{
    public class MachineBase
    {
        public MachineBase()
        {
            Program = new List<GFrame>();
            Initialize();
            ModalGCodes = new HashSet<float>();
            CurrentGCodes = new HashSet<float>();
            ModalMCodes = new HashSet<float>();
            CurrentMCodes = new HashSet<float>();
        }

        protected void Initialize()
        {
            FrameNumber = -1;
            ToolNumber = 0;
            FeedRate = 0;
            OldCoordinates = new AxisCoordinates() { X = 0, Y = 0, Z = 0 };
            CurrentCoordinates = OldCoordinates;
        }
        protected virtual void InitFrame()
        {
            if (Program.Count <= FrameNumber)
            {
                ProgramEnded(this, EventArgs.Empty);
                return;
            }
            SetOldCoordinates(CurrentCoordinates);
            CurrentFrame = Program[FrameNumber];
            CurrentCoordinates = CurrentFrame.Coordinate;
            UpdateModalGCodes(CurrentFrame.GCodes.ToHashSet());
            UpdateNonModalGCodes(CurrentFrame.GCodes.ToHashSet());
            UpdateModalMCodes(CurrentFrame.MCodes.ToHashSet());
            UpdateNonModalMCodes(CurrentFrame.MCodes.ToHashSet());
            if (CurrentFrame.Feedrate.HasValue) FeedRate = CurrentFrame.Feedrate.Value;
            CurrentMCodes.Clear();
            CurrentMCodes.UnionWith(CurrentFrame.MCodes);
            ToolNumber = CurrentFrame.ToolNumber ?? ToolNumber;
            FrameChanged(this, new FrameChangedEventArgs(FrameNumber, CurrentFrame));
        }

        private void SetOldCoordinates(AxisCoordinates currentCoordinates)
        {
            if (ModalGCodes.Contains(91))
            {
                if (currentCoordinates.A.HasValue)
                    OldCoordinates.A = currentCoordinates.A + OldCoordinates.A;
                if (currentCoordinates.B.HasValue)
                    OldCoordinates.B = currentCoordinates.B + OldCoordinates.B;
                if (currentCoordinates.C.HasValue)
                    OldCoordinates.C = currentCoordinates.C + OldCoordinates.C;
                if (currentCoordinates.U.HasValue)
                    OldCoordinates.U = currentCoordinates.U + OldCoordinates.U;
                if (currentCoordinates.V.HasValue)
                    OldCoordinates.V = currentCoordinates.V + OldCoordinates.V;
                if (currentCoordinates.W.HasValue)
                    OldCoordinates.W = currentCoordinates.W + OldCoordinates.W;
                if (currentCoordinates.X.HasValue)
                    OldCoordinates.X = currentCoordinates.X + OldCoordinates.X;
                if (currentCoordinates.Y.HasValue)
                    OldCoordinates.Y = currentCoordinates.Y + OldCoordinates.Y;
                if (currentCoordinates.Z.HasValue)
                    OldCoordinates.Z = currentCoordinates.Z + OldCoordinates.Z;
            }
            else
            {
                if (currentCoordinates.A.HasValue)
                    OldCoordinates.A = currentCoordinates.A;
                if (currentCoordinates.B.HasValue)
                    OldCoordinates.B = currentCoordinates.B;
                if (currentCoordinates.C.HasValue)
                    OldCoordinates.C = currentCoordinates.C;
                if (currentCoordinates.U.HasValue)
                    OldCoordinates.U = currentCoordinates.U;
                if (currentCoordinates.V.HasValue)
                    OldCoordinates.V = currentCoordinates.V;
                if (currentCoordinates.W.HasValue)
                    OldCoordinates.W = currentCoordinates.W;
                if (currentCoordinates.X.HasValue)
                    OldCoordinates.X = currentCoordinates.X;
                if (currentCoordinates.Y.HasValue)
                    OldCoordinates.Y = currentCoordinates.Y;
                if (currentCoordinates.Z.HasValue)
                    OldCoordinates.Z = currentCoordinates.Z;
            }
        }

        private void UpdateModalMCodes(HashSet<float> mCodes)
        {
            if (mCodes.Count == 0) return;
            foreach (var group in MCodeExpression.MutuallyExclusiveCodes)
            {
                if (mCodes.Intersect(group).Any())
                {
                    var modalCodes = mCodes.Where(c => group.Contains(c)).ToHashSet();
                    ModalMCodes.ExceptWith(group);
                    ModalMCodes.UnionWith(modalCodes);
                }
            }
        }
        private void UpdateNonModalMCodes(HashSet<float> mCodes)
        {
            if (mCodes.Count == 0) return;
            var allModalCodes = MCodeExpression.MutuallyExclusiveCodes.Aggregate((s1, s2) => s1.Concat(s2).ToArray());
            mCodes.ExceptWith(allModalCodes);
            if (mCodes.Count > 0)
                CurrentGCodes.UnionWith(mCodes);
        }
        private void UpdateModalGCodes(HashSet<float> gCodes)
        {
            if (gCodes.Count == 0) return;
            foreach (var group in GCodeExpression.MutuallyExclusiveCodes)
            {
                if (gCodes.Intersect(group).Any())
                {
                    var modalCodes = gCodes.Where(c => group.Contains(c)).ToHashSet();
                    ModalGCodes.ExceptWith(group);
                    ModalGCodes.UnionWith(modalCodes);
                }
            }
        }
        private void UpdateNonModalGCodes(HashSet<float> gCodes)
        {
            if (gCodes.Count == 0) return;
            var allModalCodes = GCodeExpression.MutuallyExclusiveCodes.Aggregate((s1, s2) => s1.Concat(s2).ToArray());
            gCodes.ExceptWith(allModalCodes);
            if (gCodes.Count > 0)
                CurrentGCodes.UnionWith(gCodes);
        }
        public virtual void LoadProgram(string path)
        {
            Program.Clear();
            GCodePreparer preparer = new GCodePreparer();
            preparer.OpenFile(path);
            preparer.PrepareStrings();

            InterpretProgram(preparer, new Progress<float>()).Wait();

            if (Program.Count == 0) return;
            FrameNumber = 0;
            CurrentFrame = Program[FrameNumber];
            InitFrame();
        }
        public virtual async Task LoadProgramAsync(string path, IProgress<float> progressChanger)
        {
            progressChanger.Report(0);
            Program.Clear();
            progressChanger.Report(1);
            GCodePreparer preparer = new GCodePreparer();
            await preparer.OpenFileAsync(path);
            progressChanger.Report(5);
            await preparer.PrepareStringsAsync();
            progressChanger.Report(10);

            await InterpretProgram(preparer, progressChanger);
            if (Program.Count == 0) return;
            FrameNumber = 0;
            CurrentFrame = Program[FrameNumber];
            InitFrame();
            progressChanger.Report(100);
        }
        private async Task InterpretProgram(GCodePreparer preparer, IProgress<float> progress)
        {
            CoordintatesExpression coordintatesExpression = new CoordintatesExpression();
            GCodeExpression gCodeExpression = new GCodeExpression();
            MCodeExpression mCodeExpression = new MCodeExpression();
            TCodeExpression tCodeExpression = new TCodeExpression();
            FeedrateExpression feedrateExpression = new FeedrateExpression();

            await Task.Run(() =>
            {
                float progressValue = 0;
                for (int lineNumber = 0; lineNumber < preparer.Strings.Count; lineNumber++)
                {
                    Context context = new Context
                    {
                        PreviousFrame = Program.LastOrDefault(),
                        InputString = preparer.Strings[lineNumber]
                    };
                    gCodeExpression.Interpret(context);
                    mCodeExpression.Interpret(context);
                    tCodeExpression.Interpret(context);
                    coordintatesExpression.Interpret(context);
                    feedrateExpression.Interpret(context);
                    Program.Add(context.OutputData);
                    var currentValue = (float)((double)lineNumber / preparer.Strings.Count * 90 + 10);
                    if ((currentValue - progressValue) > 0.1)
                    {
                        progressValue = currentValue;
                        progress.Report(progressValue);
                    }
                }
            });
        }
        public virtual void Rewind(int lineNumber = 0)
        {
            Initialize();
            FrameNumber = 0;
            InitFrame();
            for (int i = 0; i < lineNumber; i++)
                NextFrame();
        }
        public virtual void NextFrame()
        {
            if (Program.Count > FrameNumber)
            {
                FrameNumber++;
                InitFrame();
            }
        }
        public virtual void SetFrame(int number)
        {
            if (Program.Count >= number)
            {
                for (int i = FrameNumber; i < number; i++)
                {
                    FrameNumber++;
                    InitFrame();
                }
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        public int FrameNumber { get; protected set; }
        public virtual GFrame CurrentFrame { get; private set; }
        public AxisCoordinates OldCoordinates { get; protected set; }
        public AxisCoordinates CurrentCoordinates { get; protected set; }
        public HashSet<float> CurrentGCodes { get; }
        public HashSet<float> CurrentMCodes { get; }
        public HashSet<float> ModalGCodes { get; }
        public HashSet<float> ModalMCodes { get; }
        public List<GFrame> Program { get; }
        public float ToolNumber { get; protected set; }
        public float FeedRate { get; protected set; }

        public event EventHandler<FrameChangedEventArgs> FrameChanged = delegate { };
        public event EventHandler ProgramEnded = delegate { };
    }
}
