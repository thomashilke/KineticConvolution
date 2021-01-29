﻿using System;
using System.Collections.Generic;

namespace Hilke.KineticConvolution
{
    public sealed class Shape<TAlgebraicNumber>
    {
        internal Shape(IReadOnlyList<Tracing<TAlgebraicNumber>> tracings) => Tracings = tracings;

        public IReadOnlyList<Tracing<TAlgebraicNumber>> Tracings { get; }
    }
}
