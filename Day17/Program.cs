(int x1, int x2, int y1, int y2) LoadInput()
{
    //var input = "target area: x=20..30, y=-10..-5";
    var input = File.ReadAllLines("input.txt").First();

    var split1 = input.Split(": ");
    var xysplit = split1[1].Split(", ");
    var xsplit = xysplit[0][2..].Split("..");
    var ysplit = xysplit[1][2..].Split("..");
    var x1 = int.Parse(xsplit[0]);
    var x2 = int.Parse(xsplit[1]);
    var y1 = int.Parse(ysplit[0]);
    var y2 = int.Parse(ysplit[1]);

    return (x1, x2, y1, y2);
}

void Part1()
{
    var (x1, x2, y1, y2) = LoadInput();

    int? highest = null;
    for (int dx = 1; dx <= x2; dx++)
    {
        for (int dy = y1; dy < 1000000; dy++)
        {
            var height = ShootProbe(dx, dy, x1, x2, y1, y2);
            if (height.HasValue && (!highest.HasValue || height > highest))
            {
                highest = height;
            }
        }
    }

    Console.WriteLine($"Part 1: {highest}");
}

void Part2()
{
    var (x1, x2, y1, y2) = LoadInput();

    var hits = 0;
    for (int dx = 1; dx <= x2; dx++)
    {
        for (int dy = y1; dy < 1000000; dy++)
        {
            var height = ShootProbe(dx, dy, x1, x2, y1, y2);
            if (height.HasValue)
            {
                hits++;
            }
        }
    }

    Console.WriteLine($"Part 2: {hits}");
}

int? ShootProbe(int dx, int dy, int x1, int x2, int y1, int y2)
{
    var x = 0;
    var y = 0;
    var height = 0;

    while (true)
    {
        x += dx;
        y += dy;

        if (dx > 0) dx--;
        dy--;

        if (y > height) height = y;

        if (x >= x1 && x <= x2 && y >= y1 && y <= y2) return height;
        else if (dx == 0 && x < x1) return null;
        else if (x > x2) return null;
        else if (y < y1) return null;
    }
}

Part1();
Part2();
