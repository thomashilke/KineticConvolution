using System.Collections.Generic;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.TestCaseDataSource
{
    public static class DirectionTestCaseDataSource
    {
        private static readonly ConvolutionFactory Factory = new();

        public static IEnumerable<TestCaseData> TestCases()
        {
            yield return Case01();
            yield return Case02();
            yield return Case03();
        }

        private static TestCaseData Case01()
        {
            var subject = new Direction<double>(Factory.AlgebraicNumberCalculator, x: 1.0, y: 1.0);

            var direction1 = Factory.CreateDirection(x: 1.0, y: 0.0);
            var direction2 = Factory.CreateDirection(x: 0.0, y: 1.0);

            var expected = direction1;
            return new TestCaseData(subject, direction1, direction2, expected)
                .SetName($"{nameof(DirectionTestCaseDataSource)} - {nameof(Case01)}");
        }

        private static TestCaseData Case02()
        {
            var subject = new Direction<double>(Factory.AlgebraicNumberCalculator, x: 1.0, y: 1.0);

            var direction1 = Factory.CreateDirection(x: 1.0, y: 0.0);
            var direction2 = Factory.CreateDirection(x: 1.0, y: 0.0);

            var expected = direction1;
            return new TestCaseData(subject, direction1, direction2, expected)
                .SetName($"{nameof(DirectionTestCaseDataSource)} - {nameof(Case02)}");
        }

        private static TestCaseData Case03()
        {
            var subject = new Direction<double>(Factory.AlgebraicNumberCalculator, x: 1.0, y: 0.0);

            var direction1 = Factory.CreateDirection(x: 0.5, y: 0.5);
            var direction2 = Factory.CreateDirection(x: 0.4, y: 0.8);

            var expected = direction2;
            return new TestCaseData(subject, direction1, direction2, expected)
                .SetName($"{nameof(DirectionTestCaseDataSource)} - {nameof(Case03)}");
        }
    }
}
