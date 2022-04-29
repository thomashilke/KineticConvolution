using System;
using System.Linq;

namespace Hilke.KineticConvolution.DoubleAlgebraicNumber
{
    public class DoubleConverter<TAlgebraicNumber>
    {
        public DoubleConverter(IConvolutionFactory<TAlgebraicNumber> algebraicNumberFactory)
        {
            AlgebraicNumberFactory =
                algebraicNumberFactory ?? throw new ArgumentNullException(nameof(algebraicNumberFactory));
            DoubleFactory = new ConvolutionFactory();
        }

        public IConvolutionFactory<double> DoubleFactory { get; }

        public IConvolutionFactory<TAlgebraicNumber> AlgebraicNumberFactory { get; }

        public TAlgebraicNumber FromDouble(double number) =>
            AlgebraicNumberFactory.AlgebraicNumberCalculator.CreateConstant(number);

        public double ToDouble(TAlgebraicNumber number) =>
            AlgebraicNumberFactory.AlgebraicNumberCalculator.ToDouble(number);

        public Point<TAlgebraicNumber> FromDouble(Point<double> point)
        {
            if (point is null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            return AlgebraicNumberFactory.CreatePoint(FromDouble(point.X), FromDouble(point.Y));
        }

        public Point<double> ToDouble(Point<TAlgebraicNumber> point)
        {
            if (point is null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            return DoubleFactory.CreatePoint(ToDouble(point.X), ToDouble(point.Y));
        }

        public Direction<TAlgebraicNumber> FromDouble(Direction<double> direction)
        {
            if (direction is null)
            {
                throw new ArgumentNullException(nameof(direction));
            }

            return AlgebraicNumberFactory.CreateDirection(FromDouble(direction.X), FromDouble(direction.Y));
        }

        public Direction<double> ToDouble(Direction<TAlgebraicNumber> direction)
        {
            if (direction is null)
            {
                throw new ArgumentNullException(nameof(direction));
            }

            return DoubleFactory.CreateDirection(ToDouble(direction.X), ToDouble(direction.Y));
        }

        public DirectionRange<TAlgebraicNumber> FromDouble(DirectionRange<double> range)
        {
            if (range is null)
            {
                throw new ArgumentNullException(nameof(range));
            }

            return AlgebraicNumberFactory.CreateDirectionRange(
                FromDouble(range.Start),
                FromDouble(range.End),
                range.Orientation);
        }

        public DirectionRange<double> ToDouble(DirectionRange<TAlgebraicNumber> range)
        {
            if (range is null)
            {
                throw new ArgumentNullException(nameof(range));
            }

            return DoubleFactory.CreateDirectionRange(
                ToDouble(range.Start),
                ToDouble(range.End),
                range.Orientation);
        }

        public Segment<TAlgebraicNumber> FromDouble(Segment<double> segment)
        {
            if (segment is null)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            return AlgebraicNumberFactory.CreateSegment(
                FromDouble(segment.Start),
                FromDouble(segment.End),
                segment.Weight);
        }

        public Segment<double>? ToDouble(Segment<TAlgebraicNumber> segment)
        {
            if (segment is null)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            var start = ToDouble(segment.Start);
            var end = ToDouble(segment.End);

            return start == end
                       ? null
                       : DoubleFactory.CreateSegment(
                           ToDouble(segment.Start),
                           ToDouble(segment.End),
                           segment.Weight);
        }

        public Arc<TAlgebraicNumber> FromDouble(Arc<double> arc)
        {
            if (arc is null)
            {
                throw new ArgumentNullException(nameof(arc));
            }

            return AlgebraicNumberFactory.CreateArc(
                FromDouble(arc.Center),
                FromDouble(arc.Directions),
                FromDouble(arc.Radius),
                arc.Weight);
        }

        public Arc<double> ToDouble(Arc<TAlgebraicNumber> arc)
        {
            if (arc is null)
            {
                throw new ArgumentNullException(nameof(arc));
            }

            return DoubleFactory.CreateArc(
                ToDouble(arc.Center),
                ToDouble(arc.Directions),
                ToDouble(arc.Radius),
                arc.Weight);
        }

        public Tracing<TAlgebraicNumber> FromDouble(Tracing<double> tracing)
        {
            if (tracing is null)
            {
                throw new ArgumentNullException(nameof(tracing));
            }

            return tracing switch
            {
                Arc<double> arc => FromDouble(arc),
                Segment<double> segment => FromDouble(segment),
                _ => throw new NotSupportedException(
                         $"Only segments and arcs are supported but got '{tracing.GetType()}'.")
            };
        }

        public Tracing<double>? ToDouble(Tracing<TAlgebraicNumber> tracing)
        {
            if (tracing is null)
            {
                throw new ArgumentNullException(nameof(tracing));
            }

            return tracing switch
            {
                Arc<TAlgebraicNumber> arc => ToDouble(arc),
                Segment<TAlgebraicNumber> segment => ToDouble(segment),
                _ => throw new NotSupportedException(
                         $"Only segments and arcs are supported but got '{tracing.GetType()}'.")
            };
        }

        public Shape<TAlgebraicNumber> FromDouble(Shape<double> shape)
        {
            if (shape is null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            return AlgebraicNumberFactory.CreateShape(shape.Tracings.Select(t => FromDouble(t)));
        }

        public Shape<double> ToDouble(Shape<TAlgebraicNumber> shape)
        {
            if (shape is null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            var tracings = shape.Tracings.Select(t => ToDouble(t))
                                .Where(t => !(t is null))
                                .Cast<Tracing<double>>()
                                .ToList();

            if (tracings.Count < 1)
            {
                throw new InvalidOperationException("No tracing left after conversion to double.");
            }

            return DoubleFactory.CreateShape(tracings);
        }

        public ConvolvedTracing<TAlgebraicNumber> FromDouble(ConvolvedTracing<double> convolvedTracing)
        {
            if (convolvedTracing is null)
            {
                throw new ArgumentNullException(nameof(convolvedTracing));
            }

            return new ConvolvedTracing<TAlgebraicNumber>(
                FromDouble(convolvedTracing.Convolution),
                FromDouble(convolvedTracing.Parent1),
                FromDouble(convolvedTracing.Parent2));
        }

        public ConvolvedTracing<double> ToDouble(ConvolvedTracing<TAlgebraicNumber> convolvedTracing)
        {
            if (convolvedTracing is null)
            {
                throw new ArgumentNullException(nameof(convolvedTracing));
            }

            var convolution = ToDouble(convolvedTracing.Convolution);
            var parent1 = ToDouble(convolvedTracing.Parent1);
            var parent2 = ToDouble(convolvedTracing.Parent2);

            if (convolution is null)
            {
                throw new InvalidOperationException("Convolution is null after conversion to double.");
            }

            if (parent1 is null || parent2 is null)
            {
                throw new InvalidOperationException("A parent is null after conversion to double.");
            }

            return new ConvolvedTracing<double>(convolution, parent1, parent2);
        }
    }
}
