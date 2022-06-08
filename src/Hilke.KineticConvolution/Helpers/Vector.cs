using System;
using System.Collections.Generic;
using System.Linq;

namespace Hilke.KineticConvolution.Helpers
{
    public class Vector<TAlgebraicNumber>
    {
        private Vector(IReadOnlyList<TAlgebraicNumber> coordinates)
        {
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
            Dimension = coordinates.Count;
        }

        public int Dimension { get; }

        public IReadOnlyList<TAlgebraicNumber> Coordinates { get; }

        public static Vector<TAlgebraicNumber> FromEnumerable(IEnumerable<TAlgebraicNumber> coordinates)
        {
            var enumerated = coordinates?.ToList() ?? throw new ArgumentNullException(nameof(coordinates));
            return new Vector<TAlgebraicNumber>(enumerated);
        }
    }
}
