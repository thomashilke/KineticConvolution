using System;

namespace Hilke.KineticConvolution.Double
{
    internal static class DirectionHelper
    {
        public static double Determinant(
            Direction d1,
            Direction d2)
        {
            if (d1 == null)
            {
                throw new ArgumentNullException(nameof(d1));
            }

            if (d2 == null)
            {
                throw new ArgumentNullException(nameof(d2));
            }

            var a = d1.X * d2.Y;
            var b = d1.Y * d2.X;
            return a - b;
        }
    }
}
