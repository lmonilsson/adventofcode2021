List<List<int>> LoadInput()
{
    //var input = new[]
    //{
    //    "1163751742",
    //    "1381373672",
    //    "2136511328",
    //    "3694931569",
    //    "7463417111",
    //    "1319128137",
    //    "1359912421",
    //    "3125421639",
    //    "1293138521",
    //    "2311944581"
    //};
    var input = File.ReadAllLines("input.txt");

    return input.Select(line => line.Select(c => int.Parse(c.ToString())).ToList()).ToList();
}

void Part1()
{
    var risks = LoadInput();
    var lowestRiskPath = FindLowestRiskPath(risks);
    var totalRisk = lowestRiskPath!.SkipLast(1).Sum(r => risks[r.Y][r.X]);
    Console.WriteLine($"Part 1: {totalRisk}");
}

List<(int X, int Y)>? FindLowestRiskPath(List<List<int>> risks)
{
    var lowestTotalRiskPerCoords = new Dictionary<(int X, int Y), int>();
    return Traverse(risks, 0, 0, lowestTotalRiskPerCoords, -risks[0][0], null);
}

List<(int X, int Y)>? Traverse(
    List<List<int>> risks,
    int x, int y,
    Dictionary<(int X, int Y), int> lowestTotalRiskPerCoords,
    int totalRisk,
    int? bestRisk)
{
    totalRisk += risks[y][x];

    if (x == risks[0].Count - 1 && y == risks.Count - 1)
    {
        if (bestRisk == null || totalRisk < bestRisk.Value)
        {
            return new List<(int X, int Y)> { (x, y) };
        }
        else
        {
            return null;
        }
    }

    if (lowestTotalRiskPerCoords.TryGetValue((x, y), out var shortest) && shortest < totalRisk)
    {
        return null;
    }

    lowestTotalRiskPerCoords[(x, y)] = totalRisk;

    List<(int X, int Y)>? nextBestPath = null;

    foreach (var (nextX, nextY) in new[]
    {
        (x + 1, y    ),
        (x    , y + 1),
        (x - 1, y    ),
        (x    , y - 1)
    })
    {
        if (nextX >= 0 && nextX < risks[0].Count
            && nextY >= 0 && nextY < risks.Count
            && (bestRisk == null || totalRisk + risks[nextY][nextX] < bestRisk.Value))
        {
            var nextPath = Traverse(risks, nextX, nextY, lowestTotalRiskPerCoords, totalRisk, bestRisk);
            if (nextPath != null)
            {
                nextBestPath = nextPath;
                bestRisk = totalRisk + nextBestPath.Sum(r => risks[r.Y][r.X]);
            }
        }
    }

    if (nextBestPath != null)
    {
        nextBestPath.Add((x, y));
    }

    return nextBestPath;
}

Part1();
