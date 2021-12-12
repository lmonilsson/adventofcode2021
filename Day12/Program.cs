List<Node> LoadInput()
{
    //var input = new[]
    //{
    //    "dc-end",
    //    "HN-start",
    //    "start-kj",
    //    "dc-start",
    //    "dc-HN",
    //    "LN-dc",
    //    "HN-end",
    //    "kj-sa",
    //    "kj-HN",
    //    "kj-dc"
    //};
    var input = File.ReadAllLines("input.txt");

    var nodes = new List<Node>();
    foreach (var line in input)
    {
        var split = line.Split('-');
        
        var node1 = nodes.FirstOrDefault(n => n.Name == split[0]);
        if (node1 == null)
        {
            node1 = new Node(split[0]);
            nodes.Add(node1);
        }

        var node2 = nodes.FirstOrDefault(n => n.Name == split[1]);
        if (node2 == null)
        {
            node2 = new Node(split[1]);
            nodes.Add(node2);
        }

        if (node2.Name != "start")
        {
            node1.Edges.Add(node2);
        }

        if (node1.Name != "start")
        {
            node2.Edges.Add(node1);
        }
    }

    return nodes;
}

void Part1()
{
    var nodes = LoadInput();
    var start = nodes.First(n => n.Name == "start");
    var paths = Traverse(start, new List<Node>(), false);
    Console.WriteLine($"Part 1: {paths}");
}

void Part2()
{
    var nodes = LoadInput();
    var start = nodes.First(n => n.Name == "start");
    var paths = Traverse(start, new List<Node>(), true);
    Console.WriteLine($"Part 2: {paths}");
}

int Traverse(Node node, List<Node> visitedSmallCaves, bool allowTwoVisitsToSingleSmallCave)
{
    if (node.Name == "end")
    {
        return 1;
    }

    var isSecondVisitToSmallCave = node.IsSmallCave && visitedSmallCaves.Contains(node);

    if (node.IsSmallCave)
    {
        if (isSecondVisitToSmallCave)
        {
            if (!allowTwoVisitsToSingleSmallCave)
            {
                return 0;
            }

            allowTwoVisitsToSingleSmallCave = false;
        }
        else
        {
            visitedSmallCaves.Add(node);
        }
    }

    var pathsFromHere = node.Edges.Sum(e => Traverse(e, visitedSmallCaves, allowTwoVisitsToSingleSmallCave));

    if (node.IsSmallCave && !isSecondVisitToSmallCave)
    {
        visitedSmallCaves.Remove(node);
    }

    return pathsFromHere;
}

Part1();
Part2();

class Node
{
    public string Name { get; }
    public List<Node> Edges { get; } = new List<Node>();
    public bool IsSmallCave { get; }

    public Node(string name)
    {
        Name = name;
        IsSmallCave = name == name.ToLowerInvariant();
    }
}

