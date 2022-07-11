using Hilke.KineticConvolution;
using Hilke.KineticConvolution.DoubleAlgebraicNumber;

namespace Hilke.KinematicConvolution.ReadMeExamples
{
    public static class WorkingExample
    {
        public static void Run()
        {
            var calculator = new DoubleAlgebraicNumberCalculator();
            var factory = new ConvolutionFactory<double>(calculator);

            var disk = CreateDiskShape(factory);

            var pathShape = CreatePathShape(factory);

            var minkowskiSum = factory.ConvolveShapes(disk, pathShape);

            foreach (var convolvedTracing in minkowskiSum.ConvolvedTracings)
            {
                switch (convolvedTracing.Convolution)
                {
                    case Arc<double> arc:
                        Console.WriteLine(
      $"Boundary arc: center ({arc.Center.X}, {arc.Center.Y}), radius {arc.Radius}, " +
      $"start ({arc.Start.X}, {arc.Start.Y}), end ({arc.End.X}, {arc.End.Y})");
                        break;

                    case Segment<double> segment:
                        Console.WriteLine(
$"Boundary segment: start ({segment.Start.X}, {segment.Start.Y}), end ({segment.End.X}, {segment.End.Y})");
                        break;

                    default:
                        throw new InvalidOperationException("Unexpected tracing encountered.");
                }
            }
        }

        private static Shape<double> CreateDiskShape(ConvolutionFactory<double> factory)
        {
            var eastDirection = factory.CreateDirection(1.0, 0.0);

            var diskArc = factory.CreateArc(
                center: factory.CreatePoint(0.0, 0.0),
                directions: factory.CreateDirectionRange(eastDirection, eastDirection, Orientation.CounterClockwise),
                radius: 1.0,
                weight: 1);

            return factory.CreateShape(new[] { diskArc });
        }

        private static Shape<double> CreatePathShape(ConvolutionFactory<double> factory)
        {
            var pathSegment = factory.CreateSegment(
                startX: 0.0, startY: 0.0,
                endX: 1.0, endY: 2.0,
                weight: 1);

            var pathReverseSegment = factory.CreateSegment(
                startX: 1.0, startY: 2.0,
                endX: 0.0, endY: 0.0,
                weight: 1);

            var smoothingArc1 = factory.CreateArc(
                pathSegment.Start,
                factory.CreateDirectionRange(
                    pathReverseSegment.EndTangentDirection.NormalDirection().Opposite(),
                    pathSegment.StartTangentDirection.NormalDirection().Opposite(),
                    Orientation.CounterClockwise),
                0.0,
                1);

            var smoothingArc2 = factory.CreateArc(
                pathReverseSegment.Start,
                factory.CreateDirectionRange(
                    pathSegment.StartTangentDirection.NormalDirection().Opposite(),
                    pathReverseSegment.EndTangentDirection.NormalDirection().Opposite(),
                    Orientation.CounterClockwise),
                0.0,
                1);

            return factory.CreateShape(new Tracing<double>[] { smoothingArc1, pathSegment, smoothingArc2, pathReverseSegment });
        }
    }
}
