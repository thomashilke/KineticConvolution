using System;

using PeterO.Numbers;

namespace Hilke.KineticConvolution.EFloatAlgebraicNumberCalculator
{
    public class EFloatAlgebraicNumberCalculator : IAlgebraicNumberCalculator<EFloat>
    {
        private readonly EContext _context;

        public EFloatAlgebraicNumberCalculator(EContext context, EFloat zeroTolerance)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            ZeroTolerance = zeroTolerance ?? throw new ArgumentNullException(nameof(zeroTolerance));

            if (zeroTolerance.IsZero || zeroTolerance.IsNegative)
            {
                throw new ArgumentOutOfRangeException(
                    message: "Tolerance must be strictly positive.",
                    paramName: nameof(zeroTolerance));
            }
        }

        public EFloat ZeroTolerance { get; }

        public EFloat Add(EFloat left, EFloat right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            return left.Add(right, _context);
        }

        public EFloat FromInt(int value) => EFloat.FromInt32(value);

        public EFloat FromDouble(double value) => EFloat.FromDouble(value);

        public EFloat Divide(EFloat dividend, EFloat divisor)
        {
            if (dividend is null)
            {
                throw new ArgumentNullException(nameof(dividend));
            }

            if (divisor is null)
            {
                throw new ArgumentNullException(nameof(divisor));
            }

            return dividend.Divide(divisor, _context);
        }

        public EFloat Inverse(EFloat number)
        {
            if (number is null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return EFloat.One.Divide(number, _context);
        }

        public EFloat Multiply(EFloat left, EFloat right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            return left.Multiply(right, _context);
        }

        public EFloat Opposite(EFloat number)
        {
            if (number is null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return number.Negate(_context);
        }

        public int Sign(EFloat number)
        {
            if (number is null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return number.Abs().CompareTo(_tolerance) == -1 ? 0 : number.Sign;
        }

        public EFloat SquareRoot(EFloat number)
        {
            if (number is null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return number.Sqrt(_context);
        }

        public EFloat Subtract(EFloat left, EFloat right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            return left.Subtract(right, _context);
        }

        public double ToDouble(EFloat number) => number.ToDouble();
    }
}
