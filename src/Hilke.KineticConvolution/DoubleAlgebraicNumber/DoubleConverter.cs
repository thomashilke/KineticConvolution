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

        public DirectionRange<double>? TryToDouble(DirectionRange<TAlgebraicNumber> range)
        {
            if (range is null)
            {
                throw new ArgumentNullException(nameof(range));
            }

            var start = ToDouble(range.Start);
            var end = ToDouble(range.End);
            
            return range.Start != range.End && start == end
                       ? resolveLimitCase()
                       : DoubleFactory.CreateDirectionRange(start, end, range.Orientation);

            DirectionRange<double>? resolveLimitCase()
            {
                var referenceDirection = range.Start.Opposite();
                if (range.Start.CompareTo(range.End, referenceDirection) == DirectionOrder.Before)
                {
                    return range.Orientation == Orientation.CounterClockwise
                           ? null
                           : DoubleFactory.CreateDirectionRange(start, end, range.Orientation);
                }
                else
                {
                    return range.Orientation == Orientation.CounterClockwise
                               ? DoubleFactory.CreateDirectionRange(start, end, range.Orientation)
                               : null;
                }
            }
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

        public Segment<double>? TryToDouble(Segment<TAlgebraicNumber> segment)
        {
            if (segment is null)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            var start = ToDouble(segment.Start);
            var end = ToDouble(segment.End);

            return start == end
                       ? null
                       : DoubleFactory.CreateSegment(start, end, segment.Weight);
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

        public Arc<double>? TryToDouble(Arc<TAlgebraicNumber> arc)
        {
            if (arc is null)
            {
                throw new ArgumentNullException(nameof(arc));
            }

            var directions = TryToDouble(arc.Directions);

            return directions is null
                       ? null
                       : DoubleFactory.CreateArc(
                           ToDouble(arc.Center),
                           directions,
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

        public Tracing<double>? TryToDouble(Tracing<TAlgebraicNumber> tracing)
        {
            if (tracing is null)
            {
                throw new ArgumentNullException(nameof(tracing));
            }

            return tracing switch
            {
                Arc<TAlgebraicNumber> arc => TryToDouble(arc),
                Segment<TAlgebraicNumber> segment => TryToDouble(segment),
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

        public Shape<double>? ToDouble(Shape<TAlgebraicNumber> shape)
        {
            if (shape is null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            var tracings = shape.Tracings.Select(t => TryToDouble(t))
                                .Where(t => !(t is null))
                                .Cast<Tracing<double>>()
                                .ToList();

            return tracings.Count < 1 ? null : DoubleFactory.CreateShape(tracings);
        }
    }
}
