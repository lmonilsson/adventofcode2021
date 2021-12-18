public class BitStream
{
    private IReadOnlyList<byte> _bytes;

    public BitStream(IReadOnlyList<byte> bytes)
    {
        _bytes = bytes;
        BitsRead = 0;
    }

    public int BitsRead { get; private set; }

    public int ReadInt(int bits)
    {
        if (bits > 31)
        {
            throw new Exception("bits must be less than 32 (signed values not supported)");
        }

        int result = 0;

        for (int bit = 0; bit < bits; bit++)
        {
            var byteIdx = BitsRead / 8;
            var readShift = 7 - (BitsRead % 8);
            var bitValue = (_bytes[byteIdx] & (1 << readShift)) >> readShift;
            var writeShift = (bits - bit - 1);
            result |= bitValue << writeShift;
            BitsRead++;
        }

        return result;
    }
}
