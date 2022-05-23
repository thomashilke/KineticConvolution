using System;
using System.ComponentModel;
using System.Linq;

namespace Hilke.KineticConvolution.DoubleAlgebraicNumber
{
    public class DoubleConverter<TAlgebraicNumber>
    {
        public DoubleConverter(IConvolutionFactory<TAlgebraicNumber> algebraicNumberConvolutionFactory, InvalidityManagementMode mode)
        {
            AlgebraicNumberFactory =
                algebraicNumberConvolutionFactory ?? throw new ArgumentNullException(nameof(algebraicNumberConvolutionFactory));
            DoubleFactory = new ConvolutionFactory();

            if (!Enum.IsDefined(typeof(InvalidityManagementMode), mode))
            {
                throw new InvalidEnumArgumentException(nameof(mode), (int)mode, typeof(InvalidityManagementMode));
            }

            Mode = mode;
        }

        public IConvolutionFactory<double> DoubleFactory { get; }

        public IConvolutionFactory<TAlgebraicNumber> AlgebraicNumberFactory { get; }

        public InvalidityManagementMode Mode { get; }

        public TAlgebraicNumber FromDouble(double number) =>
            AlgebraicNumberFactory.AlgebraicNumberCalculator.CreateConstant(number);

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
            
            return range.Start != range.End && start == end
                       ? resolveLimitCase()
                       : DoubleFactory.CreateDirectionRange(start, end, range.Orientation);

            DirectionRange<double>? resolveLimitCase()
            {
                var referenceDirection = range.Start.Opposite();

                var result = range.Start.CompareTo(range.End, referenceDirection) == DirectionOrder.Before
                                 ? range.Orientation == Orientation.CounterClockwise
                                       ? null
                                       : DoubleFactory.CreateDirectionRange(start, end, range.Orientation)
                                 : range.Orientation == Orientation.CounterClockwise
                                     ? DoubleFactory.CreateDirectionRange(start, end, range.Orientation)
                                     : null;

                return result
                    ?? Mode switch
                       {
                           InvalidityManagementMode.Silent => null,
                           InvalidityManagementMode.ThrowException =>
                               throw new InvalidOperationException(
                                   "The range collapsed to a single direction during conversion to double."),
                           _ => throw new NotSupportedException(
                                    $"Only Silent and ThrowException mode are supported but got {Mode.GetType()}.")
                       };
            }
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

            return start == end
                       ? Mode switch
                       {
                           InvalidityManagementMode.Silent => null,
                           InvalidityManagementMode.ThrowException =>
                               throw new InvalidOperationException(
                                   "The segment collapsed to a single point during conversion to double."),
                           _ => throw new NotSupportedException(
                                    $"Only Silent and ThrowException mode are supported but got {Mode.GetType()}.")
                       }
                       : DoubleFactory.CreateSegment(start, end, segment.Weight);
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
                         $"Only segments and arcs are supported but got '{tracing.GetType()}'.")
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
                         $"Only segments and arcs are supported but got '{tracing.GetType()}'.")
            };
        }

        public virtual Shape<TAlgebraicNumber> FromDouble(Shape<double> shape)
        {
            if (shape is null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            return AlgebraicNumberFactory.CreateShape(shape.Tracings.Select(t => FromDouble(t)));
        }

        public virtual Shape<double>? ToDouble(Shape<TAlgebraicNumber> shape)
        {
            if (shape is null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            var tracings = shape.Tracings.Select(t => ToDouble(t))
                                .Where(t => !(t is null))
                                .Cast<Tracing<double>>()
                                .ToList();

            return tracings.Count < 1 ? null : DoubleFactory.CreateShape(tracings);
        }
    }
}
