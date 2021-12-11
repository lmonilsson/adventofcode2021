List<List<int>> LoadInput()
{
    //var input = new[]
    //{
    //    "5483143223",
    //    "2745854711",
    //    "5264556173",
    //    "6141336146",
    //    "6357385478",
    //    "4167524645",
    //    "2176841721",
    //    "6882881134",
    //    "4846848554",
    //    "5283751526"
    //};
    var input = File.ReadAllLines("input.txt");
    return input
        .Select(line => line.Select(c => int.Parse(c.ToString())).ToList())
        .ToList();
}

void Part1()
{
    var octopuses = LoadInput();
    var totalFlashes = 0;
    for (int i = 0; i < 100; i++)
    {
        totalFlashes += Step(octopuses);
    }

    Console.WriteLine($"Part 1: {totalFlashes}");
}

void Part2()
{
    var octopuses = LoadInput();
    var numOctopuses = octopuses.Count * octopuses[0].Count;
    var step = 1;
    while (Step(octopuses) != numOctopuses)
    {
        step++;
    }

    Console.WriteLine($"Part 2: {step}");
}

int Step(List<List<int>> octopuses)
{
    var rows = octopuses.Count();
    var cols = octopuses[0].Count();
    var pendingFlashes = new HashSet<(int, int)>();
    var doneFlashes = new HashSet<(int, int)>();

    for (int y = 0; y < rows; y++)
    {
        for (int x = 0; x < cols; x++)
        {
            octopuses[y][x]++;
            if (octopuses[y][x] > 9)
            {
                pendingFlashes.Add((y, x));
            }
        }
    }

    while (pendingFlashes.Any())
    {
        var (y, x) = pendingFlashes.First();
        pendingFlashes.Remove((y, x));
        doneFlashes.Add((y, x));

        foreach (var (y2, x2) in Neighbours(y, x, rows, cols))
        {
            octopuses[y2][x2]++;

            if (octopuses[y2][x2] > 9 && !pendingFlashes.Contains((y2, x2)) && !doneFlashes.Contains((y2, x2)))
            {
                pendingFlashes.Add((y2, x2));
            }
        }
    }

    for (int y = 0; y < rows; y++)
    {
        for (int x = 0; x < cols; x++)
        {
            if (octopuses[y][x] > 9)
            {
                octopuses[y][x] = 0;
            }
        }
    }

    return doneFlashes.Count;
}

IEnumerable<(int Y, int X)> Neighbours(int y, int x, int rows, int cols)
{
    if (y > 0 && x > 0)
    {
        yield return (y - 1, x - 1);
    }
    if (y > 0)
    {
        yield return (y - 1, x);
    }
    if (y > 0 && x < cols - 1)
    {
        yield return (y - 1, x + 1);
    }

    if (x > 0)
    {
        yield return (y, x - 1);
    }
    if (x < cols - 1)
    {
        yield return (y, x + 1);
    }

    if (y < rows - 1 && x > 0)
    {
        yield return (y + 1, x - 1);
    }
    if (y < rows - 1)
    {
        yield return (y + 1, x);
    }
    if (y < rows - 1 && x < cols - 1)
    {
        yield return (y + 1, x + 1);
    }
}

Part1();
Part2();
