namespace Hilke.KineticConvolution
{
    public interface IAlgebraicNumberCalculator<TAlgebraicNumber>
    {
        TAlgebraicNumber Add(TAlgebraicNumber left, TAlgebraicNumber right);

        TAlgebraicNumber Subtract(TAlgebraicNumber left, TAlgebraicNumber right);

        TAlgebraicNumber Multiply(TAlgebraicNumber left, TAlgebraicNumber right);

        TAlgebraicNumber Divide(TAlgebraicNumber dividend, TAlgebraicNumber divisor);

        TAlgebraicNumber Inverse(TAlgebraicNumber number);

        TAlgebraicNumber Opposite(TAlgebraicNumber number);

        int Sign(TAlgebraicNumber number);

        TAlgebraicNumber SquareRoot(TAlgebraicNumber number);
    }
}
