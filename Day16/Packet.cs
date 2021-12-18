using System.Text;

public abstract record Packet
{
    public Packet(int version, IReadOnlyList<Packet> subpackets)
    {
        Version = version;
        Subpackets = subpackets;
    }

    public int Version { get; }
    public IReadOnlyList<Packet> Subpackets { get; }

    public abstract long Evaluate();

    public static Packet Parse(BitStream stream)
    {
        var version = stream.ReadInt(3);
        var type = stream.ReadInt(3);

        switch (type)
        {
            case 4:
                {
                    var value = ParseLiteralValue(stream);
                    return new LiteralPacket(version, value);
                }

            default:
                {
                    var subpackets = ParseSubpackets(stream);

                    switch (type)
                    {
                        case 0:
                            return new SumPacket(version, subpackets);
                        case 1:
                            return new ProductPacket(version, subpackets);
                        case 2:
                            return new MinimumPacket(version, subpackets);
                        case 3:
                            return new MaximumPacket(version, subpackets);
                        case 5:
                            return new GreaterThanPacket(version, subpackets);
                        case 6:
                            return new LessThanPacket(version, subpackets);
                        case 7:
                            return new EqualToPacket(version, subpackets);
                        default:
                            throw new Exception($"Invalid type {type}");
                    }
                }
        }
    }

    public static long ParseLiteralValue(BitStream stream)
    {
        var value = 0L;
        while (true)
        {
            var chunk = stream.ReadInt(5);
            value <<= 4;
            value |= (long) (chunk & 0xF);

            if ((chunk & (1 << 4)) == 0)
            {
                break;
            }
        }

        return value;
    }

    public static IReadOnlyList<Packet> ParseSubpackets(BitStream stream)
    {
        var lengthType = stream.ReadInt(1);

        switch (lengthType)
        {
            case 0:
                return ParseSubpacketsWithBitsLength(stream);

            case 1:
                return ParseSubpacketsWithPacketsLength(stream);

            default:
                throw new Exception("Unknown length type");
        }
    }

    private static IReadOnlyList<Packet> ParseSubpacketsWithBitsLength(BitStream stream)
    {
        var numBits = stream.ReadInt(15);
        var bitsReadBefore = stream.BitsRead;
        var subPackets = new List<Packet>();
        while (stream.BitsRead < bitsReadBefore + numBits)
        {
            subPackets.Add(Parse(stream));
        }
        return subPackets;
    }

    private static IReadOnlyList<Packet> ParseSubpacketsWithPacketsLength(BitStream stream)
    {
        var numPackets = stream.ReadInt(11);
        var subPackets = new List<Packet>();
        while (subPackets.Count < numPackets)
        {
            subPackets.Add(Parse(stream));
        }
        return subPackets;
    }
}

public record LiteralPacket : Packet
{
    public LiteralPacket(int version, long value)
        : base(version, new Packet[0])
    {
        Value = value;
    }

    public long Value { get; }

    public override long Evaluate()
    {
        return Value;
    }
}

public record SumPacket : Packet
{
    public SumPacket(int version, IReadOnlyList<Packet> subpackets)
        : base(version, subpackets)
    {
    }

    public override long Evaluate()
    {
        return Subpackets.Sum(p => p.Evaluate());
    }
}

public record ProductPacket : Packet
{
    public ProductPacket(int version, IReadOnlyList<Packet> subpackets)
        : base(version, subpackets)
    {
    }

    public override long Evaluate()
    {
        return Subpackets.Aggregate(1L, (acc, p) => acc * p.Evaluate());
    }
}

public record MinimumPacket : Packet
{
    public MinimumPacket(int version, IReadOnlyList<Packet> subpackets)
        : base(version, subpackets)
    {
    }

    public override long Evaluate()
    {
        return Subpackets.Min(p => p.Evaluate());
    }
}

public record MaximumPacket : Packet
{
    public MaximumPacket(int version, IReadOnlyList<Packet> subpackets)
        : base(version, subpackets)
    {
    }

    public override long Evaluate()
    {
        return Subpackets.Max(p => p.Evaluate());
    }
}

public record GreaterThanPacket : Packet
{
    public GreaterThanPacket(int version, IReadOnlyList<Packet> subpackets)
        : base(version, subpackets)
    {
    }

    public override long Evaluate()
    {
        return Subpackets[0].Evaluate() > Subpackets[1].Evaluate() ? 1 : 0;
    }
}

public record LessThanPacket : Packet
{
    public LessThanPacket(int version, IReadOnlyList<Packet> subpackets)
        : base(version, subpackets)
    {
    }

    public override long Evaluate()
    {
        return Subpackets[0].Evaluate() < Subpackets[1].Evaluate() ? 1 : 0;
    }
}

public record EqualToPacket : Packet
{
    public EqualToPacket(int version, IReadOnlyList<Packet> subpackets)
        : base(version, subpackets)
    {
    }

    public override long Evaluate()
    {
        return Subpackets[0].Evaluate() == Subpackets[1].Evaluate() ? 1 : 0;
    }
}
