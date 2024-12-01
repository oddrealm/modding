using System;
using System.Runtime.CompilerServices;

public readonly struct LocationUID : IEquatable<LocationUID>
{
    public const uint MAX = 256 * 256 * 256;

    public readonly static LocationUID NULL = MAX;
    public readonly static LocationUID OUT_OF_BOUNDS = MAX + 1;

    public bool IsNULL { get { return _uid == MAX; } }
    public bool IsOutBounds { get { return _uid >= OUT_OF_BOUNDS; } }

    private readonly uint _uid;

    public LocationUID(uint uid) { _uid = uid; }

    public static ushort WIDTH;
    public static ushort HEIGHT;
    public static ushort DEPTH;
    public static byte BIT_WIDTH;
    public static byte BIT_HEIGHT;
    public static byte BIT_DEPTH;
    public static byte CHUNK_BIT_WIDTH;
    public static byte CHUNK_BIT_HEIGHT;
    public static byte CHUNK_BIT_DEPTH;
    public static byte CHUNK_SHIFT;
    public static byte CHUNK_WIDTH;
    public static byte CHUNK_HEIGHT;
    public static byte CHUNK_DEPTH;
    public static int TOTAL_CHUNKS;
    public static int LAYER_SIZE;
    public static uint MaxSize;

    public bool Equals(LocationUID other)
    {
        return this._uid == other._uid;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is LocationUID))
            return false;

        LocationUID other = (LocationUID)obj;
        return _uid == other._uid;
    }

    public override int GetHashCode()
    {
        return _uid.GetHashCode();
    }

    public static implicit operator LocationUID(uint value)
    {
        return new LocationUID(value);
    }

    public static implicit operator uint(LocationUID value)
    {
        return value._uid;
    }

    public static implicit operator LocationUID(int value)
    {
        return new LocationUID((uint)value);
    }

    public static implicit operator int(LocationUID value)
    {
        return (int)value._uid;
    }

    public static bool operator ==(LocationUID left, LocationUID right)
    {
        return left._uid == right._uid;
    }

    public static bool operator !=(LocationUID left, LocationUID right)
    {
        return left._uid != right._uid;
    }

