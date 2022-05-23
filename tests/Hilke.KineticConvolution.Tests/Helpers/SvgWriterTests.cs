using System;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;
using Hilke.KineticConvolution.Helpers;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.Helpers
{
    [TestFixture(TestOf = typeof(SvgWriter))]
    public class SvgWriterTests
    {
        private static readonly ConvolutionFactory<double> DoubleFactory =
            new(new DoubleAlgebraicNumberCalculator());

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_does_not_throw_exception()
        {
            // Arrange
            Action action = () => _ = new SvgWriter();

            // Assert
            action.Should().NotThrow();
        }

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_create_a_valid_instance()
        {
            // Act
            var subject = new SvgWriter();

            // Assert
            subject.Should().NotBeNull().And.BeAssignableTo<SvgWriter>();
        }

        [Test]
        public void Given_tracings_When_calling_BuildSvgContent_Then_create_a_valid_instance()
        {
            // Arrange
            var subject = new SvgWriter();
            var tracings = new Tracing<double>[]
            {
                DoubleFactory.CreateSegment(0.0, 1.0, 1.0, 0.0, 1),
                DoubleFactory.CreateArc(1.0, 1.0, -1.0, 0.0, 0.0, 1.0, Orientation.CounterClockwise, 0.4, 1)
            };

            // Act
            var svgContent = subject.Add(tracings)
                                    .BuildSvgContent();

            // Assert
            svgContent.Should()
                      .Contain("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>", Exactly.Once());
            svgContent.Should().Contain("<svg", Exactly.Once());
            svgContent.Should().Contain("</svg>", Exactly.Once());
        }
    }
}
