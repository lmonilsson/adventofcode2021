Entry ParseEntry(string line)
{
    var split = line.Split(" | ");
    var signals = split[0].Split(' ');
    var outputs = split[1].Split(' ');
    return new Entry(signals, outputs);
}

List<Entry> LoadInput()
{
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
    var entries = LoadInput();
}

Part1();
Part2();

class Entry
{
    public IReadOnlyList<string> Signals { get; }
    public IReadOnlyList<string> Outputs { get; }

    public Entry(IReadOnlyList<string> signals, IReadOnlyList<string> outputs)
    {
        Signals = signals;
        Outputs = outputs;
    }
}