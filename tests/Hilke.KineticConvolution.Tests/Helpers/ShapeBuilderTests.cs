using System;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;
using Hilke.KineticConvolution.Helpers;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.Helpers
{
    [TestFixture(TestOf = typeof(ShapeBuilder<>))]
    public class ShapeBuilderTests
    {
        private static readonly string? SaveFolder = null;

        private static readonly ConvolutionFactory<double> DoubleFactory =
            new(new DoubleAlgebraicNumberCalculator());

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_does_not_throw_exception()
        {
            // Arrange
            Action action = () => _ = new ShapeBuilder<double>(DoubleFactory);

            // Assert
            action.Should().NotThrow();
        }

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_create_a_valid_instance()
        {
            // Act
            var subject = new ShapeBuilder<double>(DoubleFactory);

            // Assert
            subject.Should().NotBeNull().And.BeAssignableTo<ShapeBuilder<double>>();
        }

        [Test]
        public void Given_invalid_parameters_When_calling_constructor_Then_throw_exception()
        {
            // Arrange
            Action action = () => _ = new ShapeBuilder<double>(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("factory");
        }

        [Test]
        public void Given_valid_instructions_When_creating_minimal_shape_Should_create_expected()
        {
            // Arrange
            var calc = DoubleFactory.AlgebraicNumberCalculator;
            var subject = new ShapeBuilder<double>(DoubleFactory);

            var p0 = DoubleFactory.CreatePoint(0.0, 0.0);
            var p1 = DoubleFactory.CreatePoint(1.0, 0.0);

            subject.StartAt(p0)
                   .AddCorner(p1, calc.CreateConstant(0.0))
                   .CloseWith(calc.CreateConstant(0.0));

            // Act
            var shape = subject.Build();

            // Assert
            shape.Tracings.Should().HaveCount(4);

            if (SaveFolder is not null)
            {
                new SvgWriter().Add(shape.Tracings)
                               .Write($"{SaveFolder}\\minimal_shape.svg");
            }
        }

        [Test]
        [TestCase(1.0, 0.0, TestName = "square 01")]
        [TestCase(1.0, 0.1, TestName = "square 02")]
        [TestCase(1.0, 0.4, TestName = "square 03")]
        public void Given_valid_instructions_When_creating_square_Should_create_expected(double side, double radius)
        {
            // Arrange
            var calc = DoubleFactory.AlgebraicNumberCalculator;
            var subject = new ShapeBuilder<double>(DoubleFactory);

            var p0 = DoubleFactory.CreatePoint(0.0, 0.0);
            var p1 = DoubleFactory.CreatePoint(side, 0.0);
            var p2 = DoubleFactory.CreatePoint(side, side);
            var p3 = DoubleFactory.CreatePoint(0.0, side);

            subject.StartAt(p0)
                   .AddCorner(p1, calc.CreateConstant(radius))
                   .AddCorner(p2, calc.CreateConstant(radius))
                   .AddCorner(p3, calc.CreateConstant(radius))
                   .CloseWith(calc.CreateConstant(radius));

            // Act
            var shape = subject.Build();

            // Assert
            shape.Tracings.Should().HaveCount(8);

            if (SaveFolder is not null)
            {
                new SvgWriter().Add(shape.Tracings)
                               .Write($"{SaveFolder}\\square_{side}_{radius}.svg");
            }
        }

        [Test]
        [TestCase(1.0, 0.0, TestName = "triangle 01")]
        [TestCase(1.0, 0.1, TestName = "triangle 02")]
        [TestCase(1.0, 0.2, TestName = "triangle 03")]
        public void Given_valid_instructions_When_creating_triangle_Should_create_expected(double side, double radius)
        {
            // Arrange
            var calc = DoubleFactory.AlgebraicNumberCalculator;
            var subject = new ShapeBuilder<double>(DoubleFactory);

            var p0 = DoubleFactory.CreatePoint(0.0, 0.0);
            var p1 = DoubleFactory.CreatePoint(side, 0.0);
            var p2 = DoubleFactory.CreatePoint(0.0, side);

            subject.StartAt(p0)
                   .AddCorner(p1, calc.CreateConstant(radius))
                   .AddCorner(p2, calc.CreateConstant(radius))
                   .CloseWith(calc.CreateConstant(radius));

            // Act
            var shape = subject.Build();

            // Assert
            shape.Tracings.Should().HaveCount(6);

            if (SaveFolder is not null)
            {
                new SvgWriter().Add(shape.Tracings)
                               .Write($"{SaveFolder}\\triangle_{side}_{radius}.svg");
            }
        }

        [Test]
        [TestCase(1.0, 0.0, 8, TestName = "L-shape 01")]
        [TestCase(1.0, 0.1, 8, TestName = "L-shape 02")]
        [TestCase(1.0, 0.4, 8, TestName = "L-shape 03")]
        [TestCase(1.0, 1.0, 4, TestName = "L-shape 04")]
        public void Given_valid_instructions_When_creating_lshape_Should_create_expected(double side, double radius, int expectedCount)
        {
            // Arrange
            var calc = DoubleFactory.AlgebraicNumberCalculator;
            var subject = new ShapeBuilder<double>(DoubleFactory);

            var p0 = DoubleFactory.CreatePoint(0.0, 0.0);
            var p1 = DoubleFactory.CreatePoint(side, 0.0);
            var p2 = DoubleFactory.CreatePoint(0.0, side);

            subject.StartAt(p0)
                   .AddCorner(p1, calc.CreateConstant(0.0))
                   .AddCorner(p0, calc.CreateConstant(radius))
                   .AddCorner(p2, calc.CreateConstant(0.0))
                   .CloseWith(calc.CreateConstant(radius));

            // Act
            var shape = subject.Build();

            // Assert
            shape.Tracings.Should().HaveCount(expectedCount);

            if (SaveFolder is not null)
            {
                new SvgWriter().Add(shape.Tracings)
                               .Write($"{SaveFolder}\\lshape_{side}_{radius}.svg");
            }
        }

        [Test]
        [TestCase(1.0, TestName = "flat profile 01")]
        public void Given_valid_instructions_When_creating_flat_profile_Should_create_expected(double side)
        {
            // Arrange
            var calc = DoubleFactory.AlgebraicNumberCalculator;
            var subject = new ShapeBuilder<double>(DoubleFactory);

            var p0 = DoubleFactory.CreatePoint(0.0, 0.0);
            var p1 = DoubleFactory.CreatePoint(side, 0.0);
            var p2 = DoubleFactory.CreatePoint(2.0 * side, 0.0);

            subject.StartAt(p0)
                   .AddCorner(p1, calc.CreateConstant(0.0))
                   .AddCorner(p2, calc.CreateConstant(0.0))
                   .AddCorner(p1, calc.CreateConstant(0.0))
                   .CloseWith(calc.CreateConstant(0.0));

            // Act
            var shape = subject.Build();

            // Assert
            shape.Tracings.Should().HaveCount(6);

            if (SaveFolder is not null)
            {
                new SvgWriter().Add(shape.Tracings)
                               .Write($"{SaveFolder}\\flat_{side}.svg");
            }
        }
    }
}
