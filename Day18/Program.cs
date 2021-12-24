IReadOnlyList<string> LoadInput()
{
    //var input = new []
    //{
    //    "[[[[4,3],4],4],[7,[[8,4],9]]]",
    //    "[1,1]"
    //};
    //var input = new[]
    //{
    //    "[[[0,[4, 5]],[0, 0]],[[[4,5],[2,6]],[9,5]]]",
    //    "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]",
    //    "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]",
    //    "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]",
    //    "[7,[5,[[3,8],[1,4]]]]",
    //    "[[2,[2,2]],[8,[8,1]]]",
    //    "[2,9]",
    //    "[1,[[[9,3],9],[[9,0],[0,7]]]]",
    //    "[[[5,[7,4]],7],1]",
    //    "[[[[4,2],2],6],[8,7]]"
    //};
    //var input = new[]
    //{
    //    "[[[0,[5, 8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
    //    "[[[5,[2,8]],4],[5,[[9,9],0]]]",
    //    "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
    //    "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
    //    "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
    //    "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
    //    "[[[[5,4],[7,7]],8],[[8,3],8]]",
    //    "[[9,3],[[9,9],[6,[4,9]]]]",
    //    "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
    //    "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]"
    //};
    var input = File.ReadAllLines("input.txt");
    return input;
}

void Part1()
{
    var input = LoadInput();
    var elements = input.Select(line => Element.Parse(line)).ToList();
    var element = elements.First();
    foreach (var line in elements.Skip(1))
    {
        element = element.Add(line);
    }

    Console.WriteLine($"Part 1: {CalculateMagnitude(element)}");
}

void Part2()
{
    var input = LoadInput();
    var elements = input.Select(line => Element.Parse(line)).ToList();

    var maxMagnitude = long.MinValue;
    for (int i = 0; i < elements.Count - 1; i++)
    {
        for (int j = i + 1; j < elements.Count; j++)
        {
            var mag = CalculateMagnitude(elements[i].Add(elements[j]));
            if (mag > maxMagnitude)
            {
                maxMagnitude = mag;
            }

            mag = CalculateMagnitude(elements[j].Add(elements[i]));
            if (mag > maxMagnitude)
            {
                maxMagnitude = mag;
            }
        }
    }

    Console.WriteLine($"Part 2: {maxMagnitude}");
}

long CalculateMagnitude(Element element)
{
    if (element.Value != null)
    {
        return element.Value.Value;
    }
    else if (element.Pair != null)
    {
        var leftMag = CalculateMagnitude(element.Pair.Left);
        var rightMag = CalculateMagnitude(element.Pair.Right);
        return leftMag * 3 + rightMag * 2;
    }

    throw new Exception("Element has neither Value nor Pair");
}

Part1();
Part2();


record Element()
{
    public long? Value { get; private init; }
    public Pair? Pair { get; private init; }

    public Element(long value) : this() { Value = value;  }
    public Element(Pair pair) : this() { Pair = pair; }

    public static Element Parse(ReadOnlySpan<char> span)
    {
        // [[[9,2],[[2,9],0]],[1,[[2,3],0]]]
        if (span[0] == '[')
        {
            var mid = FindComma(span);
            var left = Parse(span.Slice(1, mid - 1));
            var right = Parse(span.Slice(mid + 1, span.Length - mid - 2));
            return new Element(new Pair(left, right));
        }
        else
        {
            return new Element(long.Parse(span));
        }
    }

    private static int FindComma(ReadOnlySpan<char> span)
    {
        // Don't count initial bracket.
        int open = 0;
        for (int i = 1; i < span.Length; i++)
        {
            if (span[i] == '[') open++;
            else if (span[i] == ']') open--;
            else if (span[i] == ',' && open == 0) return i;
        }

        throw new Exception("Comma not found");
    }

    public Element Add(Element other)
    {
        var newElement = new Element(new Pair(this, other));
        var reduced = newElement.TryReduce();
        while (reduced != null)
        {
            newElement = reduced;
            reduced = reduced.TryReduce();
        }
        return newElement;
    }

    private Element? TryReduce()
    {
        var exploded = TryExplodeElement(this, 0);
        if (exploded != null)
        {
            return exploded.Value.NewElement;
        }

        var split = TrySplitElement(this);
        if (split != null)
        {
            return split;
        }

        return null;
    }

