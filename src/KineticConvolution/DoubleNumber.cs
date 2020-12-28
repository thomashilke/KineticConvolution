using System;

namespace KineticConvolution
{
    public sealed class DoubleNumber : IAlgebraicNumber
    {
        private readonly double _value;

        public double Value => _value;

        public IAlgebraicNumber Add(IAlgebraicNumber operand)
        {
            if (operand is DoubleNumber n)
            {
                return new DoubleNumber(n._value + _value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public IAlgebraicNumber Substract(IAlgebraicNumber operand)
        {
            if (operand is DoubleNumber n)
            {
                return new DoubleNumber(_value - n._value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public IAlgebraicNumber MultipliedBy(IAlgebraicNumber operand)
        {
            if (operand is DoubleNumber n)
            {
                return new DoubleNumber(n._value * _value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public IAlgebraicNumber DividedBy(IAlgebraicNumber operand)
        {
            if (operand is DoubleNumber n)
            {
                return new DoubleNumber(_value / n._value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public IAlgebraicNumber Inverse()
        {
            if (_value != 0.0)
            {
                return new DoubleNumber(1.0 / _value);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public IAlgebraicNumber SquareRoot()
        {
            if (_value >= 0.0)
            {
                return new DoubleNumber(Math.Sqrt(_value));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public IAlgebraicNumber Opposite()
        {
            return new DoubleNumber(-_value);
        }

        public int Sign()
        {
            return Math.Sign(_value);
        }

        public static DoubleNumber FromDouble(double value)
        {
            return new DoubleNumber(value);
        }

        private DoubleNumber(double value)
        {
            _value = value;
        }
    }
}
