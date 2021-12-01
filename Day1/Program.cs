using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

List<int> LoadDepths()
{
    return File.ReadAllLines("input.txt")
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
    for (int i = 1; i < depths.Count - 2; i++)
    {
        var window = depths[i] + depths[i + 1] + depths[i + 2];
        var prevWindow = depths[i - 1] + depths[i] + depths[i + 1];
        if (window > prevWindow)
        {
            increases++;
        }
    }

    Console.WriteLine($"Part 2: {increases}");
}

Part1();
Part2();
