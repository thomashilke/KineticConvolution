using System;
using System.ComponentModel;
using System.Linq;

namespace Hilke.KineticConvolution.DoubleAlgebraicNumber
{
    public class DoubleConverter<TAlgebraicNumber>
    {
        public DoubleConverter(
            IConvolutionFactory<TAlgebraicNumber> algebraicNumberConvolutionFactory,
            InvalidConversionPolicy policy = InvalidConversionPolicy.Throw)
        {
            AlgebraicNumberFactory =
                algebraicNumberConvolutionFactory
             ?? throw new ArgumentNullException(nameof(algebraicNumberConvolutionFactory));
            DoubleFactory = new ConvolutionFactory<double>(new DoubleAlgebraicNumberCalculator());

            if (!Enum.IsDefined(typeof(InvalidConversionPolicy), policy))
            {
                throw new InvalidEnumArgumentException(nameof(policy), (int)policy, typeof(InvalidConversionPolicy));
            }

            Policy = policy;
        }

        public IConvolutionFactory<double> DoubleFactory { get; }

        public IConvolutionFactory<TAlgebraicNumber> AlgebraicNumberFactory { get; }

        public InvalidConversionPolicy Policy { get; }

        public TAlgebraicNumber FromDouble(double number) =>
            AlgebraicNumberFactory.AlgebraicNumberCalculator.FromDouble(number);

        public double ToDouble(TAlgebraicNumber number) =>
            AlgebraicNumberFactory.AlgebraicNumberCalculator.ToDouble(number);

        public virtual Point<TAlgebraicNumber> FromDouble(Point<double> point)
        {
            if (point is null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            return AlgebraicNumberFactory.CreatePoint(FromDouble(point.X), FromDouble(point.Y));
        }

        public virtual Point<double> ToDouble(Point<TAlgebraicNumber> point)
        {
            if (point is null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            return DoubleFactory.CreatePoint(ToDouble(point.X), ToDouble(point.Y));
        }

        public virtual Direction<TAlgebraicNumber> FromDouble(Direction<double> direction)
        {
            if (direction is null)
            {
                throw new ArgumentNullException(nameof(direction));
            }

            return AlgebraicNumberFactory.CreateDirection(FromDouble(direction.X), FromDouble(direction.Y));
        }

        public virtual Direction<double> ToDouble(Direction<TAlgebraicNumber> direction)
        {
            if (direction is null)
            {
                throw new ArgumentNullException(nameof(direction));
            }

            return DoubleFactory.CreateDirection(ToDouble(direction.X), ToDouble(direction.Y));
        }

        public virtual DirectionRange<TAlgebraicNumber> FromDouble(DirectionRange<double> range)
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

        public virtual DirectionRange<double>? ToDouble(DirectionRange<TAlgebraicNumber> range)
        {
            if (range is null)
            {
                throw new ArgumentNullException(nameof(range));
            }

            var start = ToDouble(range.Start);
            var end = ToDouble(range.End);

            if (start != end || range.Start == range.End)
            {
                return DoubleFactory.CreateDirectionRange(start, end, range.Orientation);
            }

            var isAlmostOmniRange =
                range.Orientation == Orientation.CounterClockwise
                    ? range.Start.CompareTo(range.End, range.Start.Opposite()) == DirectionOrder.After
                    : range.Start.CompareTo(range.End, range.Start.Opposite()) == DirectionOrder.Before;

            return isAlmostOmniRange
                       ? DoubleFactory.CreateDirectionRange(start, end, range.Orientation)
                       : Policy switch
                       {
                           InvalidConversionPolicy.Throw =>
                               throw new InvalidOperationException(
                                   "The range collapsed to a single direction during conversion to double."),
                           InvalidConversionPolicy.IgnoreSilently => null,
                           _ => throw new NotSupportedException(
                                    $"Only Silent and ThrowException modes are supported but got {Policy}.")
                       };
        }

        public virtual Segment<TAlgebraicNumber> FromDouble(Segment<double> segment)
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

        public virtual Segment<double>? ToDouble(Segment<TAlgebraicNumber> segment)
        {
            if (segment is null)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            var start = ToDouble(segment.Start);
            var end = ToDouble(segment.End);

            if (start != end)
            {
                return DoubleFactory.CreateSegment(start, end, segment.Weight);
            }

            return Policy switch
            {
                InvalidConversionPolicy.Throw =>
                    throw new InvalidOperationException(
                        "The segment collapsed to a single point during conversion to double."),
                InvalidConversionPolicy.IgnoreSilently => null,
                _ => throw new NotSupportedException(
                         $"Only Silent and ThrowException modes are supported but got {Policy}.")
            };
        }

        public virtual Arc<TAlgebraicNumber> FromDouble(Arc<double> arc)
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

        public virtual Arc<double>? ToDouble(Arc<TAlgebraicNumber> arc)
        {
            if (arc is null)
            {
                throw new ArgumentNullException(nameof(arc));
            }

            var directions = ToDouble(arc.Directions);

            return directions is null
                       ? null
                       : DoubleFactory.CreateArc(
                           ToDouble(arc.Center),
                           directions,
                           ToDouble(arc.Radius),
                           arc.Weight);
        }

        public virtual Tracing<TAlgebraicNumber> FromDouble(Tracing<double> tracing)
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
                         $"Only segments and arcs are supported but got '{tracing}'.")
            };
        }

        public virtual Tracing<double>? ToDouble(Tracing<TAlgebraicNumber> tracing)
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
                         $"Only segments and arcs are supported but got '{tracing}'.")
            };
        }

        public virtual Shape<TAlgebraicNumber> FromDouble(Shape<double> shape)
        {
            if (shape is null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            return AlgebraicNumberFactory.CreateShape(shape.Tracings.Select(FromDouble));
        }

        public virtual Shape<double>? ToDouble(Shape<TAlgebraicNumber> shape)
        {
            if (shape is null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            var tracings = shape.Tracings.Select(ToDouble)
                                .Where(t => !(t is null))
                                .Cast<Tracing<double>>()
                                .ToList();

            return tracings.Count < 1 ? null : DoubleFactory.CreateShape(tracings);
        }
    }
}
