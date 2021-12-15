(string Template, Rule[] Rules) LoadInput()
{
    //var input = new[]
    //{
    //    "NNCB",
    //    "",
    //    "CH -> B",
    //    "HH -> N",
    //    "CB -> H",
    //    "NH -> C",
    //    "HB -> C",
    //    "HC -> B",
    //    "HN -> C",
    //    "NN -> C",
    //    "BH -> H",
    //    "NC -> B",
    //    "NB -> B",
    //    "BN -> B",
    //    "BB -> N",
    //    "BC -> B",
    //    "CC -> N",
    //    "CN -> C"
    //};
    var input = File.ReadAllLines("input.txt");

    var template = input.First();
    var rules = input.Skip(2).Select(Rule.Parse).ToArray();
    return (template, rules);
}

void Part1()
{
    var (template, rules) = LoadInput();
    var polymer = BuildPolymer(template, rules, 10);
    var elementCounts = polymer.GroupBy(e => e);
    var lowestCount = elementCounts.MinBy(g => g.LongCount())!.LongCount();
    var highestCount = elementCounts.MaxBy(g => g.LongCount())!.LongCount();
    Console.WriteLine($"Part 1: {highestCount - lowestCount}");

}

void Part2()
{
    var (template, rules) = LoadInput();
    var elementCounts = BuildPolymerElementCounts(template, rules, 40);
    var lowestCount = elementCounts.First().Count;
    var highestCount = elementCounts.Last().Count;
    Console.WriteLine($"Part 2: {highestCount - lowestCount}");
}

LinkedList<char> BuildPolymer(string template, Rule[] rules, int steps)
{
    var ruleDict = rules.ToDictionary(r => (r.E1, r.E2));
    var polymer = new LinkedList<char>(template.Select(c => c));
    for (int i = 0; i < steps; i++)
    {
        ApplyRules(polymer, ruleDict);
    }

    return polymer;
}

void ApplyRules(LinkedList<char> polymer, Dictionary<(char E1, char E2), Rule> ruleDict)
{
    for (var node = polymer.First; node != null && node.Next != null; node = node.Next)
    {
        var next = node.Next;
        if (ruleDict.TryGetValue((node.Value, next.Value), out var rule))
        {
            polymer.AddAfter(node, rule.Insert);
            node = node.Next;
        }
    }
}

List<(char Element, long Count)> BuildPolymerElementCounts(string template, Rule[] rules, int steps)
{
    var ruleDict = rules.ToDictionary(r => (r.E1, r.E2));
    var pairCounts =
        template.Zip(template.Skip(1), (e1, e2) => (E1: e1, E2: e2))
            .ToDictionary(es => (es.E1, es.E2), _ => 1L);

    for (int i = 0; i < steps; i++)
    {
        var updates = new List<(char E1, char E2, long Count)>();

        foreach (var pairCount in pairCounts)
        {
            var e1 = pairCount.Key.E1;
            var e2 = pairCount.Key.E2;

            if (ruleDict.TryGetValue((e1, e2), out var rule))
            {
                updates.Add((e1, e2, -pairCount.Value));
                updates.Add((e1, rule.Insert, pairCount.Value));
                updates.Add((rule.Insert, e2, pairCount.Value));
            }
        }

        foreach (var update in updates)
        {
            if (pairCounts.ContainsKey((update.E1, update.E2)))
            {
                pairCounts[(update.E1, update.E2)] += update.Count;
            }
            else
            {
                pairCounts[(update.E1, update.E2)] = update.Count;
            }
        }
    }

    var elementCounts = pairCounts
        .SelectMany(kvp => new[] { (Element: kvp.Key.E1, Count: kvp.Value), (Element: kvp.Key.E2, Count: kvp.Value) })
        .GroupBy(elemCount => elemCount.Element, elemCount => elemCount.Count)
        .ToDictionary(elemGroup => elemGroup.Key, elemGroup => elemGroup.Sum() / 2);

    elementCounts[template[0]]++;
    elementCounts[template[^1]]++;

    return elementCounts
        .Select(kvp => (Element: kvp.Key, Count: kvp.Value))
        .OrderBy(count => count.Count)
        .ToList();
}

Part1();
Part2();

record Rule(char E1, char E2, char Insert)
{
    internal static Rule Parse(string line)
    {
        // AB -> C
        var split = line.Split(" -> ");
        var e1 = split[0][0];
        var e2 = split[0][1];
        var insert= split[1][0];
        return new Rule(e1, e2, insert);
    }
}
