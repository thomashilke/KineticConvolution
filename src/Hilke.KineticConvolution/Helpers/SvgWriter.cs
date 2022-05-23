using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

namespace Hilke.KineticConvolution.Tests.Helpers
{
    public class SvgWriter
    {
        private const double DefaultMinimumArcRadius = 1.0e-02;
        private const double DefaultLineWidth = DefaultMinimumArcRadius;
        private const double DefaultSvgScaling = 1000.0;

        private readonly ConvolutionFactory<double> _factory;
        private readonly double _minimumArcRadius, _lineWidth, _svgScaling;

        public SvgWriter(
            double minimumArcRadius = DefaultMinimumArcRadius,
            double lineWidth = DefaultLineWidth,
            double svgScaling = DefaultSvgScaling)
        {
            _factory = new(new DoubleAlgebraicNumberCalculator());
            _minimumArcRadius = minimumArcRadius;
            _lineWidth = lineWidth;
            _svgScaling = svgScaling;
        }

        public void ExportTracingsTo(IEnumerable<Tracing<double>> tracings, string filename) =>
            ExportManyTracingsTo(
                new (IReadOnlyList<Tracing<double>>, string)[] { (tracings.ToList(), "a_tracing") },
                filename);

        public void ExportManyTracingsTo(
            IEnumerable<IReadOnlyList<Tracing<double>>> manyTracings,
            string filename) =>
            ExportManyTracingsTo(manyTracings.Select((tracings, index) => (tracings, $"tracing {index}")), filename);

        public void ExportManyTracingsTo(
            IEnumerable<(IReadOnlyList<Tracing<double>> Tracings, string Name)> manyNamedTracings,
            string filename)
        {
            var enumerated = manyNamedTracings.ToList();
            var coloredTracings =
                enumerated.Zip(
                              GenerateColors(enumerated.Count),
                              (namedTracings, Color) =>
                                  (Tracings: PrepareForSvg(namedTracings.Tracings),
                                   namedTracings.Name, Color))
                          .ToList();

            File.WriteAllText(filename, BuildManyTracingsSvgDocument(coloredTracings));
        }

        public static void ExportSumTo(
            Shape<double> shapeA,
            Shape<double> shapeB,
            IEnumerable<Tracing<double>> convolvedTracings,
            string filename)
        {
            var pathA = BuildSvgPath(shapeA.Tracings, buildContinuous: true, addNewLines: false);
            var pathB = BuildSvgPath(shapeB.Tracings, buildContinuous: true, addNewLines: false);
            var pathSum = BuildSvgPath(convolvedTracings, buildContinuous: true, addNewLines: false);

            File.WriteAllText($"{withoutExtension(filename)}.html", BuildHtmlDocument(pathA, pathB, pathSum));
            File.WriteAllText($"{withoutExtension(filename)}.svg", BuildSvgDocument(pathA, pathB, pathSum));

            static string withoutExtension(string fn) =>
                $"{Path.GetDirectoryName(fn)}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(fn)}";
        }

        private string BuildManyTracingsSvgDocument(
            IEnumerable<(IReadOnlyList<Tracing<double>> Tracings, string Name, string Color)> namedTracings)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone =\"yes\"?>");
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

        private static string BuildHtmlDocument(string pathA, string pathB, string pathSum)
        {
            return @"<!DOCTYPE html>
<html>
  <head>
    <style>
      svg { display: block;  position: absolute; }

      #wheel {
          background-color: red;
          offset-path: path('"
                 + pathSum
                 + @"');
          offset-distance: 0%;
          offset-rotate: 180deg;
          animation: red-ball 20s linear normal infinite;
      }

      @keyframes red-ball {
          from { offset-distance: 0%; }
          to { offset-distance: 100%; }
      }
    </style>
  </head>
  <body>
    <svg id=""map-svg"" width=""200px"" height=""200px"" viewBox=""-5 -5 10 10"">
      <g id = ""matrix-group"" transform=""matrix(1.2 0 0 -1.2 0 0)"">
        <path fill = ""transparent"" stroke=""#bbbbbb"" stroke-width=""0.1"" d="""
                 + pathSum
                 + @" z"" id=""minkowski-sum""></path>
        <path fill = ""#55555555"" stroke=""#555555"" stroke-width=""0.1"" d="""
                 + pathA
                 + @" z"" id=""profile""></path>
        <path fill = ""#55aa0055"" stroke=""#555555"" stroke-width=""0.1"" d="""
                 + pathB
                 + @" z"" id=""wheel""></path>
      </g>
    </svg>
  </body>
</html>
";
        }

        private static string BuildSvgDocument(string pathA, string pathB, string pathSum)
        {
            return @"<?xml version=""1.0"" encoding=""UTF-8"" standalone =""yes""?>
<svg id=""map-svg"" width=""800px"" height=""800px"" viewBox=""-100 -100 200 200"">
    <path fill-opacity=""0.0"" stroke=""#800080"" stroke-width=""0.02"" d="""
                 + pathSum
                 + @" z"" id=""minkowski-sum""></path>
    <path fill-opacity=""0.0"" stroke=""#0000FF"" stroke-width=""0.02"" d="""
                 + pathA
                 + @" z"" id=""profile""></path>
    <path fill-opacity=""0.0"" stroke=""#FF0000"" stroke-width=""0.02"" d="""
                 + pathB
                 + @" z"" id=""wheel""></path>
</svg>
";
        }

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
            tracings.Select(t => EnsureMinimalRadiusForArc(t))
                    .Select(t => DivideCircle(t))
                    .SelectMany(ts => ts)
                    .ToList();

        // zero radius arcs do not work (the whole path is ignored)
        private Tracing<double> EnsureMinimalRadiusForArc(Tracing<double> tracing) =>
            tracing switch
            {
                Arc<double> arc =>
                    arc.Radius >= _minimumArcRadius
                        ? arc
                        : _factory.CreateArc(arc.Center, arc.Directions, _minimumArcRadius, arc.Weight),
                _ => tracing
            };

        // full circle arcs do not work (path is ignored)
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
