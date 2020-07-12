namespace GcodeParser
{
    public class AxisCoordinates
    {
        public AxisCoordinates()
        {

        }
        public AxisCoordinates(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public AxisCoordinates(float a, float b, float c, float u, float v, float w, float x, float y, float z) : this(x, y, z)
        {
            A = a;
            B = b;
            C = c;
            U = u;
            V = v;
            W = w;
        }

        public bool NotEmpty()
        {
            return A.HasValue || B.HasValue || C.HasValue || U.HasValue || V.HasValue || W.HasValue || X.HasValue || Y.HasValue || Z.HasValue;
        }

        public float? A { get; set; }
        public float? B { get; set; }
        public float? C { get; set; }
        public float? U { get; set; }
        public float? V { get; set; }
        public float? W { get; set; }
        public float? X { get; set; }
        public float? Y { get; set; }
        public float? Z { get; set; }
    }
}