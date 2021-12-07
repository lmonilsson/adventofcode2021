List<int> LoadInput()
{
    //var input = "16,1,2,0,4,2,7,1,2,14";
    var input = File.ReadAllText("input.txt");
    return input.Split(',').Select(int.Parse).ToList();
}

void Part1()
{
    var positions = LoadInput();
    var minPos = positions.Min();
    var maxPos = positions.Max();

    var bestTotalDist = int.MaxValue;
    for (var i = minPos; i <= maxPos; i++)
    {
        var totalDist = positions.Select(p => Math.Abs(p - i)).Sum();
        if (totalDist < bestTotalDist)
        {
            bestTotalDist = totalDist;
        }
    }

    Console.WriteLine($"Part 1: {bestTotalDist}");
}

void Part2()
{
    var positions = LoadInput();
    var minPos = positions.Min();
    var maxPos = positions.Max();

    var bestTotalDist = int.MaxValue;
    for (var i = minPos; i <= maxPos; i++)
    {
        var totalDist = positions.Select(p => CalculateFuelWithIncreasingCost(p, i)).Sum();
        if (totalDist < bestTotalDist)
        {
            bestTotalDist = totalDist;
        }
    }

    Console.WriteLine($"Part 2: {bestTotalDist}");
}

int CalculateFuelWithIncreasingCost(int position, int target)
{
    var from = Math.Min(position, target);
    var to = Math.Max(position, target);
    var cost = 1;
    var total = 0;
    for (var i = from; i < to; i++)
    {
        total += cost;
        cost++;
    }

    return total;
}

Part1();
Part2();
