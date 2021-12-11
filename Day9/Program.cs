List<List<int>> LoadInput()
{
    //var input = new[]
    //{
    //    "2199943210",
    //    "3987894921",
    //    "9856789892",
    //    "8767896789",
    //    "9899965678"
    //};
    var input = File.ReadAllLines("input.txt");
    return input
        .Select(line => line.Select(c => (int) char.GetNumericValue(c)).ToList()).ToList()
        .ToList();
}

void Part1()
{
    var heights = LoadInput();
    var riskLevel = FindLowPoints(heights).Sum(lp => heights[lp.Y][lp.X] + 1);
    Console.WriteLine($"Part 1: {riskLevel}");
}

void Part2()
{
    var heights = LoadInput();
    var rows = heights.Count;
    var cols = heights[0].Count;
    var visited = new HashSet<(int Y, int X)>();
    var largestBasinSizes = new List<int> { 0, 0, 0 };

    foreach (var lp in FindLowPoints(heights))
    {
        var h = heights[lp.Y][lp.X];
        visited.Add((lp.Y, lp.X));

        if (h < 9)
        {
            var basin = new HashSet<(int Y, int X)>();
            var basinSize = 1;
            basin.Add((lp.Y, lp.X));

            while (basin.Any())
            {
                var (y, x) = basin.First();
                basin.Remove((y, x));
                visited.Add((y, x));
                h = heights[y][x];

                if (y > 0 && !visited.Contains((y - 1, x)) && heights[y - 1][x] > h && heights[y - 1][x] < 9)
                {
                    if (basin.Add((y - 1, x))) basinSize++;
                }
                if (y < rows - 1 && !visited.Contains((y + 1, x)) && heights[y + 1][x] > h && heights[y + 1][x] < 9)
                {
                    if (basin.Add((y + 1, x))) basinSize++;
                }
                if (x > 0 && !visited.Contains((y, x - 1)) && heights[y][x - 1] > h && heights[y][x - 1] < 9)
                {
                    if (basin.Add((y, x - 1))) basinSize++;
                }
                if (x < cols - 1 && !visited.Contains((y, x + 1)) && heights[y][x + 1] > h && heights[y][x + 1] < 9)
                {
                    if (basin.Add((y, x + 1))) basinSize++;
                }
            }

            var replaceIdx = largestBasinSizes.FindIndex(x => x < basinSize);
            if (replaceIdx >= 0)
            {
                largestBasinSizes[replaceIdx] = basinSize;
                largestBasinSizes.Sort();
            }
        }
    }

    var result = largestBasinSizes.Aggregate(1, (a, b) => a * b);
    Console.WriteLine($"Part 2: {result}");
}

IEnumerable<(int Y, int X)> FindLowPoints(List<List<int>> heights)
{
    var rows = heights.Count;
    var cols = heights[0].Count;

    for (int y = 0; y < rows; y++)
    {
        for (int x = 0; x < cols; x++)
        {
            var h = heights[y][x];

            if ((y == 0 || heights[y - 1][x] > h) &&
                (y == rows - 1 || heights[y + 1][x] > h) &&
                (x == 0 || heights[y][x - 1] > h) &&
                (x == cols - 1 || heights[y][x + 1] > h))
            {
                yield return (y, x);
            }
        }
    }
}

Part1();
Part2();
