(List<Dot> Dots, List<Fold> Folds) LoadInput()
{
    //var input = new[]
    //{
    //    "6,10",
    //    "0,14",
    //    "9,10",
    //    "0,3",
    //    "10,4",
    //    "4,11",
    //    "6,0",
    //    "6,12",
    //    "4,1",
    //    "0,13",
    //    "10,12",
    //    "3,4",
    //    "3,0",
    //    "8,4",
    //    "1,10",
    //    "2,14",
    //    "8,10",
    //    "9,0",
    //    "",
    //    "fold along y=7",
    //    "fold along x=5"
    //};
    var input = File.ReadAllLines("input.txt");

    var dots = input
        .TakeWhile(line => !string.IsNullOrEmpty(line))
        .Select(Dot.Parse)
        .ToList();

    var folds = input
        .SkipWhile(line => !string.IsNullOrEmpty(line))
        .Skip(1)
        .Select(Fold.Parse)
        .ToList();

    return (dots, folds);
}

void Part1()
{
    var (dots, folds) = LoadInput();

    var fold = folds.First();
    dots = ExecuteFold(fold, dots);

    Console.WriteLine($"Part 1: {dots.Count}");
}

void Part2()
{
    var (dots, folds) = LoadInput();

    foreach (var fold in folds)
    {
        dots = ExecuteFold(fold, dots);
    }

    Console.WriteLine($"Part 2:");
    DisplayDots(dots);
}

void DisplayDots(List<Dot> dots)
{
    var minY = dots.Min(d => d.Y);
    var maxY = dots.Max(d => d.Y);
    var minX = dots.Min(d => d.X);
    var maxX = dots.Max(d => d.X);
    var dotsSet = dots.Select(d => (d.Y, d.X)).ToHashSet();

    for (int y = minY; y <= maxY; y++)
    {
        for (int x = minX; x <= maxX; x++)
        {
            Console.Write(dotsSet.Contains((y, x)) ? "#" : ".");
        }

        Console.WriteLine();
    }
}

List<Dot> ExecuteFold(Fold fold, List<Dot> dots)
{
    switch (fold.Dir)
    {
        case FoldDir.Up:
            return dots
                .Where(d => d.Y != fold.Where)
                .Select(d => d.Y < fold.Where
                    ? d
                    : d with { Y = fold.Where - (d.Y - fold.Where) })
                .Distinct()
                .ToList();

        case FoldDir.Left:
            return dots
                .Where(d => d.X != fold.Where)
                .Select(d => d.X < fold.Where
                    ? d
                    : d with { X = fold.Where - (d.X - fold.Where) })
                .Distinct()
                .ToList();

        default:
            throw new Exception($"Invalid FoldDir {fold.Dir}");
    }
}

Part1();
Part2();

record Dot(int X, int Y)
{ 
    public static Dot Parse(string line)
    {
        // 6,10
        var split = line.Split(',').Select(int.Parse).ToList();
        return new Dot(split[0], split[1]);
    }
}

enum FoldDir
{
    Up,
    Left
}

record Fold(FoldDir Dir, int Where)
{
    public static Fold Parse(string line)
    {
        // fold along y=7
        var split = line.Split("=");
        var dir = split[0][^1] == 'y' ? FoldDir.Up : FoldDir.Left;
        var where = int.Parse(split[1]);
        return new Fold(dir, where);
    }
}
