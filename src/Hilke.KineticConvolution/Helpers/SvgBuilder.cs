using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

namespace Hilke.KineticConvolution.Helpers
{
    public class SvgWriter
    {
        private const double DefaultMinimumArcRadius = 1.0e-02;
        private const double DefaultLineWidth = DefaultMinimumArcRadius;
        private const double DefaultSvgScaling = 1000.0;

        private readonly ConvolutionFactory<double> _factory;
        private readonly double _minimumArcRadius, _lineWidth, _svgScaling;
        private readonly List<(IReadOnlyList<Tracing<double>> Tracings, string Name)> _manyTracings;

        public SvgWriter(
            double minimumArcRadius = DefaultMinimumArcRadius,
            double lineWidth = DefaultLineWidth,
            double svgScaling = DefaultSvgScaling)
        {
            _factory = new(new DoubleAlgebraicNumberCalculator());
            _minimumArcRadius = minimumArcRadius;
            _lineWidth = lineWidth;
            _svgScaling = svgScaling;
            _manyTracings = new List<(IReadOnlyList<Tracing<double>> Tracings, string Name)>();
        }

        public SvgWriter Add(IEnumerable<Tracing<double>> tracings, string name = "tracing")
        {
            if (tracings is null)
            {
                throw new ArgumentNullException(nameof(tracings));
            }

            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _manyTracings.Add((PrepareForSvg(tracings), name));

            return this;
        }

        public string BuildSvgContent()
        {
            var coloredTracings =
                _manyTracings.Zip(
                                 GenerateColors(_manyTracings.Count),
                                 (namedTracings, Color) =>
                                     (namedTracings.Tracings, namedTracings.Name, Color))
                             .ToList();

            return BuildManyTracingsSvgDocument(coloredTracings);
        }

        public void Write(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Invalid filename.", nameof(filename));
            }

            File.WriteAllText(filename, BuildSvgContent());
        }
        
        private string BuildManyTracingsSvgDocument(
            IEnumerable<(IReadOnlyList<Tracing<double>> Tracings, string Name, string Color)> namedTracings)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.AppendLine(
                $"<svg id=\"map-svg\" width=\"{_svgScaling}px\" height=\"{_svgScaling}px\" viewBox=\"0 0 1 1\">");

            foreach (var (tracings, name, color) in namedTracings)
            {
                var svgPath = BuildSvgPath(tracings, buildContinuous: false, addNewLines: true);
                sb.AppendLine(
                    $"  <path id=\"{name}\" fill=\"transparent\" fill-opacity=\"0\" "
                  + $"stroke=\"{color}\" stroke-width=\"{_lineWidth}\" d =\"{Environment.NewLine}{svgPath}\"/>");
            }

            sb.AppendLine(@"</svg>");

            return sb.ToString();
        }

        private static string BuildSvgPath(IEnumerable<Tracing<double>> tracings, bool buildContinuous, bool addNewLines)
        {
            var pathSegments = new List<string>();

            var newLine = addNewLines ? Environment.NewLine : string.Empty;
            Point<double>? currentLocation = null;
            foreach (var tracing in tracings)
            {
                var locationPrefix =
                    buildContinuous && currentLocation is not null && tracing.Start.Equals(currentLocation)
                        ? string.Empty
                        : $"M {tracing.Start.X} {tracing.Start.Y}";

                var tracingPath = tracing switch
                {
                    Arc<double> arc => EmitPathSegment(arc),
                    Segment<double> segment => EmitPathSegment(segment),
                    _ => throw new NotSupportedException()
                };

                pathSegments.Add($"{locationPrefix} {tracingPath}{newLine}");
                currentLocation = tracing.End;
            }

            return string.Join(" ", pathSegments);
        }

        private static string EmitPathSegment(Arc<double> arc)
        {
            var v1x = arc.End.X - arc.Start.X;
            var v1y = arc.End.Y - arc.Start.Y;

            var v2x = arc.Center.X - arc.Start.X;
            var v2y = arc.Center.Y - arc.Start.Y;

            var v1tx = -v1y;
            var v1ty = v1x;

            var projection = v1tx * v2x + v1ty * v2y;

            var sweepFlag = arc.Directions.Orientation == Orientation.Clockwise ? 0 : 1;
            var largeArcFlag = sweepFlag == 0 && projection > 0.0 || sweepFlag != 0 && projection <= 0.0 ? 1 : 0;

            return
                $"A {arc.Radius} {arc.Radius} 0.0 {largeArcFlag} {sweepFlag} {arc.End.X} {arc.End.Y}";
        }

        private static string EmitPathSegment(Segment<double> segment) =>
            $"L {segment.End.X} {segment.End.Y}";
        

        private static readonly Dictionary<string, string> Colors = new()
        {
            { "Green", "#008000" },
            { "Blue", "#0000FF" },
            { "Purple", "#800080" },
            { "Orange", "#D45500" },
            { "Teal", "#008080" },
            { "Olive", "#808000" }
        };

        private static IEnumerable<string> GenerateColors(int count) =>
            Colors.Values.Concat(Enumerable.Repeat("#000000", Math.Max(0, count - Colors.Count))).ToArray();

        private IReadOnlyList<Tracing<double>> PrepareForSvg(IEnumerable<Tracing<double>> tracings) =>
            tracings.Select(EnsureMinimalRadiusForArc)
                    .Select(DivideCircle)
                    .SelectMany(ts => ts)
                    .ToList();

        private Tracing<double> EnsureMinimalRadiusForArc(Tracing<double> tracing) =>
            tracing switch
            {
                Arc<double> arc =>
                    arc.Radius >= _minimumArcRadius
                        ? arc
                        : _factory.CreateArc(arc.Center, arc.Directions, _minimumArcRadius, arc.Weight),
                _ => tracing
            };

        // full circle arcs are not allowed in svg path 
        private Tracing<double>[] DivideCircle(Tracing<double> tracing) =>
            tracing switch
            {
                Arc<double> arc =>
                    arc.Directions.Start != arc.Directions.End
                        ? new[] { arc }
                        : new[]
                        {
                            _factory.CreateArc(
                                arc.Center,
                                _factory.CreateDirectionRange(
                                    arc.Directions.Start,
                                    arc.Directions.Start.Opposite(),
                                    arc.Directions.Orientation),
                                arc.Radius,
                                arc.Weight),
                            _factory.CreateArc(
                                arc.Center,
                                _factory.CreateDirectionRange(
                                    arc.Directions.Start.Opposite(),
                                    arc.Directions.Start,
                                    arc.Directions.Orientation),
                                arc.Radius,
                                arc.Weight)
                        },
                _ => new[] { tracing }
            };
    }
}
