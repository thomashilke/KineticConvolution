﻿using System;
using System.Collections.Generic;

namespace Hilke.KineticConvolution
{
    public class Shape<TAlgebraicNumber>
        where TAlgebraicNumber : IEquatable<TAlgebraicNumber>
    {
        internal Shape(IReadOnlyList<Tracing<TAlgebraicNumber>> tracings) => Tracings = tracings;

        public IReadOnlyList<Tracing<TAlgebraicNumber>> Tracings { get; }
    }
}
