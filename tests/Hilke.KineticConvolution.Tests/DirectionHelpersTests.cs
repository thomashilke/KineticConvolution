using FluentAssertions;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class DirectionHelpersTests
    {
        private const double _equalityTolerance = 1.0e-9;

        [Test]
        public void Direction_Determinant_Should_Have_Expected_Sign()
        {
            var d1 = new Direction<DoubleNumber>(DoubleNumber.FromDouble(1.0), DoubleNumber.FromDouble(1.0));
            var d2 = new Direction<DoubleNumber>(DoubleNumber.FromDouble(-1.0), DoubleNumber.FromDouble(1.0));

            var determinant1 = DirectionHelper.Determinant(d1, d2);
            var determinant2 = DirectionHelper.Determinant(d2, d1);

            (determinant1 as DoubleNumber).Value.Should().BeApproximately(2.0, _equalityTolerance);

            determinant1.Sign().Should().Be(1);
            determinant2.Sign().Should().Be(-1);
        }
    }
}
