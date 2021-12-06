List<int> LoadInput()
{
    //return "3,4,3,1,2".Split(',').Select(int.Parse).ToList();
    return File.ReadAllText("input.txt").Split(',').Select(int.Parse).ToList();
}

void Part1()
{
    var timers = LoadInput();

    for (int d = 0; d < 80; d++)
    {
        var timerCount = timers.Count;
        for (int t = 0; t < timerCount; t++)
        {
            timers[t]--;
            if (timers[t] < 0)
            {
                timers.Add(8);
                timers[t] = 6;
            }
        }
    }

    Console.WriteLine($"Part 1: {timers.Count}");
}

void Part2()
{
    var timers = LoadInput();
    var generations = Enumerable.Range(0, 7).Select(n => (long) timers.Count(t => t == n)).ToList();
    var newGenerations = new List<long> { 0, 0 };

    for (int d = 0; d < 256; d++)
    {
        var grown = newGenerations[0];
        newGenerations[0] = newGenerations[1];
        newGenerations[1] = generations[d % 7];
        generations[d % 7] += grown;
    }

    Console.WriteLine($"Part 2: {generations.Sum() + newGenerations.Sum()}");
}

Part1();
Part2();
