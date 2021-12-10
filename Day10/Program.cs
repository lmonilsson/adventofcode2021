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
        var illegal = TryFindIllegalCharacter(entry);
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

char? TryFindIllegalCharacter(string entry)
{
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
            return c;
        }
    }

    return null;
}

Part1();
