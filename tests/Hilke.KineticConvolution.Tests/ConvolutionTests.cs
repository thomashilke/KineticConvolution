using System;
using System.Collections.Generic;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class ConvolutionTests
    {
        private ConvolutionFactory<double> _factory;
        private Shape<double> _shape1;
        private Shape<double> _shape2;
        private IReadOnlyList<ConvolvedTracing<double>> _convolvedTracings;

        [SetUp]
        public void SetUp()
        {
            var calculator = new DoubleAlgebraicNumberCalculator();
            _factory = new ConvolutionFactory<double>(calculator);

            var segment = _factory.CreateSegment(
                startX: 10,
                startY: 5,
                endX: 15,
                endY: 10,
                weight: 4);

            var tracings = new List<Tracing<double>> {segment};
            _shape1 = new Shape<double>(tracings);
            _shape2 = new Shape<double>(tracings);
            _convolvedTracings = new List<ConvolvedTracing<double>>();
        }

        [Test]
        public void When_calling_Convolution_With_null_shape1_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            Action action = () => _ = new Convolution<double>(null!, _shape1, _convolvedTracings);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("shape1");
        }

        [Test]
        public void When_calling_Convolution_With_null_shape2_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            Action action = () => _ = new Convolution<double>(_shape1, null!, _convolvedTracings);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("shape2");
        }

        [Test]
        public void
            When_calling_Convolution_With_null_convolvedTracings_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            Action action = () => _ = new Convolution<double>(_shape1, _shape2, null!);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("convolvedTracings");
        }

        [Test]
        public void When_calling_Shape1_Then_the_correct_instance_Should_be_returned()
        {
            // Arrange
            var subject = new Convolution<double>(_shape1, _shape2, _convolvedTracings);

            var actual = subject.Shape1;

            // Assert
            actual.Should().BeSameAs(_shape1);
        }

        [Test]
        public void When_calling_Shape2_Then_the_correct_instance_Should_be_returned()
        {
            // Arrange
            var subject = new Convolution<double>(_shape1, _shape2, _convolvedTracings);

            var actual = subject.Shape2;

            // Assert
            actual.Should().BeSameAs(_shape2);
        }

        [Test]
        public void When_calling_ConvolvedTracings_Then_the_correct_instance_Should_be_returned()
        {
            // Arrange
            var subject = new Convolution<double>(_shape1, _shape2, _convolvedTracings);

            var actual = subject.ConvolvedTracings;

            // Assert
            actual.Should().BeSameAs(_convolvedTracings);
        }
    }
}
