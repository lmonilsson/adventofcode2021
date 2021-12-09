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
    var lowPointHeights = new List<int>();
    var rows = heights.Count;
    var cols = heights[0].Count;
    for (int y = 0; y < rows; y++)
    {
        for (int x = 0; x < cols; x++)
        {
            var h = heights[y][x];

            if ((y == 0        || heights[y - 1][x] > h) &&
                (y == rows - 1 || heights[y + 1][x] > h) &&
                (x == 0        || heights[y][x - 1] > h) &&
                (x == cols - 1 || heights[y][x + 1] > h))
            {
                lowPointHeights.Add(h);
            }
        }
    }

    var riskLevel = lowPointHeights.Sum(h => h + 1);
    Console.WriteLine($"Part 1: {riskLevel}");
}

void Part2()
{

}

Part1();
Part2();
