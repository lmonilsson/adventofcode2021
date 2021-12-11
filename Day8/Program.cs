Entry ParseEntry(string line)
{
    var split = line.Split(" | ");
    var signals = split[0].Split(' ');
    var outputs = split[1].Split(' ');
    return new Entry(signals, outputs);
}

List<Entry> LoadInput()
{
    //var input = new[]
    //{
    //    "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf"
    //};
    //var input = new string[]
    //{
    //    "be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe",
    //    "edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc",
    //    "fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg",
    //    "fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb",
    //    "aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea",
    //    "fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb",
    //    "dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe",
    //    "bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef",
    //    "egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb",
    //    "gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce",
    //};
    var input = File.ReadAllLines("input.txt");
    return input.Select(ParseEntry).ToList();
}

void Part1()
{
    var entries = LoadInput();
    var numEasyOutputDigits = entries
        .SelectMany(e => e.Outputs)
        .Count(s => s.Length == 2 || s.Length == 4 || s.Length == 3 || s.Length == 7);

    Console.WriteLine($"Part 1: {numEasyOutputDigits}");
}

void Part2()
{
    var digits = new Dictionary<string, int>()
    {
        { "abcefg", 0 },
        { "cf", 1 },
        { "acdeg", 2 },
        { "acdfg", 3 },
        { "bcdf", 4 },
        { "abdfg", 5 },
        { "abdefg", 6 },
        { "acf", 7 },
        { "abcdefg", 8 },
        { "abcdfg", 9 }
    };

    var entries = LoadInput();

    long outputSum = 0;
    foreach (var entry in entries)
    {
        var signals = entry.Signals;
        var map = new Dictionary<char, char>();

        var d1 = signals.First(x => x.Length == 2);
        var d7 = signals.First(x => x.Length == 3);
        map['a'] = d7.Except(d1).Single();

        var d4 = signals.Single(x => x.Length == 4);
        var d3 = signals.Single(x => x.Length == 5 && x.Intersect(d1).Count() == 2 && x.Intersect(d4).Count() == 3);
        map['b'] = d4.Except(d1).Except(d3).Single();
        map['d'] = d3.Intersect(d4).Except(d1).Single();

        var d9 = signals.Single(x => x.Length == 6 && x.Intersect(d1).Count() == 2 && x.Contains(map['a']) && x.Contains(map['b']) && x.Contains(map['d']));
        map['g'] = d9.Except(d1).Where(c => c != map['a'] && c != map['b'] && c != map['d']).Single();

        var d6 = signals.Single(x => x.Length == 6 && x.Intersect(d1).Count() == 1);
        map['e'] = d6.Except(d1).Where(c => c != map['a'] && c != map['b'] && c != map['d'] && c != map['g']).Single();
        map['f'] = d6.Intersect(d1).Single();
        map['c'] = d1.Except(d6).Single();

        long output = 0;
        foreach (var outp in entry.Outputs)
        {
            var decodedOutp = new string(outp.Select(c => map.First(kvp => kvp.Value == c).Key).OrderBy(c => c).ToArray());
            output *= 10;
            output += digits[decodedOutp];
        }

        outputSum += output;
    }

    Console.WriteLine($"Part 2: {outputSum}");
}

Part1();
Part2();

class Entry
{
    public IReadOnlyList<string> Signals { get; }
    public IReadOnlyList<string> Outputs { get; }

    public Entry(IReadOnlyList<string> signals, IReadOnlyList<string> outputs)
    {
        Signals = signals.Select(x => new string(x.OrderBy(c => c).ToArray())).ToList();
        Outputs = outputs.Select(x => new string(x.OrderBy(c => c).ToArray())).ToList();
    }
}