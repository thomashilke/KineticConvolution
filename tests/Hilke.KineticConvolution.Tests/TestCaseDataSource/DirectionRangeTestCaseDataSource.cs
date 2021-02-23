using System.Collections.Generic;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.TestCaseDataSource
{
    internal static class DirectionRangeTestCaseDataSource
    {
        private static readonly IAlgebraicNumberCalculator<double> Calculator =
            new DoubleAlgebraicNumberCalculator(zeroTolerance: 1e-12);

        public static IEnumerable<TestCaseData> TestCases()
        {
            yield return Case01();
            yield return Case02();
            yield return Case03();
            yield return Case04();
            yield return Case05();
        }

        private static TestCaseData Case01()
        {
            var start1 = new Direction<double>(Calculator, x: 1.0, y: 0.0);
            var end1 = new Direction<double>(Calculator, x: 0.0, y: 1.0);

            var start2 = new Direction<double>(Calculator, x: 0.5, y: 0.5);
            var end2 = new Direction<double>(Calculator, x: 0.0, y: -1.0);

            var range1 = new DirectionRange<double>(Calculator, start1, end1, Orientation.CounterClockwise);
            var range2 = new DirectionRange<double>(Calculator, start2, end2, Orientation.Clockwise);

            var expectedRange = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.5, y: 0.5),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> {expectedRange};

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case01)}");
        }

        private static TestCaseData Case02()
        {
            var start1 = new Direction<double>(Calculator, x: 1.0, y: 0.0);
            var end1 = new Direction<double>(Calculator, x: 0.0, y: 1.0);

            var start2 = new Direction<double>(Calculator, x: 0.5, y: 0.5);
            var end2 = new Direction<double>(Calculator, x: 0.7, y: 0.3);

            var range1 = new DirectionRange<double>(Calculator, start1, end1, Orientation.CounterClockwise);
            var range2 = new DirectionRange<double>(Calculator, start2, end2, Orientation.CounterClockwise);

            var expectedRange1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.7, y: 0.3),
                Orientation.CounterClockwise);

            var expectedRange2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 0.5, y: 0.5),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> {expectedRange1, expectedRange2};

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case02)}");
        }

        private static TestCaseData Case03()
        {
            var start1 = new Direction<double>(Calculator, x: 1.0, y: 0.0);
            var end1 = new Direction<double>(Calculator, x: 0.0, y: 1.0);

            var start2 = new Direction<double>(Calculator, x: 1.0, y: 0.0);
            var end2 = new Direction<double>(Calculator, x: 0.0, y: 1.0);

            var range1 = new DirectionRange<double>(Calculator, start1, end1, Orientation.CounterClockwise);
            var range2 = new DirectionRange<double>(Calculator, start2, end2, Orientation.CounterClockwise);

            var expectedRange = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> {expectedRange};

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case03)}");
        }

        private static TestCaseData Case04()
        {
            var start1 = new Direction<double>(Calculator, x: 1.0, y: 0.0);
            var end1 = new Direction<double>(Calculator, x: 0.0, y: 1.0);

            var start2 = new Direction<double>(Calculator, x: 0.0, y: 1.0);
            var end2 = new Direction<double>(Calculator, x: 1.0, y: 0.0);

            var range1 = new DirectionRange<double>(Calculator, start1, end1, Orientation.CounterClockwise);
            var range2 = new DirectionRange<double>(Calculator, start2, end2, Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> ();

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case04)}");
        }

        private static TestCaseData Case05()
        {
            var start1 = new Direction<double>(Calculator, x: 0.0, y: 1.0);
            var end1 = new Direction<double>(Calculator, x: 0.0, y: 1.0);

            var start2 = new Direction<double>(Calculator, x: 1.0, y: 0.0);
            var end2 = new Direction<double>(Calculator, x: -1.0, y: 0.0);

            var range1 = new DirectionRange<double>(Calculator, start1, end1, Orientation.CounterClockwise);
            var range2 = new DirectionRange<double>(Calculator, start2, end2, Orientation.CounterClockwise);

            var expectedRange1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedRange2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                new Direction<double>(Calculator, x: -1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> {expectedRange1, expectedRange2};

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case05)}");
        }
    }
}
