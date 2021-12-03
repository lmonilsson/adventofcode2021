using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

IReadOnlyList<string> LoadInput()
{
    return File.ReadAllLines("input.txt");
}

void Part1()
{
    var input = LoadInput();

    var gamma = MeasureGamme(input);
    var epsilon = MeasureEpsilon(input);
    var power = gamma * epsilon;

    Console.WriteLine($"Part 1: {power}");
}

int MeasureGamme(IReadOnlyList<string> input)
{
    var half = input.Count / 2;
    var numBits = input[0].Length;
    var gamma = 0;
    for (var c = 0; c < numBits; c++)
    {
        var ones = CountsOnes(input, c);
        if (ones > half)
        {
            gamma |= (1 << (numBits - c - 1));
        }
    }

    return gamma;
}

int MeasureEpsilon(IReadOnlyList<string> input)
{
    var half = input.Count / 2;
    var numBits = input[0].Length;
    var epsilon = 0;
    for (var c = 0; c < numBits; c++)
    {
        var ones = CountsOnes(input, c);
        if (ones < half)
        {
            epsilon |= (1 << (numBits - c - 1));
        }
    }

    return epsilon;
}

void Part2()
{
    var input = LoadInput();

    var oxygen = MeasureOxygen(input);
    var scrubber = MeasureScrubber(input);
    var lifeSupport = oxygen * scrubber;

    Console.WriteLine($"Part 2: {lifeSupport}");
}

int MeasureOxygen(IReadOnlyList<string> input)
{
    var numBits = input[0].Length;
    var inputLeft = input;

    for (var c = 0; c < numBits && inputLeft.Count > 1; c++)
    {
        var ones = CountsOnes(inputLeft, c);
        var zeroes = inputLeft.Count - ones;
        var keep = ones >= zeroes ? '1' : '0';
        inputLeft = inputLeft.Where(line => line[c] == keep).ToList();
    }

    return ParseBinary(inputLeft.First());
}

int MeasureScrubber(IReadOnlyList<string> input)
{
    var numBits = input[0].Length;
    var inputLeft = input;

    for (var c = 0; c < numBits && inputLeft.Count > 1; c++)
    {
        var ones = CountsOnes(inputLeft, c);
        var zeroes = inputLeft.Count - ones;
        var keep = ones < zeroes ? '1' : '0';
        inputLeft = inputLeft.Where(line => line[c] == keep).ToList();
    }

    return ParseBinary(inputLeft.First());
}

int CountsOnes(IReadOnlyList<string> input, int c)
{
    return input.Count(line => line[c] == '1');
}

int ParseBinary(string line)
{
    var numBits = line.Length;
    var num = 0;
    for (var c = 0; c < numBits; c++)
    {
        if (line[c] == '1')
        {
            num |= (1 << (numBits - c - 1));
        }
    }

    return num;
}

Part1();
Part2();
