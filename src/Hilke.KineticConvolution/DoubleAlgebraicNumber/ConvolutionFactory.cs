namespace Hilke.KineticConvolution.DoubleAlgebraicNumber
{
    public class ConvolutionFactory : ConvolutionFactory<double>
    {
        /// <inheritdoc />
        public ConvolutionFactory()
            : base(new DoubleAlgebraicNumberCalculator()) { }
    }
}
