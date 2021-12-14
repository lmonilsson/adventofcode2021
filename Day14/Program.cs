(string Template, Rule[] Rules) LoadInput()
{
    var input = new[]
    {
        "NNCB",
        "",
        "CH -> B",
        "HH -> N",
        "CB -> H",
        "NH -> C",
        "HB -> C",
        "HC -> B",
        "HN -> C",
        "NN -> C",
        "BH -> H",
        "NC -> B",
        "NB -> B",
        "BN -> B",
        "BB -> N",
        "BC -> B",
        "CC -> N",
        "CN -> C"
    };
    //var input = File.ReadAllLines("input.txt");

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
    //var polymer = BuildPolymer(template, rules, 40);
    //var elementCounts = polymer.GroupBy(e => e);
    //var lowestCount = elementCounts.MinBy(g => g.LongCount())!.LongCount();
    //var highestCount = elementCounts.MaxBy(g => g.LongCount())!.LongCount();
    //Console.WriteLine($"Part 2: {highestCount - lowestCount}");

    /* Ideas:
     *
     * - Does it repeat? Can one stop after it has repeated, and do some multiplication to get the result?
     *   No, it does not seem to repeat (tried 20 iterations).
     *
     * - It it possible to see which rules will be most and least common, and throw away everything that doesn't lead there?
     *   No, this wouldn't help, since the number of occurences of the two elements is still too large.
     *
     * - Simple maths to get the number of expansion of the specific elements?
     */
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
