namespace Dolphin
{
    public class Node
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Node(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public enum Colour
    {
        White, Gray, Black
    }
}