var OpeningTokens = new List<char> { '(', '[', '{', '<' };
var ClosingTokens = new List<char> { ')', ']', '}', '>' };

List<string> LoadInput()
{
    //var input = new[]
    //{
    //    "[({(<(())[]>[[{[]{<()<>>",
    //    "[(()[<>])]({[<{<<[]>>(",
    //    "{([(<{}[<>[]}>{[]{[(<()>",
    //    "(((({<>}<{<{<>}{[]{[]{}",
    //    "[[<[([]))<([[{}[[()]]]",
    //    "[{[{({}]{}}([{[{{{}}([]",
    //    "{<[[]]>}<{[{[{[]{()[[[]",
    //    "[<(<(<(<{}))><([]([]()",
    //    "<{([([[(<>()){}]>(<<{{",
    //    "<{([{{}}[<[[[<>{}]]]>[]]"
    //};
    var input = File.ReadAllLines("input.txt");
    return input.ToList();
}

void Part1()
{
    var entries = LoadInput();

    var score = 0;
    foreach (var entry in entries)
    {
        var (illegal, _) = ParseEntry(entry);
        score += illegal switch
        {
            ')' => 3,
            ']' => 57,
            '}' => 1197,
            '>' => 25137,
            _ => 0
        };
    }

    Console.WriteLine($"Part 1: {score}");
}

void Part2()
{
    var entries = LoadInput();

    var scores = new List<long>();
    foreach (var entry in entries)
    {
        var (_, completion) = ParseEntry(entry);
        if (completion.Any())
        {
            scores.Add(GetCompletionScore(completion));
        }
    }

    scores = scores.OrderBy(x => x).ToList();
    var middle = scores[scores.Count / 2];
    Console.WriteLine($"Part 2: {middle}");
}

(char? IllegalCharacter, List<char> Completion) ParseEntry(string entry)
{
    char? illegal = null;
    var completion = new List<char>();
    var openings = new Stack<char>();

    foreach (var c in entry)
    {
        var closingTokenIdx = ClosingTokens.IndexOf(c);
        if (closingTokenIdx < 0)
        {
            openings.Push(c);
        }
        else if (openings.Any() && openings.Peek() == OpeningTokens[closingTokenIdx])
        {
            openings.Pop();
        }
        else
        {
            illegal = c;
            break;
        }
    }

    if (illegal == null)
    {
        while (openings.Any())
        {
            var openingTokenIdx = OpeningTokens.IndexOf(openings.Pop());
            completion.Add(ClosingTokens[openingTokenIdx]);
        }
    }

    return (illegal, completion);
}

long GetCompletionScore(List<char> completion)
{
    long score = 0;
    foreach (var c in completion)
    {
        score *= 5;
        score += c switch
        {
            ')' => 1,
            ']' => 2,
            '}' => 3,
            '>' => 4,
            _ => 0
        };
    }

    return score;
}

Part1();
Part2();