    private static (Element NewElement, long? LeftValue, long? RightValue)? TryExplodeElement(Element elem, int depth)
    {
        // If any pair is nested inside four pairs, the leftmost such pair explodes.
        // To explode a pair, the pair's left value is added to the first regular number
        // to the left of the exploding pair (if any), and the pair's right value is added
        // to the first regular number to the right of the exploding pair (if any).
        // Exploding pairs will always consist of two regular numbers.
        // Then, the entire exploding pair is replaced with the regular number 0.

        if (elem.Pair != null)
        {
            if (depth >= 4 && elem.Pair.Left.Value != null && elem.Pair.Right.Value != null)
            {
                var zero = new Element(0);
                return (zero, elem.Pair.Left.Value, elem.Pair.Right.Value);
            }

            if (elem.Pair != null)
            {
                var explodedLeft = TryExplodeElement(elem.Pair.Left, depth + 1);
                if (explodedLeft != null)
                {
                    var newRight = elem.Pair.Right;
                    var returnRightBalue = explodedLeft.Value.RightValue;
                    if (returnRightBalue != null)
                    {
                        var appliedRight = TryApplyExplodedValue(elem.Pair.Right, returnRightBalue.Value, false);
                        if (appliedRight != null)
                        {
                            newRight = appliedRight;
                            returnRightBalue = null;
                        }
                    }

                    var newPair = elem.Pair with { Left = explodedLeft.Value.NewElement, Right = newRight };
                    return (new Element(newPair), explodedLeft.Value.LeftValue, returnRightBalue);
                }
                else
                {
                    var explodedRight = TryExplodeElement(elem.Pair.Right, depth + 1);
                    if (explodedRight != null)
                    {
                        var newLeft = elem.Pair.Left;
                        var remainingLeftValue = explodedRight.Value.LeftValue;
                        if (remainingLeftValue != null)
                        {
                            var appliedLeft = TryApplyExplodedValue(elem.Pair.Left, remainingLeftValue.Value, true);
                            if (appliedLeft != null)
                            {
                                newLeft = appliedLeft;
                                remainingLeftValue = null;
                            }
                        }

                        var newPair = elem.Pair with { Left = newLeft, Right = explodedRight.Value.NewElement };
                        return (new Element(newPair), remainingLeftValue, explodedRight.Value.RightValue);
                    }
                }
            }
        }

        return null;
    }

    private static Element? TryApplyExplodedValue(Element elem, long value, bool isLeftValue)
    {
        if (elem.Value != null)
        {
            return new Element(elem.Value.Value + value);
        }
        else if (elem.Pair != null)
        {
            if (isLeftValue)
            {
                var rightApplied = TryApplyExplodedValue(elem.Pair.Right, value, isLeftValue);
                if (rightApplied != null)
                {
                    var newPair = elem.Pair with { Right = rightApplied };
                    return elem with { Pair = newPair };
                }

                var leftApplied = TryApplyExplodedValue(elem.Pair.Left, value, isLeftValue);
                if (leftApplied != null)
                {
                    var newPair = elem.Pair with { Left = leftApplied };
                    return elem with { Pair = newPair };
                }
            }
            else
            {
                var leftApplied = TryApplyExplodedValue(elem.Pair.Left, value, isLeftValue);
                if (leftApplied != null)
                {
                    var newPair = elem.Pair with { Left = leftApplied };
                    return elem with { Pair = newPair };
                }

                var rightApplied = TryApplyExplodedValue(elem.Pair.Right, value, isLeftValue);
                if (rightApplied != null)
                {
                    var newPair = elem.Pair with { Right = rightApplied };
                    return elem with { Pair = newPair };
                }
            }
        }

        return null;
    }

    private static Element? TrySplitElement(Element elem)
    {
        // If any regular number is 10 or greater, the leftmost such regular number splits.
        // To split a regular number, replace it with a pair; the left element of the pair
        // should be the regular number divided by two and rounded down, while the right
        // element of the pair should be the regular number divided by two and rounded up.

        if (elem.Value != null)
        {
            if (elem.Value >= 10)
            {
                var newLeft = new Element(elem.Value.Value / 2);
                var newRight = new Element((long) (elem.Value.Value / 2.0 + 0.5));
                return new Element(new Pair(newLeft, newRight));
            }
        }
        else if (elem.Pair != null)
        {
            var splitLeft = TrySplitElement(elem.Pair.Left);
            if (splitLeft != null)
            {
                var newPair = elem.Pair with { Left = splitLeft };
                return new Element(newPair);
            }

            var splitRight = TrySplitElement(elem.Pair.Right);
            if (splitRight != null)
            {
                var newPair = elem.Pair with { Right = splitRight };
                return new Element(newPair);
            }
        }

        return null;
    }

    public override string ToString()
    {
        if (Value != null)
        {
            return Value.Value.ToString();
        }
        else if (Pair != null)
        {
            return $"[{Pair.Left},{Pair.Right}]";
        }
        else
        {
            throw new Exception("Both Value and Pair are null");
        }
    }
}

record Pair(Element Left, Element Right);
