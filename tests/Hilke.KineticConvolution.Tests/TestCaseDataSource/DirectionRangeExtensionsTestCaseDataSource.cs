using System.Collections.Generic;
using System.Linq;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.TestCaseDataSource
{
    internal static class DirectionRangeExtensionsTestCaseDataSource
    {
        public static IEnumerable<TestCaseData> TestCases()
        {
            var factory = new ConvolutionFactory();

            yield return new TestCaseData(
                Orientation.CounterClockwise, Enumerable.Empty<DirectionRange<double>>());

            yield return new TestCaseData(
                Orientation.Clockwise,
                new []
                {
                    factory.CreateDirectionRange(
                        factory.CreateDirection(0, 1),
                        factory.CreateDirection(0, -1),
                        Orientation.Clockwise)
                });
        }
    }
}
