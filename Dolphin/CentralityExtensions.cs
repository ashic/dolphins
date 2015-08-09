using System.Collections.Generic;
using System.Linq;

namespace Dolphin
{
    public static class CentralityExtensions
    {
        public static int[] GetEdgesByDecreasingBetweennessCentrality(this Dictionary<int, List<Path>> paths,
            Graph g)
        {
            var centralities = new double[g.Edges.Count];

            foreach (var u in paths.Keys)
            {
                var uPaths = paths[u];
                var vs = uPaths.SelectMany(x => x.NodesInPath).Distinct().ToList();

                foreach (var v in vs)
                {
                    var potentialPaths = uPaths.Where(x => x.NodesInPath.Contains(v)).Select(x => x.TruncateTo(v)).ToArray();
                    var distinctPaths = potentialPaths.Distinct(new PathEqualityComparer()).ToArray();

                    var nPaths = distinctPaths.Length;
                    var edges = distinctPaths.SelectMany(x => x.EdgeIndexes(g)).ToArray();

                    var grouped = edges.GroupBy(x => x);

                    foreach (var e in grouped)
                    {
                        var count =(double) e.Count();
                        var ratio = count/nPaths;
                        centralities[e.Key] += ratio;
                    }

                }

            }

            var sortedIndexes = centralities.Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderByDescending(x => x.Key).Select(x => x.Value).ToArray();

            return sortedIndexes;
        }
        
    }
}