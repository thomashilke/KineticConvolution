# Kinetic Convolution of Polygonal Tracings
This repository offer an implementation of the concept of kinetic
convolution of polygonal tracings, as introduced in [Gui83] and
extended in [Mil07].

# Usage
Add the NuGet reference to the project:
```PowerShell
    dotnet add package Hilke.KineticConvolution --version 0.1.0
```

and import the namespace in your source file:
```C#
    using Hilke.KineticConvolution;
```

Given `shape1` and `shape2` of type `Shape<T>` which represent two
polygonal tracings, the kinetic convolution of those two tracings is
obtained by calling
```C#
    var factory = new ConvolutionFactory();
    var convolution = factory.Convolve(shape1, shape2);
```

The result of the kinetic convolution of the two shape is a collection
of `ConvolutionTracings`, each of which holds references to both
parent tracings, as well as the tracing that results from the
convolution:
```C#
    foreach (var convolvedTracing in Convolution.ConvolvedTracings)
    {
        var parent1 = convolvedTracing.Parent1;
        var parent2 = convolvedTracing.Parent2;
        var tracing = convolvedTracing.Convolution;
    }
```
Here, `tracing` is the polygonal segment that arose from the
convolution of `parent1` from `shape1` and `parent2` from `shape2`.

# References
[Gui83]: L. Guibas, L. Ramshaw and J. Stolfi, ["A kinetic framework for computational geometry,"](https://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=4568066&isnumber=4568049) 24th Annual Symposium on Foundations of Computer Science (sfcs 1983), Tucson, AZ, USA, 1983, pp. 100-111, doi: 10.1109/SFCS.1983.1.

[Mil07]: Milenkovic, Victor, and Elisha Sacks. ["A monotonic convolution for Minkowski sums."](https://www.worldscientific.com/doi/abs/10.1142/S0218195907002392) International Journal of Computational Geometry & Applications 17.04 (2007): 383-396.
