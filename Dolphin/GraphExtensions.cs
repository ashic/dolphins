using System;
using System.Linq;
using System.Text;

namespace Dolphin
{
    public static class GraphExtensions
    {
        public static string AsD3JsJson(this Graph graph, double[] sizes)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("  \"nodes\":[");
            var lastNode = graph.Nodes.Values.Last();

            var nodes = graph.Nodes.Values.ToArray();
            for (var i=0; i<nodes.Length; i++)
            {
                sb.AppendFormat("    {{\"name\":\"{0}\", \"group\":{1}, \"size\":{2}}}{3}{4}", nodes[i].Name, 
                    getGroup(sizes[i]),
                    sizes[i], nodes[i] == lastNode?"":",", Environment.NewLine);
            }

            sb.AppendLine("  ],");
            sb.AppendLine("  \"links\":[");
            var lastEdge = graph.Edges.Last();
            foreach (var edge in graph.Edges)
            {
                sb.AppendFormat("    {{\"source\":{0}, \"target\":{1}, \"value\":1}}{2}{3}",
                    edge.Source, edge.Target, edge == lastEdge ? "" : ",", Environment.NewLine);
            }
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static int getGroup(double d)
        {
            if (d < 5) return 0;
            if (d < 6) return 1;
            if (d < 7) return 2;
            if (d < 8) return 3;
            if (d < 9) return 4;
            return 5;
        }

        public static double[] NodeSizes(this Graph graph)
        {
            var degrees = graph.Nodes.Keys.Select(x => graph.Edges.Count(y => y.Source == x || y.Target == x)).ToArray();
            var minDegrees = degrees.Min();
            var maxDegrees = degrees.Max();
            var steps = 5.0D;
            var stepSize = (maxDegrees - minDegrees) / steps;

            var sizes = degrees.Select(x => (x - minDegrees) / stepSize).ToArray();
            var rs = sizes.Select(x => 4.0D + x).ToArray();

            return rs;
        }
    }
}