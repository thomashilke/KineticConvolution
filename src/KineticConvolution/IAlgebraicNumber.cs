using System;

namespace Hilke.KineticConvolution
{
    public interface IAlgebraicNumber<TAlgebraicNumber> : IEquatable<IAlgebraicNumber<TAlgebraicNumber>>
        where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
    {
        TAlgebraicNumber Add(TAlgebraicNumber number);

        TAlgebraicNumber Subtract(TAlgebraicNumber number);

        TAlgebraicNumber MultipliedBy(TAlgebraicNumber number);

        TAlgebraicNumber DividedBy(TAlgebraicNumber number);

        TAlgebraicNumber Inverse();

        TAlgebraicNumber Opposite();

        TAlgebraicNumber SquareRoot();

        int Sign();
    }
}
