using System.Collections.Generic;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.TestCaseDataSource
{
    internal static class DirectionRangeExtensionsUnionTestCaseDataSource
    {
        private static readonly IAlgebraicNumberCalculator<double> _calculator =
            new DoubleAlgebraicNumberCalculator(zeroTolerance: 1.0e-12);

        private static readonly ConvolutionFactory<double> _factory =
            new ConvolutionFactory<double>(_calculator);

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

        public static IEnumerable<TestCaseData> TestCasesMultipleUnion()
        {
            yield return MultipleUnionCase01();
            yield return MultipleUnionCase02();
        }

        public static IEnumerable<TestCaseData> TestCasesSortByStart()
        {
            yield return SortCase01();
        }

        private static TestCaseData MultipleUnionCase01()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);
            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 1.0),
                _factory.CreateDirection(x: -1.0, y: 1.0),
                Orientation.CounterClockwise);
            var range3 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: -1.0),
                Orientation.CounterClockwise);
            var range4 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: -1.0),
                _factory.CreateDirection(x: 1.0, y: -1.0),
                Orientation.CounterClockwise);
            var range5 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 7.0, y: -3.0),
                _factory.CreateDirection(x: -0.1, y: 1.0),
                Orientation.CounterClockwise);

            var ranges = new List<DirectionRange<double>> { range1, range2, range3, range4, range5 };

            var union1 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: 0.0),
                _factory.CreateDirection(x: 1.0, y: -1.0),
                Orientation.CounterClockwise);
            var union2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 7.0, y: -3.0),
                _factory.CreateDirection(x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { union1, union2 };
            return new TestCaseData(ranges, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(MultipleUnionCase01)}");
        }

        private static TestCaseData MultipleUnionCase02()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 1.0, y: 1.0),
                Orientation.CounterClockwise);
            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 0.0, y: 1.0),
                _factory.CreateDirection(x: -1.0, y: 1.0),
                Orientation.CounterClockwise);
            var range3 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: -1.0),
                Orientation.CounterClockwise);
            var range4 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: -1.0),
                _factory.CreateDirection(x: 1.0, y: -1.0),
                Orientation.CounterClockwise);
            var range5 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 7.0, y: -3.0),
                _factory.CreateDirection(x: -0.1, y: 1.0),
                Orientation.CounterClockwise);

            var ranges = new List<DirectionRange<double>> { range1, range2, range3, range4, range5 };

            var union1 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: 0.0),
                _factory.CreateDirection(x: 1.0, y: -1.0),
                Orientation.CounterClockwise);
            var union2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 7.0, y: -3.0),
                _factory.CreateDirection(x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { union1, union2 };
            return new TestCaseData(ranges, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(MultipleUnionCase02)}");
        }

        private static TestCaseData SortCase01()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);
            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 1.0),
                _factory.CreateDirection(x: -1.0, y: 1.0),
                Orientation.CounterClockwise);
            var range3 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: -1.0),
                Orientation.CounterClockwise);
            var range4 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: -1.0),
                _factory.CreateDirection(x: 1.0, y: -1.0),
                Orientation.CounterClockwise);
            var range5 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 7.0, y: -3.0),
                _factory.CreateDirection(x: -0.1, y: 1.0),
                Orientation.CounterClockwise);

            var unsortedRanges = new List<DirectionRange<double>> { range1, range4, range3, range5, range2 };

            var expected = new List<DirectionRange<double>> { range1, range2, range3, range4, range5 };

            return new TestCaseData(unsortedRanges, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(SortCase01)}");
        }

        private static TestCaseData Case01()
        {
            var start1 = _factory.CreateDirection(1.0, 0.0);
            var end1 = _factory.CreateDirection(0.0, 1.0);

            var start2 = _factory.CreateDirection(0.5,  0.5);
            var end2 = _factory.CreateDirection(0.0, -1.0);

            var range1 = _factory.CreateDirectionRange( start1, end1, Orientation.CounterClockwise);
            var range2 = _factory.CreateDirectionRange( start2, end2, Orientation.Clockwise);

            var expectedRange = _factory.CreateDirectionRange(
                _factory.CreateDirection(0.0, -1.0),
                _factory.CreateDirection(0.0, 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { expectedRange };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case01)}");
        }

        private static TestCaseData Case02()
        {
            var start1 = _factory.CreateDirection(1.0, 0.0);
            var end1 = _factory.CreateDirection(0.0, 1.0);

            var start2 = _factory.CreateDirection(0.5, 0.5);
            var end2 = _factory.CreateDirection( 0.7, 0.3);

            var range1 = _factory.CreateDirectionRange(start1, end1, Orientation.CounterClockwise);
            var range2 = _factory.CreateDirectionRange(start2, end2, Orientation.CounterClockwise);

            var expectedRange = _factory.CreateDirectionRange(
                _factory.CreateDirection(1.0, 0.0),
                _factory.CreateDirection(1.0, 0.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { expectedRange, };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case02)}");
        }

        private static TestCaseData Case03()
        {
            var start1 = _factory.CreateDirection( x: 1.0, y: 0.0);
            var end1 = _factory.CreateDirection( x: 0.0, y: 1.0);

            var start2 = _factory.CreateDirection( x: 1.0, y: 0.0);
            var end2 = _factory.CreateDirection(x: 0.0, y: 1.0);

            var range1 = _factory.CreateDirectionRange(start1, end1, Orientation.CounterClockwise);
            var range2 = _factory.CreateDirectionRange(start2, end2, Orientation.CounterClockwise);

            var expectedRange = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { expectedRange };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case03)}");
        }

        private static TestCaseData Case04()
        {
            var start1 = _factory.CreateDirection( x: 1.0, y: 0.0);
            var end1 = _factory.CreateDirection( x: 0.0, y: 1.0);

            var start2 = _factory.CreateDirection( x: 0.0, y: 1.0);
            var end2 = _factory.CreateDirection( x: 1.0, y: 0.0);

            var range1 = _factory.CreateDirectionRange(start1, end1, Orientation.CounterClockwise);
            var range2 = _factory.CreateDirectionRange(start2, end2, Orientation.CounterClockwise);

            var expectedRange = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { expectedRange };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case04)}");
        }

        private static TestCaseData Case05()
        {
            var start1 = _factory.CreateDirection( x: 0.0, y: 1.0);
            var end1 = _factory.CreateDirection( x: 0.0, y: 1.0);

            var start2 = _factory.CreateDirection( x: 1.0, y: 0.0);
            var end2 = _factory.CreateDirection( x: -1.0, y: 0.0);

            var range1 = _factory.CreateDirectionRange( start1, end1, Orientation.CounterClockwise);
            var range2 = _factory.CreateDirectionRange( start2, end2, Orientation.CounterClockwise);

            var expectedRange = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 0.0, y: 1.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { expectedRange };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case05)}");
        }

        private static TestCaseData Case06()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 0.0, y: -3.0),
                _factory.CreateDirection( x: 0.0, y: -3.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 0.0, y: -3.0),
                _factory.CreateDirection( x: 1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expectedUnion = range1;

            var expected = new List<DirectionRange<double>> { expectedUnion };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case06)}");
        }

        private static TestCaseData Case_SpInRBar_EpInRBar1()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: -1.0, y: 1.0),
                _factory.CreateDirection( x: -1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { range1, range2 };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpInRBar_EpInRBar1)}");
        }

        private static TestCaseData Case_SpInRBar_EpInRBar2()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: -1.0, y: 1.0),
                _factory.CreateDirection( x: -0.5, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { range2 };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpInRBar_EpInRBar2)}");
        }

        private static TestCaseData Case_SpInRBar_EpEqualsS()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: -1.0),
                _factory.CreateDirection(x: 1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expected = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: -1.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expected };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpInRBar_EpEqualsS)}");
        }

        private static TestCaseData Case_SpInRBar_EpInR()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: -1.0),
                _factory.CreateDirection(x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: -1.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expected };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpInRBar_EpInR)}");
        }

        private static TestCaseData Case_SpInRBar_EpEqualsE()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: -1.0, y: -1.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { range2 };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpInRBar_EpEqualsE)}");
        }

        private static TestCaseData Case_SpEqualsS_EpInR()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { range1 };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpEqualsS_EpInR)}");
        }

        private static TestCaseData Case_SpEqualsS_EpEqualsE()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { range1 };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpEqualsS_EpEqualsE)}");
        }

        private static TestCaseData Case_SpEqualsS_EpInRBar()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { range2 };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpEqualsS_EpInRBar)}");
        }

        private static TestCaseData Case_SpInR_EpInR()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 0.0, y: 1.0),
                Orientation.CounterClockwise);


            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 1.0),
                _factory.CreateDirection(x: 0.5, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { range1 };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpInR_EpInR)}");
        }

        private static TestCaseData Case_SpInR_EpEqualsE()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 1.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { range1 };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpInR_EpEqualsE)}");
        }

        private static TestCaseData Case_SpInR_EpInRBar()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 1.0),
                _factory.CreateDirection( x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expected };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpInR_EpInRBar)}");
        }

        private static TestCaseData Case_SpInR_EpEqualsS()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 1.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedIntersections = new List<DirectionRange<double>> { expected };

            return new TestCaseData(range1, range2, expectedIntersections)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpInR_EpEqualsS)}");
        }

        private static TestCaseData Case_SpInR_EpInR2()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 1.0),
                _factory.CreateDirection( x: 1.0, y: 0.5),
                Orientation.CounterClockwise);

            var expectedUnion = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { expectedUnion };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpInR_EpInR2)}");
        }

        // interesting for union
        private static TestCaseData Case_SpEqualsE_EpInRBar()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 0.0, y: 1.0),
                _factory.CreateDirection( x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedUnion = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { expectedUnion };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpEqualsE_EpInRBar)}");
        }

        // interesting for union
        private static TestCaseData Case_SpEqualsE_EpEqualsS()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 0.0, y: 1.0),
                _factory.CreateDirection( x: 1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expectedUnion = _factory.CreateDirectionRange(
                _factory.CreateDirection(x: 1.0, y: 0.0),
                _factory.CreateDirection(x: 1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { expectedUnion };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpEqualsE_EpEqualsS)}");
        }

        private static TestCaseData Case_SpEqualsE_EpInR()
        {
            var range1 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 0.0, y: 1.0),
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 0.0, y: 1.0),
                _factory.CreateDirection( x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedUnion = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 0.0),
                _factory.CreateDirection( x: 1.0, y: 0.0),
                Orientation.CounterClockwise);

            var expectedUnions = new List<DirectionRange<double>> { expectedUnion };

            return new TestCaseData(range1, range2, expectedUnions)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_SpEqualsE_EpInR)}");
        }

        private static TestCaseData Case_RDegenerate_SpEqualsS_EpInR()
        {
            var S = _factory.CreateDirection( x: 1.0, y: 0.0);

            var range1 = _factory.CreateDirectionRange(
                S, S,
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                S,
                _factory.CreateDirection( x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedUnion = range1;

            var expected = new List<DirectionRange<double>> { expectedUnion };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_RDegenerate_SpEqualsS_EpInR)}");
        }

        private static TestCaseData Case_RDegenerate_SpInR_EpInR1()
        {
            var S = _factory.CreateDirection( x: 1.0, y: 0.0);

            var range1 = _factory.CreateDirectionRange(
                S, S,
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 1.0),
                _factory.CreateDirection( x: -1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expectedUnion = range1;

            var expected = new List<DirectionRange<double>> { expectedUnion };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_RDegenerate_SpInR_EpInR1)}");
        }

        private static TestCaseData Case_RDegenerate_SpInR_EpEqualsS()
        {
            var S = _factory.CreateDirection( x: 1.0, y: 0.0);

            var range1 = _factory.CreateDirectionRange(
                S, S,
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 1.0, y: 1.0),
                S,
                Orientation.CounterClockwise);

            var expectedUnion = range1;

            var expected = new List<DirectionRange<double>> { expectedUnion };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_RDegenerate_SpInR_EpEqualsS)}");
        }

        private static TestCaseData Case_RDegenerate_SpInR_EpInR2()
        {
            var S = _factory.CreateDirection( x: 1.0, y: 0.0);

            var range1 = _factory.CreateDirectionRange(
                S, S,
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                _factory.CreateDirection( x: 0.0, y: 1.0),
                _factory.CreateDirection( x: 1.0, y: 1.0),
                Orientation.CounterClockwise);

            var expected = new List<DirectionRange<double>> { range1 };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_RDegenerate_SpInR_EpInR2)}");
        }

        private static TestCaseData Case_RDegenerateRpDegenerate_SEqualsSp()
        {
            var S = _factory.CreateDirection( x: 1.0, y: 0.0);

            var range1 = _factory.CreateDirectionRange(
                S, S,
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                S,
                S,
                Orientation.CounterClockwise);

            var expectedUnion = range2;

            var expected = new List<DirectionRange<double>> { expectedUnion };

            return new TestCaseData(range1, range2, expected)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_RDegenerateRpDegenerate_SEqualsSp)}");
        }

        private static TestCaseData Case_RDegenerateRpDegenerate_SpInR()
        {
            var S = _factory.CreateDirection( x: 1.0, y: 0.0);

            var Sp = _factory.CreateDirection( x: 0.0, y: 1.0);

            var range1 = _factory.CreateDirectionRange(
                S, S,
                Orientation.CounterClockwise);

            var range2 = _factory.CreateDirectionRange(
                Sp, Sp,
                Orientation.CounterClockwise);

            var expectedUnions = new List<DirectionRange<double>> { range1 };

            return new TestCaseData(range1, range2, expectedUnions)
                .SetName($"{nameof(DirectionRangeExtensionsUnionTestCaseDataSource)} - {nameof(Case_RDegenerateRpDegenerate_SpInR)}");
        }
    }
}