#if ODD_REALM_APP

    public LocationUID(BlockPoint point)
    {
        _uid = (uint)((point.x << BIT_HEIGHT << BIT_DEPTH) |
                      (point.y << BIT_DEPTH) |
                       point.z);
    }

    public LocationUID(int x, int y, int z)
    {
        _uid = (uint)((x << BIT_HEIGHT << BIT_DEPTH) |
                      (y << BIT_DEPTH) |
                       z);
    }

    public static void Init(WorldDimensions dimensions)
    {
        WIDTH = (ushort)(dimensions.Width - 1);
        HEIGHT = (ushort)(dimensions.Height - 1);
        DEPTH = (ushort)(dimensions.Depth - 1);
        BIT_WIDTH = dimensions.BitWidth;
        BIT_HEIGHT = dimensions.BitHeight;
        BIT_DEPTH = dimensions.BitDepth;
        CHUNK_BIT_WIDTH = (byte)(dimensions.BitWidth - RenderChunk.BITS_X);
        CHUNK_BIT_HEIGHT = (byte)(dimensions.BitHeight - RenderChunk.BITS_Y);
        CHUNK_BIT_DEPTH = (byte)(dimensions.BitDepth - RenderChunk.BITS_Z);
        CHUNK_WIDTH = (byte)Math.Pow(2, CHUNK_BIT_WIDTH);
        CHUNK_HEIGHT = (byte)Math.Pow(2, CHUNK_BIT_HEIGHT);
        CHUNK_DEPTH = (byte)Math.Pow(2, CHUNK_BIT_DEPTH);
        LAYER_SIZE = CHUNK_WIDTH * CHUNK_HEIGHT;
        TOTAL_CHUNKS = CHUNK_WIDTH * CHUNK_HEIGHT * CHUNK_DEPTH;
        MaxSize = dimensions.TotalBlocks;
    }

    public static void UnInit()
    {

    }

    public byte x { get { return (byte)(((_uid) >> BIT_HEIGHT >> BIT_DEPTH) & WIDTH); } }
    public byte y { get { return (byte)(((_uid) >> BIT_DEPTH) & HEIGHT); } }
    public byte z { get { return (byte)((_uid) & DEPTH); } }

    public BlockPoint Point { get { return new BlockPoint(x, y, z); } }

    public ushort ChunkIndex
    {
        get
        {
            ushort xPart = (ushort)((x >> 3) << CHUNK_BIT_HEIGHT << CHUNK_BIT_DEPTH);

            ushort yPart = (ushort)((y >> 3) << CHUNK_BIT_DEPTH);

            ushort zPart = (ushort)(z >> 3);

            // Combine the extracted parts to form the final 'renderIndex'.
            return (ushort)(xPart | yPart | zPart);
        }
    }

    public byte ChunkLayer
    {
        get
        {
            return (byte)(this.z & (RenderChunk.SIZE_Z - 1));
        }
    }

    public static BlockPoint ToBlockPoint(LocationUID locationUID)
    {
        return new BlockPoint(locationUID.x,
                              locationUID.y,
                              locationUID.z);
    }

    public static bool InBounds(BlockPoint p)
    {
        return p.x >= 0 && p.x <= WIDTH && p.y >= 0 && p.y <= HEIGHT && p.z >= 0 && p.z <= DEPTH;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocationUID North(LocationUID l)
    {
        byte y = l.y;

        if (y >= HEIGHT) { return OUT_OF_BOUNDS; }

        y = (byte)(y + 1);
        return (uint)((l.x << BIT_HEIGHT << BIT_DEPTH) | (y << BIT_DEPTH) | l.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocationUID NorthEast(LocationUID l)
    {
        byte y = l.y;

        if (y >= HEIGHT) { return OUT_OF_BOUNDS; }

        byte x = l.x;

        if (x >= WIDTH) { return OUT_OF_BOUNDS; }

        y = (byte)(y + 1);
        x = (byte)(x + 1);

        return (uint)((x << BIT_HEIGHT << BIT_DEPTH) | (y << BIT_DEPTH) | l.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocationUID East(LocationUID l)
    {
        byte x = l.x;

        if (x >= WIDTH) { return OUT_OF_BOUNDS; }

        x = (byte)(x + 1);
        return (uint)((x << BIT_HEIGHT << BIT_DEPTH) | (l.y << BIT_DEPTH) | l.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocationUID SouthEast(LocationUID l)
    {
        byte y = l.y;

        if (y <= 0) { return OUT_OF_BOUNDS; }

        byte x = l.x;

        if (x >= WIDTH) { return OUT_OF_BOUNDS; }

        y = (byte)(y - 1);
        x = (byte)(x + 1);

        return (uint)((x << BIT_HEIGHT << BIT_DEPTH) | (y << BIT_DEPTH) | l.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocationUID South(LocationUID l)
    {
        byte y = l.y;

        if (y <= 0) { return OUT_OF_BOUNDS; }

        y = (byte)(y - 1);
        return (uint)((l.x << BIT_HEIGHT << BIT_DEPTH) | (y << BIT_DEPTH) | l.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocationUID SouthWest(LocationUID l)
    {
        byte y = l.y;

        if (y <= 0) { return OUT_OF_BOUNDS; }

        byte x = l.x;

        if (x <= 0) { return OUT_OF_BOUNDS; }

        y = (byte)(y - 1);
        x = (byte)(x - 1);

        return (uint)((x << BIT_HEIGHT << BIT_DEPTH) | (y << BIT_DEPTH) | l.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocationUID West(LocationUID l)
    {
        byte x = l.x;

        if (x <= 0) { return OUT_OF_BOUNDS; }

        x = (byte)(x - 1);
        return (uint)((x << BIT_HEIGHT << BIT_DEPTH) | (l.y << BIT_DEPTH) | l.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocationUID NorthWest(LocationUID l)
    {
        byte y = l.y;

        if (y >= HEIGHT) { return OUT_OF_BOUNDS; }

        byte x = l.x;

        if (x <= 0) { return OUT_OF_BOUNDS; }

        y = (byte)(y + 1);
        x = (byte)(x - 1);

        return (uint)((x << BIT_HEIGHT << BIT_DEPTH) | (y << BIT_DEPTH) | l.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocationUID Up(LocationUID l)
    {
        byte z = l.z;

        if (z >= DEPTH) { return OUT_OF_BOUNDS; }

        z = (byte)(z + 1);
        return (uint)((l.x << BIT_HEIGHT << BIT_DEPTH) | (l.y << BIT_DEPTH) | z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LocationUID Down(LocationUID l)
    {
        byte z = l.z;

        if (z <= 0) { return OUT_OF_BOUNDS; }

        z = (byte)(z - 1);
        return (uint)((l.x << BIT_HEIGHT << BIT_DEPTH) | (l.y << BIT_DEPTH) | z);
    }

    public static LocationUID GetNeighbor(LocationUID origin, int i)
    {
        switch (i)
        {
            case 0:
                return North(origin);
            case 1:
                return NorthEast(origin);
            case 2:
                return East(origin);
            case 3:
                return SouthEast(origin);
            case 4:
                return South(origin);
            case 5:
                return SouthWest(origin);
            case 6:
                return West(origin);
            case 7:
                return NorthWest(origin);
            case 8:
                return Up(origin);
            case 9:
                return Down(origin);
        }

        return origin;
    }

    public override string ToString()
    {
        if (IsNULL) { return "[NULL]"; }
        if (IsOutBounds) { return "[OUT_OF_BOUNDS]"; }
        return $"[UID:{_uid}, {Point}]";
    }
#endif
}