using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class DirectionHelpersTests
    {
        private const double EqualityTolerance = 1.0e-9;

        [Test]
        public void Direction_Determinant_Should_Have_Expected_Sign()
        {
            var factory = new ConvolutionFactory<double>(new DoubleAlgebraicNumberCalculator());

            var d1 = factory.CreateDirection(1.0, 1.0);
            var d2 = factory.CreateDirection(-1.0, 1.0);

            var determinant1 = d1.Determinant(d2);
            var determinant2 = d2.Determinant(d1);

            determinant1.Should().BeApproximately(2.0, EqualityTolerance);
            determinant1.Should().BeGreaterThan(0.0);
            determinant2.Should().BeLessThan(0.0);
        }
    }
}
