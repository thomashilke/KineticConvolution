namespace KineticConvolution
{
    public interface IAlgebraicNumber
    {
        IAlgebraicNumber Add(IAlgebraicNumber number);

        IAlgebraicNumber Substract(IAlgebraicNumber number);

        IAlgebraicNumber MultipliedBy(IAlgebraicNumber number);

        IAlgebraicNumber DividedBy(IAlgebraicNumber number);

        IAlgebraicNumber Inverse();

        IAlgebraicNumber Opposite();

        IAlgebraicNumber SquareRoot();

        int Sign();
    }
}
