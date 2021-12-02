using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

List<(string Dir, int Units)> LoadCommands()
{
    return File.ReadAllLines("input.txt")
        .Select(line => line.Split(" "))
        .Select(split => (Dir: split[0], Units: int.Parse(split[1])))
        .ToList();
}

void Part1()
{
    var commands = LoadCommands();

    var pos = 0;
    var depth = 0;
    foreach (var cmd in commands)
    {
        if (cmd.Dir == "forward")
        {
            pos += cmd.Units;
        }
        else if(cmd.Dir == "down")
        {
            depth += cmd.Units;
        }
        else
        {
            depth -= cmd.Units;
        }
    }

    Console.WriteLine($"Part 1: {pos * depth}");
}

void Part2()
{
    var commands = LoadCommands();

    var pos = 0;
    var depth = 0;
    var aim = 0;
    foreach (var cmd in commands)
    {
        if (cmd.Dir == "forward")
        {
            pos += cmd.Units;
            depth += cmd.Units * aim;
        }
        else if (cmd.Dir == "down")
        {
            aim += cmd.Units;
        }
        else
        {
            aim -= cmd.Units;
        }
    }

    Console.WriteLine($"Part 2: {pos * depth}");
}

Part1();
Part2();
