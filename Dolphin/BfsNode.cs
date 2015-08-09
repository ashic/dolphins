using System.Collections.Generic;
using System.Linq;

namespace Dolphin
{

    public class Path
    {
        private readonly int[] _path;

        public int[] NodesInPath { get { return _path; } }

        private Path(int[] path)
        {
            _path = path;
        }

        public static Path New()
        {
            return new Path(new int[0]);
        }

        public Path Add(int node)
        {
            return new Path(new List<int>(_path) {node}.ToArray());
        }

        public Path TruncateTo(int v)
        {
            var pre = _path.TakeWhile(x => x != v).ToList();
            pre.Add(v);
            return new Path(pre.ToArray());
        }

        public int LastNode {get { return _path.Last(); } }


        public bool SameAs(Path a)
        {
            if (a._path.Length == _path.Length == false) return false;

            for (int i=0; i < _path.Length; i++)
                if (_path[i] != a._path[i]) return false;

            return true;
        }

        public int[] EdgeIndexes(Graph g)
        {
            var results = new List<int>();
            for (var i = 0; i < _path.Length - 1; i++)
            {
               var index = g.Edges.FindIndex(x => (x.Source == _path[i] && x.Target == _path[i + 1])
                                       || x.Source == _path[i + 1] && x.Target == _path[i]);
                results.Add(index);
            }

            return results.ToArray();
        }

    }

    public class PathEqualityComparer : IEqualityComparer<Path>
    {
        public bool Equals(Path x, Path y)
        {
            return x.SameAs(y);
        }

        public int GetHashCode(Path obj)
        {
            var baseHash = 0x4785;
            var hashCodes = obj.NodesInPath.Select(x => x.GetHashCode());

            foreach (var h in hashCodes)
            {
                baseHash ^= h;
            }

            return baseHash;
        }
    }

    public class AdjacencyGraph
    {
        private readonly Graph _g;

        public AdjacencyGraph(Graph g)
        {
            _g = g;
        }

        public Dictionary<int, List<Path>> GetAllPaths()
        {
            var results = new Dictionary<int, List<Path>>();
            var bfsNodes = initialize(_g);

            foreach (var node in _g.Nodes.Values)
            {
                results[node.Id] = getPathsForNode(node.Id, bfsNodes);
            }

            return results;
        } 

        private List<Path> getPathsForNode(int node, IReadOnlyDictionary<int, BfsNode> nodes)
        {
            var paths = new List<Path> {Path.New().Add(node)};
            var visited = new SortedSet<int> {node};

            while (true)
            {
                var added = false;

                var newPaths = new List<Path>();
                foreach (var p in paths)
                {
                    var pLast = p.LastNode;
                    var pLastNode = nodes[pLast];
                    var newNodes = pLastNode.AdjacentNodes.Where(x => visited.Contains(x) == false).ToArray();
                    var pathsForThis = newNodes.Select(x => p.Add(x));
                    newPaths.AddRange(pathsForThis);
                    if (newNodes.Any()) added = true;
                    else newPaths.Add(p);
                }

                foreach (var p in newPaths)
                {
                    visited.Add(p.LastNode);
                }

                paths = newPaths;

                if (!added) break;
            }

            return paths;
        } 

        private Dictionary<int, BfsNode> initialize(Graph g)
        {
            var d = new Dictionary<int, BfsNode>();
            foreach (var n in g.Nodes.Values)
            {
                var node = new BfsNode(n.Id);
                foreach (var e in g.Edges)
                    node.RegisterEdge(e);
                d.Add(node.Id, node);
            }

            return d;
        }
    }

    public class BfsNode
    {
        public int Id { get; private set; }
        public SortedSet<int> AdjacentNodes { get; private set; }
        public Colour Colour = Colour.White;
        public int Distance = 10000;
        public BfsNode Parent = null;

        public BfsNode(int id)
        {
            Id = id;
            AdjacentNodes = new SortedSet<int>();
        }

        public BfsNode RegisterEdge(Edge e)
        {
            if (e.Source == Id)
            {
                AdjacentNodes.Add(e.Target);
            }
            else if (e.Target == Id)
            {
                AdjacentNodes.Add(e.Source);
            }

            return this;
        }
    }
}