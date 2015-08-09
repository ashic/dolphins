namespace Dolphin
{
    public class Edge
    {
        public int Source { get; private set; }
        public int Target { get; private set; }

        public Edge(int source, int target)
        {
            Source = source;
            Target = target;
        }
    }
}