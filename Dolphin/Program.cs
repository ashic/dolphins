using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;

namespace Dolphin
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = Graph.ParseGml("dolphins.gml");
            var originalGraph = graph;

            for (var i = 0; i < 50; i++)
            {
                var adjGraph = new AdjacencyGraph(graph);
                var paths = adjGraph.GetAllPaths();
                var centralities = paths.GetEdgesByDecreasingBetweennessCentrality(graph);
                var toDrop = centralities.Take(1).ToArray();
                graph = graph.RemoveEdges(toDrop);
            }


            var asJson = graph.AsD3JsJson(originalGraph.NodeSizes());
            File.WriteAllText("../../../Visualization/Scripts/dolphins_pruned.json", asJson);

            var originalAsJson = originalGraph.AsD3JsJson(originalGraph.NodeSizes());
            File.WriteAllText("../../../Visualization/Scripts/dolphins.json", originalAsJson);
        }
    }
}
