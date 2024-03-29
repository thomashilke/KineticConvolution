# Kinetic Convolution of Polygonal Tracings
This repository offers an implementation of the concept of kinetic
convolution of polygonal tracings, as introduced in [[Gui83]](#references) and
extended in [[Mil07]](#references). A complete working example is
provided for the impatient in section [Working Example](#working-example).

![kinetic convolution of two polygonal tracings](/images/minkowski-sum.png)

# Usage
Add the NuGet reference to the project:
```PowerShell
    dotnet add package Hilke.KineticConvolution --version 0.1.0
```

and import the namespace in your source file:
```C#
    using Fractions;
    using Hilke.KineticConvolution;
```

You are then required to provide an implementation of the interface
`IAlgebraicNumberCalculator<TAlgebraicNumber>` according to your
requirements to model the algebraic, or constructible, numbers. These
numbers form the field whose elements will be used to describe all
geometric entities, such as `Direction`, `Arc` and `Segment`.

If you want to use the `double` data type as an (approximate)
implementation of algebraic numbers,  import
```C#
    using Hilke.KineticConvolution.DoubleAlgebraicNumber;
```
and create an instance of
`DoubleAlgebraicNumber.DoubleAlgebraicNumberCalculator` that can then
be used to create geometric entities:
```C#
    var calculator = new DoubleAlgebraicNumberCalculator();
    var factory = new ConvolutionFactory<double>(calculator);

    var segment = factory.CreateSegment(
        startX: 0.0, startY: 0.0,
        endX: 1.0, endY: 2.0,
        weight: new Fraction(2, 3));
```

Given `shape1` and `shape2` of type `Shape<T>` which represent two
tracings and `factory` of type
`IConvolutionFactory<double>`, the kinetic convolution of those two
tracings is obtained by calling
```C#
    var convolution = factory.ConvolveShapes(shape1, shape2);
```
Note that if `shape1` and `shape2` represent the boundary of two
convex domains, then the kinetic convolution of `shape1` and `shape2`
is exactly the boundary of the Minkowski sum of the two domains.

The result of the kinetic convolution of `shape1` and `shape2` is a
collection of `ConvolutionTracings`, each of which holds references to
both parent tracings, one from `shape1` and one from `shape2`, as well
as the tracing that results from the convolution:
```C#
    foreach (var convolvedTracing in convolution.ConvolvedTracings)
    {
        var parent1 = convolvedTracing.Parent1;
        var parent2 = convolvedTracing.Parent2;
        var tracing = convolvedTracing.Convolution;
    }
```
Here, `tracing` is the polygonal segment that arose from the
convolution of `parent1` from `shape1` and `parent2` from `shape2`.

Instance of `Shape<T>`, as well as every other objects must be
instantiated through a factory instance of
`IConvolutionFactory<TAlgebraicNumber>`. More about the reasons for
this design is given in section [Factory and algebraic numbers](#factory-and-algebraic-numbers)
below.

# Factory and algebraic numbers
A fundamental initial requirement of this implementation was the
ability to leave the choice of the underlying number type to the
clients of the library. Users are free to choose to represent the
point and direction coodinates, arc radii, intersection location
coordinates, etc, with the type of their choice, by providing the
proper type parameter for `TAlgebraicNumber`, and implementing the
interface `IAlgebraicNumberCalculator`.

Unfortunately, the C# language does not allow overloading algebraic
operators (or any operator, for that matter) on interfaces, and does not
allow arithmetic operations involving generic types. The architecture
that combines a generic type parameter `TAlgebraicNumber` along with
an object instance of type `IAlgebraicNumberCalculator` which
abstracts away the  manipulation of instances of `TAlgebraicNumber`
is a workaround to this limitation of the language.

In this case, `IAlgebraicNumberCalculator` models the concept of
[constructible numbers](https://en.wikipedia.org/wiki/Constructible_number) which is
a subset of [algebraic
numbers](https://en.wikipedia.org/wiki/Constructible_number). Hence an
instance of `TAlgebraicNumber` is an object that can be summed,
subtracted, multiplied, divided, taken the square root of, and whose sign
can be inspected.

As a convenience, an implementation of approximate algebraic number
based on the floating type `double` is provided by
`DoubleAlgebaicNumber.DoubleAlgebraicNumberCalculator`. However, it is
only an approximation of algebraic numbers, as it is a well known fact
than `double` numbers cannot represent every constructible number. As
a consequence, there isn't any guarantee of robustness for any
geometrical predicate with this implementation.

Most of the types defined in this project depend on the generic type
parameter `TAlgebraicNumber` and need to manipulate instances of
`TAlgebraicNumber`. To alleviate the need to pass an instance of the
calculator to the constructor of all these objects, a calculator is
instantiated in the factory, and object instances are created through
the factory's methods `CreatePoint`, `CreateSegment`, etc.

# Working example
The following example demonstrates the library usage to compute the
convolution of a unit disk with a straight line. The two shapes being
convex, the result is the boundary of the Minkowski sum of the two
shapes. The following code can be found ready to run in the project
`samples/Hilke.KineticConvolution.ReadMeExamples/`.

```C#
using Hilke.KineticConvolution;
using Hilke.KineticConvolution.DoubleAlgebraicNumber;

namespace Hilke.KinematicConvolution.ReadMeExamples
{
    class WorkingExample
    {
        static void Run()
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
                    case Arc<double> arc: Console.WriteLine(
                        $"Boundary arc: center ({arc.Center.X}, {arc.Center.Y}), radius {arc.Radius}, " +
                        $"start ({arc.Start.X}, {arc.Start.Y}), end ({arc.End.X}, {arc.End.Y})");
                        break;

                    case Segment<double> segment: Console.WriteLine(
                        $"Boundary segment: start ({segment.Start.X}, {segment.Start.Y}), end ({segment.End.X}, {segment.End.Y})");
                        break;

                    default:
                        throw new InvalidOperationException("Unexpected tracing encountered.");
                }
            }
        }

        static Shape<double> CreateDiskShape(ConvolutionFactory<double> factory)
        {
            var eastDirection = factory.CreateDirection(1.0, 0.0);

            var diskArc = factory.CreateArc(
                center: factory.CreatePoint(0.0, 0.0),
                directions: factory.CreateDirectionRange(eastDirection, eastDirection, Orientation.CounterClockwise),
                radius: 1.0,
                weight: 1);

            return factory.CreateShape(new[] { diskArc });
        }

        static Shape<double> CreatePathShape(ConvolutionFactory<double> factory)
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
```

# References
[Gui83]: L. Guibas, L. Ramshaw and J. Stolfi, ["A kinetic framework for computational geometry,"](https://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=4568066&isnumber=4568049) 24th Annual Symposium on Foundations of Computer Science (sfcs 1983), Tucson, AZ, USA, 1983, pp. 100-111, doi: 10.1109/SFCS.1983.1.

[Mil07]: Milenkovic, Victor, and Elisha Sacks. ["A monotonic convolution for Minkowski sums."](https://www.worldscientific.com/doi/abs/10.1142/S0218195907002392) International Journal of Computational Geometry & Applications 17.04 (2007): 383-396.
