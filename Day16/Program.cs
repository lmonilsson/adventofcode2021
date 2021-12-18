using System.Globalization;

string LoadInput()
{
    // Operator
    // Length type Bits
    // Sub-packet length: 27 bits
    // Sub-packet: Literal value 10
    // Sub-packet: Literal value 20
    //return "38006F45291200";

    // Operator
    // Length type Packets
    // Sub-packet length: 3
    // Sub-packet: Literal value 1
    // Sub-packet: Literal value 2
    // Sub-packet: Literal value 3
    //return "EE00D40C823060";

    // Version sum: 16
    //return "8A004A801A8002F478";

    // Version sum: 12
    //return "620080001611562C8802118E34";

    // Version sum: 23
    //return "C0015000016115A2E0802F182340";

    // Version sum: 31
    //return "A0016C880162017C3686B18A3D4780";

    // Evaluate: 3
    //return "C200B40A82";

    // Evaluate: 1
    //return "9C0141080250320F1802104A08";

    return File.ReadAllLines("input.txt").First();
}

void Part1()
{
    var bytes = LoadInput().Chunk(2).Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToList();
    var bitStream = new BitStream(bytes);
    var packet = Packet.Parse(bitStream);
    Console.WriteLine($"Part 1: {SumPacketVersions(packet)}");
}

void Part2()
{
    var bytes = LoadInput().Chunk(2).Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToList();
    var bitStream = new BitStream(bytes);
    var packet = Packet.Parse(bitStream);
    Console.WriteLine($"Part 2: {packet.Evaluate()}");
}

int SumPacketVersions(Packet packet)
{
    return packet.Version + packet.Subpackets.Sum(SumPacketVersions);
}

Part1();
Part2();
