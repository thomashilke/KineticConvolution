using System.Collections.Generic;
using System.Linq;

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
            yield return Case06();

            // Enumerate all combinatorial cases where range R = (S, E), range Rp = (Sp, Ep),
            // RBar = (E, S) is the complement of range R, R and Rp are counter clockwise,
            // S is not equal to E, and Sp is not equal to Ep.
            yield return Case_SpInRBar_EpInRBar1();
            yield return Case_SpInRBar_EpInRBar2();
            yield return Case_SpInRBar_EpEqualsS();
            yield return Case_SpInRBar_EpInR();
            yield return Case_SpInRBar_EpEqualsE();
            yield return Case_SpEqualsS_EpInR();
            yield return Case_SpEqualsS_EpEqualsE();
            yield return Case_SpEqualsS_EpInRBar();
            yield return Case_SpInR_EpInR();
            yield return Case_SpInR_EpEqualsE();
            yield return Case_SpInR_EpInRBar();
            yield return Case_SpInR_EpEqualsS();
            yield return Case_SpInR_EpInR2();
            yield return Case_SpEqualsE_EpInRBar();
            yield return Case_SpEqualsE_EpEqualsS();
            yield return Case_SpEqualsE_EpInR();

            // Enumerate all combinatorial cases where range R = (S, S) is degenerate
            // and Rp = (Sp, Ep), R and Rp are counter clockwise and Sp is not equal to Ep.
            yield return Case_RDegenerate_SpEqualsS_EpInR();
            yield return Case_RDegenerate_SpInR_EpInR1();
            yield return Case_RDegenerate_SpInR_EpEqualsS();
            yield return Case_RDegenerate_SpInR_EpInR2();

            // Enumerate all combinatorial cases where range R = (S, S) is degenerate
            // and Rp = (Sp, Sp) is degenerate.
            yield return Case_RDegenerateRpDegenerate_SEqualsSp();
            yield return Case_RDegenerateRpDegenerate_SpInR();
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

        private static TestCaseData Case06()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 0.0, y: -3.0),
                new Direction<double>(Calculator, x: 0.0, y: -3.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 0.0, y: -3.0),
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expectedIntersection = range2;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case06)}");
        }

        private static TestCaseData Case_SpInRBar_EpInRBar1()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: -1.0, y: 1.0),
                new Direction<double>(Calculator, x: -1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expectedIntersections = Enumerable.Empty<DirectionRange<double>>().ToList();

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpInRBar_EpInRBar1)}");
        }

        private static TestCaseData Case_SpInRBar_EpInRBar2()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: -1.0, y: 1.0),
                new Direction<double>(Calculator, x: -0.5, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = range1;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpInRBar_EpInRBar2)}");
        }

        private static TestCaseData Case_SpInRBar_EpEqualsS()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: -1.0, y: -1.0),
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expectedIntersections = Enumerable.Empty<DirectionRange<double>>().ToList();

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpInRBar_EpEqualsS)}");
        }

        private static TestCaseData Case_SpInRBar_EpInR()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: -1.0, y: -1.0),
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpInRBar_EpInR)}");
        }

        private static TestCaseData Case_SpInRBar_EpEqualsE()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: -1.0, y: -1.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = range1;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpInRBar_EpEqualsE)}");
        }

        private static TestCaseData Case_SpEqualsS_EpInR()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = range2;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpEqualsS_EpInR)}");
        }

        private static TestCaseData Case_SpEqualsS_EpEqualsE()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = range2;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpEqualsS_EpEqualsE)}");
        }

        private static TestCaseData Case_SpEqualsS_EpInRBar()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = range1;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpEqualsS_EpInRBar)}");
        }

        private static TestCaseData Case_SpInR_EpInR()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);


            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                new Direction<double>(Calculator, x: 0.5, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = range2;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpInR_EpInR)}");
        }

        private static TestCaseData Case_SpInR_EpEqualsE()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = range2;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpInR_EpEqualsE)}");
        }

        private static TestCaseData Case_SpInR_EpInRBar()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                new Direction<double>(Calculator, x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpInR_EpInRBar)}");
        }

        private static TestCaseData Case_SpInR_EpEqualsS()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpInR_EpEqualsS)}");
        }

        private static TestCaseData Case_SpInR_EpInR2()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                new Direction<double>(Calculator, x: 1.0, y: 0.5),
                Orientation.CounterClockwise);

            var expectedIntersection1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 1.0, y: 0.5),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection1, expectedIntersection2 };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpInR_EpInR2)}");
        }

        private static TestCaseData Case_SpEqualsE_EpInRBar()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                new Direction<double>(Calculator, x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>>();

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpEqualsE_EpInRBar)}");
        }

        private static TestCaseData Case_SpEqualsE_EpEqualsS()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>>();

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpEqualsE_EpEqualsS)}");
        }

        private static TestCaseData Case_SpEqualsE_EpInR()
        {
            var range1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 0.0),
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_SpEqualsE_EpInR)}");
        }

        private static TestCaseData Case_RDegenerate_SpEqualsS_EpInR()
        {
            var S = new Direction<double>(Calculator, x: 1.0, y: 0.0);

            var range1 = new DirectionRange<double>(
                Calculator,
                S, S,
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                S,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = range2;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_RDegenerate_SpEqualsS_EpInR)}");
        }

        private static TestCaseData Case_RDegenerate_SpInR_EpInR1()
        {
            var S = new Direction<double>(Calculator, x: 1.0, y: 0.0);

            var range1 = new DirectionRange<double>(
                Calculator,
                S, S,
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                new Direction<double>(Calculator, x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection = range2;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_RDegenerate_SpInR_EpInR1)}");
        }

        private static TestCaseData Case_RDegenerate_SpInR_EpEqualsS()
        {
            var S = new Direction<double>(Calculator, x: 1.0, y: 0.0);

            var range1 = new DirectionRange<double>(
                Calculator,
                S, S,
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                S,
                Orientation.CounterClockwise);

            var expectedIntersection = range2;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_RDegenerate_SpInR_EpEqualsS)}");
        }

        private static TestCaseData Case_RDegenerate_SpInR_EpInR2()
        {
            var S = new Direction<double>(Calculator, x: 1.0, y: 0.0);

            var range1 = new DirectionRange<double>(
                Calculator,
                S, S,
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersection1 = new DirectionRange<double>(
                Calculator,
                new Direction<double>(Calculator, x: 0.0, y: 1.0),
                S,
                Orientation.CounterClockwise);

            var expectedIntersection2 = new DirectionRange<double>(
                Calculator,
                S,
                new Direction<double>(Calculator, x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection1, expectedIntersection2 };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_RDegenerate_SpInR_EpInR2)}");
        }

        private static TestCaseData Case_RDegenerateRpDegenerate_SEqualsSp()
        {
            var S = new Direction<double>(Calculator, x: 1.0, y: 0.0);

            var range1 = new DirectionRange<double>(
                Calculator,
                S, S,
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                S,
                S,
                Orientation.CounterClockwise);

            var expectedIntersection = range2;

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_RDegenerateRpDegenerate_SEqualsSp)}");
        }

        private static TestCaseData Case_RDegenerateRpDegenerate_SpInR()
        {
            var S = new Direction<double>(Calculator, x: 1.0, y: 0.0);

            var Sp = new Direction<double>(Calculator, x: 0.0, y: 1.0);

            var range1 = new DirectionRange<double>(
                Calculator,
                S, S,
                Orientation.CounterClockwise);

            var range2 = new DirectionRange<double>(
                Calculator,
                Sp, Sp,
                Orientation.CounterClockwise);

            var expectedIntersection1 = new DirectionRange<double>(
                Calculator,
                S, Sp,
                Orientation.CounterClockwise);

            var expectedIntersection2 = new DirectionRange<double>(
                Calculator,
                Sp, S,
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expectedIntersection1, expectedIntersection2 };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeTestCaseDataSource)} - {nameof(Case_RDegenerateRpDegenerate_SpInR)}");
        }
    }
}
