using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

List<int> LoadDepths()
{
    return File.ReadAllText("input.txt").Trim().Split()
        .Select(line => int.Parse(line))
        .ToList();
}

void Part1()
{
    var depths = LoadDepths();
    var increases = 0;
    for (int i = 1; i < depths.Count; i++)
    {
        if (depths[i] > depths[i - 1])
        {
            increases++;
        }
    }

    Console.WriteLine($"Part 1: {increases}");
}

void Part2()
{
    var depths = LoadDepths();
    var increases = 0;
    var window = depths.Take(3).Sum();
    for (int i = 1; i < depths.Count - 2; i++)
    {
        var nextWindow = depths[i] + depths[i + 1] + depths[i + 2];
        if (nextWindow > window)
        {
            increases++;
        }
        window = nextWindow;
    }

    Console.WriteLine($"Part 2: {increases}");
}

Part1();
Part2();
