using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dolphin
{
    public class Graph
    {
        public readonly Dictionary<int, Node> Nodes; 
        public readonly List<Edge> Edges;

        public static Graph ParseGml(string fileName)
        {
            var graph = new Graph(new Dictionary<int, Node>(), new List<Edge>());
            using (var file = new FileStream(fileName, FileMode.Open))
            {
                using (var sr = new StreamReader(file))
                {
                    while (sr.EndOfStream == false)
                    {
                        var line = sr.ReadLine().Trim();
                        switch (line)
                        {
                            case "node":
                                sr.ReadLine();
                                var id = int.Parse(sr.ReadLine().Trim().Split(' ')[1]);
                                var label = sr.ReadLine().Trim().Split(' ')[1].Replace("\"", "");
                                graph.Nodes[id] = new Node(id, label);
                                sr.ReadLine();
                                break;
                            case "edge":
                                sr.ReadLine();
                                var source = int.Parse(sr.ReadLine().Trim().Split(' ')[1]);
                                var target = int.Parse(sr.ReadLine().Trim().Split(' ')[1]);
                                sr.ReadLine();
                                graph.Edges.Add(new Edge(source, target));
                                break;
                        }
                    }
                }
            }

            return graph;
        }

        private Graph(Dictionary<int, Node> nodes, List<Edge> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public Graph RemoveEdges(int[] edgeIndexes)
        {
            var newEdges = new List<Edge>();
            for (int i = 0; i < Edges.Count; i++)
            {
                if(edgeIndexes.Contains(i) == false)
                    newEdges.Add(Edges[i]);
            }

            var graph = new Graph(Nodes, newEdges);

            return graph;
        }
    }
}