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
    var totalRisk = lowestRiskPath.SkipLast(1).Sum(r => risks[r.Y][r.X]);
    Console.WriteLine($"Part 1: {totalRisk}");
}

void Part2()
{
    var initialRisks = LoadInput();
    var risks = new List<List<int>>(initialRisks.Count * 5);
    for (int r = 0; r < initialRisks.Count * 5; r++)
    {
        var initialRow = initialRisks[r % initialRisks.Count];
        var row = new List<int>(initialRow.Count * 5);
        risks.Add(row);

        for (int c = 0; c < initialRow.Count * 5; c++)
        {
            var newRisk = initialRow[c % initialRow.Count];
            for (var b = 0; b < r / initialRisks.Count + c / initialRow.Count; b++)
            {
                newRisk++;
                if (newRisk == 10) newRisk = 1;
            }

            row.Add(newRisk);
        }
    }

    var lowestRisk = FuckingDijkstraIt(risks);
    Console.WriteLine($"Part 2: {lowestRisk}");
}

List<(int X, int Y)> FindLowestRiskPath(List<List<int>> risks)
{
    var lowestTotalRiskPerCoords = new Dictionary<(int X, int Y), int>();
    var path = Traverse(risks, 0, 0, lowestTotalRiskPerCoords, -risks[0][0], null);
    if (path == null)
    {
        throw new Exception("Solution not found");
    }
    return path;
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

int FuckingDijkstraIt(List<List<int>> risks)
{
    var queue = new PriorityQueue<(int X, int Y, int TotalRisk), int>();
    var visited = new HashSet<(int X, int Y)>();

    queue.Enqueue((0, 0, 0), 0);

    while (queue.Count > 0)
    {
        var (x, y, totalRisk) = queue.Dequeue();
        if (visited.Contains((x, y)))
        {
            continue;
        }

        if (x == risks[0].Count - 1 && y == risks.Count - 1)
        {
            return totalRisk;
        }

        visited.Add((x, y));

        foreach (var (nextX, nextY) in new[]
        {
            (x + 1, y    ),
            (x    , y + 1),
            (x - 1, y    ),
            (x    , y - 1)
        })
        {
            if (nextX >= 0 && nextX < risks[0].Count &&
                nextY >= 0 && nextY < risks.Count &&
                !visited.Contains((nextX, nextY)))
            {
                var nextRisk = totalRisk + risks[nextY][nextX];
                queue.Enqueue((nextX, nextY, nextRisk), nextRisk);
            }
        }
    }

    throw new Exception("Solution not found");
}

Part1();
Part2();
