using System;
using System.Collections.Generic;
using System.Linq;

namespace KineticConvolution
{
    public class ConvolvedTracing
    {

    }
    
    public class Convolution
    {
        public Convolution(IEnumerable<Tracing> shape1, IEnumerable<Tracing> shape2)
        {
            var convolutions =
                from tracing1 in shape1
                from tracing2 in shape2
                select Convolve(tracing1, tracing2);
            
            ConvolvedTracings = convolutions.SelectMany(x => x);
        }

        public IEnumerable<Tracing> Shape1 { get; }

        public IEnumerable<Tracing> Shape2 { get; }

        public IEnumerable<ConvolvedTracing> ConvolvedTracings { get; }

        private IEnumerable<ConvolvedTracing> Convolve(Tracing tracing1, Tracing tracing2) =>
            (tracing1, tracing2) switch
            {
                (Arc arc1, Arc arc2) => ConvolveArcs(arc1, arc2),
                (Arc arc, Segment segment) => ConvolveArcAndSegment(arc, segment),
                (Segment segment, Arc arc) => ConvolveArcAndSegment(arc, segment),
                (Segment _, Segment _) => Enumerable.Empty<ConvolvedTracing>(),
                _ => throw new NotSupportedException()
            };

        private static IEnumerable<ConvolvedTracing> ConvolveArcs(Arc arc1, Arc arc2)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<ConvolvedTracing> ConvolveArcAndSegment(Arc arc, Segment segment)
        {
            throw new NotImplementedException();
        }
    }
}
