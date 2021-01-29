namespace Hilke.KineticConvolution.DoubleAlgebraicNumber
{
    public sealed class ConvolutionFactory : ConvolutionFactory<double>
    {
        /// <inheritdoc />
        public ConvolutionFactory()
            : base(new DoubleAlgebraicNumberCalculator()) { }
    }
}
