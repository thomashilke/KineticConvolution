using System;

namespace Hilke.KineticConvolution
{
    public interface IAlgebraicNumber : IEquatable<IAlgebraicNumber>
    {
        IAlgebraicNumber Add(IAlgebraicNumber number);

        IAlgebraicNumber Subtract(IAlgebraicNumber number);

        IAlgebraicNumber MultiplyBy(IAlgebraicNumber number);

        IAlgebraicNumber DivideBy(IAlgebraicNumber number);

        IAlgebraicNumber Inverse();

        IAlgebraicNumber Opposite();

        IAlgebraicNumber SquareRoot();

        int Sign();
    }
}
