using System;

namespace Hilke.KineticConvolution.Double
{
    public interface IAlgebraicNumber<TAlgebraicNumber> : IEquatable<IAlgebraicNumber<TAlgebraicNumber>>
        where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
    {
        TAlgebraicNumber Add(TAlgebraicNumber number);

        TAlgebraicNumber Subtract(TAlgebraicNumber number);

        TAlgebraicNumber Multiply(TAlgebraicNumber number);

        TAlgebraicNumber Divide(TAlgebraicNumber number);

        TAlgebraicNumber Inverse();

        TAlgebraicNumber Opposite();

        TAlgebraicNumber SquareRoot();

        int Sign();
    }
}
