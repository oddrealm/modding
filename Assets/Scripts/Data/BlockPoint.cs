using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;





public struct BlockPoint : IEqualityComparer<BlockPoint>
{
    // Y offset from blockpoint floor.
    public const float CAM_Z = 0.99f;
    public const float SURFACE_Z = 0.095f;
    public const float EMPTY_BLOCK_MESH_Z = 0.08f;
    public const float FX_Z = 0.07f;
    public const float ENTITY_Z = 0.062f;
    public const float FISH_Z = 0.061f;
    public const float ENTITY_SHADOW_Z = 0.06f;
    // overlays = 0.05
    // conditions = 0.042
    // Block surface = 0.04
    // block interior = 0.03
    // item = 0.025
    // plant = 0.02
    // platform = 0.01
    public const float BLOCK_Z = 0.0f;

    public bool IsNULL { get; private set; }

    public static readonly BlockPoint NULL = new BlockPoint(true);
    public static readonly BlockPoint ZERO = new BlockPoint();
    public readonly int x;
    public readonly int y;
    public readonly int z;

    public const int MAX_X = 256;
    public const int MAX_Y = 256;
    public const int MAX_Z = 256;

    public BlockPoint(bool isNull)
    {
        IsNULL = isNull;
        this.x = -1;
        this.y = -1;
        this.z = -1;
    }

    public BlockPoint(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        IsNULL = x == -1 && y == -1 && z == -1;
    }

    public BlockPoint(float x, float y, float z)
    {
        this.x = (int)x;
        this.y = (int)y;
        this.z = (int)z;
        IsNULL = this.x == -1 && this.y == -1 && this.z == -1;
    }

    public BlockPoint(Vector3Int intVec)
    {
        this.x = (int)intVec.x;
        this.y = (int)intVec.y;
        this.z = (int)intVec.z;
        IsNULL = this.x == -1 && this.y == -1 && this.z == -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint North(int i = 1)
    {
        return new BlockPoint(x, y + i, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint East(int i = 1)
    {
        return new BlockPoint(x + i, y, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint South(int i = 1)
    {
        return new BlockPoint(x, y - i, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint West(int i = 1)
    {
        return new BlockPoint(x - i, y, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint Up(int i = 1)
    {
        return new BlockPoint(x, y, z + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint UpNorth(int i = 1)
    {
        return new BlockPoint(x, y + i, z + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint UpEast(int i = 1)
    {
        return new BlockPoint(x + i, y, z + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint UpSouth(int i = 1)
    {
        return new BlockPoint(x, y - i, z + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint UpWest(int i = 1)
    {
        return new BlockPoint(x - i, y, z + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint Down(int i = 1)
    {
        return new BlockPoint(x, y, z - i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint DownNorth(int i = 1)
    {
        return new BlockPoint(x, y + i, z - i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint DownEast(int i = 1)
    {
        return new BlockPoint(x + i, y, z - i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint DownSouth(int i = 1)
    {
        return new BlockPoint(x, y - i, z - i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint DownWest(int i = 1)
    {
        return new BlockPoint(x - i, y, z - i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint NorthEast(int i = 1)
    {
        return new BlockPoint(x + i, y + i, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint SouthEast(int i = 1)
    {
        return new BlockPoint(x + i, y - i, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint SouthWest(int i = 1)
    {
        return new BlockPoint(x - i, y - i, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint NorthWest(int i = 1)
    {
        return new BlockPoint(x - i, y + i, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint UpNorthEast(int i = 1)
    {
        return new BlockPoint(x + i, y + i, z + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint UpSouthEast(int i = 1)
    {
        return new BlockPoint(x + i, y - i, z + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint UpSouthWest(int i = 1)
    {
        return new BlockPoint(x - i, y - i, z + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint UpNorthWest(int i = 1)
    {
        return new BlockPoint(x - i, y + i, z + i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint DownNorthEast(int i = 1)
    {
        return new BlockPoint(x + i, y + i, z - i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint DownSouthEast(int i = 1)
    {
        return new BlockPoint(x + i, y - i, z - i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint DownSouthWest(int i = 1)
    {
        return new BlockPoint(x - i, y - i, z - i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint DownNorthWest(int i = 1)
    {
        return new BlockPoint(x - i, y + i, z - i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(x, y, z);
    }

#if ODD_REALM_APP

    public TileLocationUID ToTileUID()
    {
        return new TileLocationUID(x, y);
    }

    public BlockPoint(Vector3 vec)
    {
        this.x = (int)vec.x / Master.Instance.PixelsPerBlockAxis;
        this.y = (int)vec.y / Master.Instance.PixelsPerBlockAxis;
        this.z = -(int)vec.z / Master.Instance.PixelsPerBlockAxis;
        IsNULL = this.x == -1 && this.y == -1 && this.z == -1;
    }


    private static GameSession _session;

    public static void Init(GameSession session)
    {
        _session = session;
    }

    public static void Save(ES2Writer writer, BlockPoint p)
    {
        writer.Write(p.x);
        writer.Write(p.y);
        writer.Write(p.z);
    }

    public static BlockPoint Load(ES2Reader reader)
    {
        int x = reader.Read<int>();
        int y = reader.Read<int>();
        int z = reader.Read<int>();

        return new BlockPoint(x, y, z);
    }

    // TODO: Write a proper line iterator where start and end can be different in 3D space.
    public static void IterateVerticalLine(BlockPoint start, BlockPoint end, System.Action<BlockPoint> onLine)
    {
        onLine?.Invoke(start);

        if (start == end)
        {
            return;
        }

        BlockPoint c = start;
        int dist = ManhattanDistance(start, end);

        for (int i = 0; i < dist; i++)
        {
            BlockPoint dir = DirectionAsBlockPoint(c, end);

            c += dir;

            onLine?.Invoke(c);
        }
    }

    public static void IteratePointsInArea(BlockPoint start, BlockPoint end, bool borderOnly, System.Action<BlockPoint> callback)
    {
        BlockPoint delta = (end - start).Abs();
        int originX = start.x;
        int originY = start.y;
        int originZ = start.z;

        int width = delta.x;
        int height = delta.y;
        int depth = delta.z;

        if (originX > end.x) { originX = end.x; }
        if (originY > end.y) { originY = end.y; }
        if (originZ < end.z) { originZ = end.z; }

        if (width * height > 5000) { return; }

        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                if (borderOnly &&
                    x != 0 && x != width && y != 0 && y != height) { continue; }

                int nx = originX + x;
                int ny = originY + y;

                for (int z = 0; z <= depth; z++)
                {
                    int nz = originZ - z;

                    if (nz < 0) { break; }

                    BlockPoint bp = new BlockPoint(nx, ny, nz);

                    callback(bp);
                }
            }
        }
    }

    public void IterateAllNeighbors(System.Action<BlockPoint> onNeighbor)
    {
        if (this == NULL) { return; }
        onNeighbor(new BlockPoint(x, y + 1, z));
        onNeighbor(new BlockPoint(x + 1, y + 1, z));
        onNeighbor(new BlockPoint(x + 1, y, z));
        onNeighbor(new BlockPoint(x + 1, y - 1, z));
        onNeighbor(new BlockPoint(x, y - 1, z));
        onNeighbor(new BlockPoint(x - 1, y - 1, z));
        onNeighbor(new BlockPoint(x - 1, y, z));
        onNeighbor(new BlockPoint(x - 1, y + 1, z));

        onNeighbor(new BlockPoint(x, y, z + 1));
        onNeighbor(new BlockPoint(x, y, z - 1));

        onNeighbor(new BlockPoint(x, y + 1, z + 1));
        onNeighbor(new BlockPoint(x + 1, y + 1, z + 1));
        onNeighbor(new BlockPoint(x + 1, y, z + 1));
        onNeighbor(new BlockPoint(x + 1, y - 1, z + 1));
        onNeighbor(new BlockPoint(x, y - 1, z + 1));
        onNeighbor(new BlockPoint(x - 1, y - 1, z + 1));
        onNeighbor(new BlockPoint(x - 1, y, z + 1));
        onNeighbor(new BlockPoint(x - 1, y + 1, z + 1));

        onNeighbor(new BlockPoint(x, y + 1, z - 1));
        onNeighbor(new BlockPoint(x + 1, y + 1, z - 1));
        onNeighbor(new BlockPoint(x + 1, y, z - 1));
        onNeighbor(new BlockPoint(x + 1, y - 1, z - 1));
        onNeighbor(new BlockPoint(x, y - 1, z - 1));
        onNeighbor(new BlockPoint(x - 1, y - 1, z - 1));
        onNeighbor(new BlockPoint(x - 1, y, z - 1));
        onNeighbor(new BlockPoint(x - 1, y + 1, z - 1));
    }

    public void IterateCardinalAndVerticalNeighbors(System.Action<BlockPoint> onNeighbor)
    {
        if (this == NULL) { return; }
        onNeighbor(new BlockPoint(x, y + 1, z));
        onNeighbor(new BlockPoint(x + 1, y + 1, z));
        onNeighbor(new BlockPoint(x + 1, y, z));
        onNeighbor(new BlockPoint(x + 1, y - 1, z));
        onNeighbor(new BlockPoint(x, y - 1, z));
        onNeighbor(new BlockPoint(x - 1, y - 1, z));
        onNeighbor(new BlockPoint(x - 1, y, z));
        onNeighbor(new BlockPoint(x - 1, y + 1, z));

        onNeighbor(new BlockPoint(x, y, z + 1));
        onNeighbor(new BlockPoint(x, y, z - 1));
    }

    public void IterateCardinalNeighbors(System.Action<BlockPoint> onNeighbor)
    {
        if (this == NULL) { return; }
        onNeighbor(new BlockPoint(x, y + 1, z));
        onNeighbor(new BlockPoint(x + 1, y + 1, z));
        onNeighbor(new BlockPoint(x + 1, y, z));
        onNeighbor(new BlockPoint(x + 1, y - 1, z));
        onNeighbor(new BlockPoint(x, y - 1, z));
        onNeighbor(new BlockPoint(x - 1, y - 1, z));
        onNeighbor(new BlockPoint(x - 1, y, z));
        onNeighbor(new BlockPoint(x - 1, y + 1, z));
    }

    public BlockPoint GetNeighbor(BlockDirection dir)
    {
        switch (dir.Angle)
        {
            case BlockDirectionTypes.NORTH:
                return North();
            case BlockDirectionTypes.EAST:
                return East();
            case BlockDirectionTypes.SOUTH:
                return South();
            case BlockDirectionTypes.WEST:
                return West();
            case BlockDirectionTypes.UP:
                return Up();
            case BlockDirectionTypes.DOWN:
                return Down();
        }

        Debug.LogError("Unsupported direction: " + dir.Angle);

        return this;
    }

    public BlockPoint GetRandomNeighbor(int distance)
    {
        BlockPoint np = GetNeighbor(TinyBeast.Random.Range(0, 27));

        return np * distance;
    }

    public BlockPoint GetNeighbor(int i)
    {
        switch (i)
        {
            case 0:
                return North();
            case 1:
                return NorthEast();
            case 2:
                return East();
            case 3:
                return SouthEast();
            case 4:
                return South();
            case 5:
                return SouthWest();
            case 6:
                return West();
            case 7:
                return NorthWest();
            case 8:
                return Up();
            case 9:
                return Down();
            case 10:
                return UpNorth();
            case 11:
                return UpNorthEast();
            case 12:
                return UpEast();
            case 13:
                return UpSouthEast();
            case 14:
                return UpSouth();
            case 15:
                return UpSouthWest();
            case 16:
                return UpWest();
            case 17:
                return UpNorthWest();
            case 18:
                return DownNorth();
            case 19:
                return DownNorthEast();
            case 20:
                return DownEast();
            case 21:
                return DownSouthEast();
            case 22:
                return DownSouth();
            case 23:
                return DownSouthWest();
            case 24:
                return DownWest();
            case 25:
                return DownNorthWest();
        }

        return NULL;
    }

    public BlockPoint RandomCardinalFromPoint(BlockDirectionFlags permitted = BlockDirectionFlags.ALL, int distance = 1)
    {
        int r = TinyBeast.Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
        {
            BlockDirection dir = new BlockDirection((i + r) % 4);

            if ((dir.Flag & permitted) == BlockDirectionFlags.NONE) { continue; }

            return (dir.ToBlockPoint() * distance) + this;
        }

        return NULL;
    }

    public BlockPoint RandomCardinalAndVerticalDirectionFromPoint(BlockDirectionFlags permitted = BlockDirectionFlags.ALL, int distance = 1)
    {
        int r = TinyBeast.Random.Range(0, 6);

        for (int i = 0; i < 6; i++)
        {
            BlockDirection dir = new BlockDirection((i + r) % 6);

            if ((dir.Flag & permitted) == BlockDirectionFlags.NONE) { continue; }

            return (dir.ToBlockPoint() * distance) + this;
        }

        return NULL;
    }

    public BlockPoint RandomCardinalFromPoint(int i = 1)
    {
        int r = TinyBeast.Random.Range(0, 4);

        if (r == 0) { return North(i); }
        if (r == 1) { return East(i); }
        if (r == 2) { return South(i); }
        if (r == 3) { return West(i); }

        return this;
    }

    public BlockPoint RandomCardinalAndVerticalDirectionFromPoint(int i = 1)
    {
        int r = TinyBeast.Random.Range(0, 6);

        if (r == 0) { return North(i); }
        if (r == 1) { return East(i); }
        if (r == 2) { return South(i); }
        if (r == 3) { return West(i); }
        if (r == 4) { return Up(i); }
        if (r == 5) { return Down(i); }

        return this;
    }

    public static BlockPoint GetRandomCardinalDirection(int i = 1)
    {
        int r = TinyBeast.Random.Range(0, 4);

        if (r == 0) { return BlockDirection.North().ToBlockPoint(i); }
        if (r == 1) { return BlockDirection.East().ToBlockPoint(i); }
        if (r == 2) { return BlockDirection.South().ToBlockPoint(i); }
        if (r == 3) { return BlockDirection.West().ToBlockPoint(i); }

        return BlockDirection.North().ToBlockPoint(i);
    }

    public BlockPoint[] CardinalFourAndVerticalNeighbors(BlockPoint[] cachedArray)
    {
        cachedArray[0] = North();
        cachedArray[1] = East();
        cachedArray[2] = South();
        cachedArray[3] = West();
        cachedArray[4] = Up();
        cachedArray[5] = Down();

        return cachedArray;
    }

    public BlockPoint[] CardinalFourAndVerticalNeighbors()
    {
        BlockPoint[] n = new BlockPoint[6];
        n[0] = North();
        n[1] = East();
        n[2] = South();
        n[3] = West();
        n[4] = Up();
        n[5] = Down();

        return n;
    }

    public BlockPoint[] CardinalFourAndDownNeighbors(BlockPoint[] cachedArray)
    {
        cachedArray[0] = North();
        cachedArray[1] = East();
        cachedArray[2] = South();
        cachedArray[3] = West();
        cachedArray[4] = Down();

        return cachedArray;
    }

    public BlockPoint[] CardinalFourAndDownNeighbors()
    {
        BlockPoint[] n = new BlockPoint[5];
        n[0] = North();
        n[1] = East();
        n[2] = South();
        n[3] = West();
        n[4] = Down();

        return n;
    }

    public BlockPoint[] IntercardinalFourNeighbors(BlockPoint[] cachedArray)
    {
        cachedArray[0] = NorthEast();
        cachedArray[1] = SouthEast();
        cachedArray[2] = SouthWest();
        cachedArray[3] = NorthWest();

        return cachedArray;
    }

    public BlockPoint[] IntercardinalFourNeighbors()
    {
        BlockPoint[] n = new BlockPoint[4];
        n[0] = NorthEast();
        n[1] = SouthEast();
        n[2] = SouthWest();
        n[3] = NorthWest();

        return n;
    }

    public BlockPoint[] CardinalFourNeighbors(BlockPoint[] cachedArray)
    {
        cachedArray[0] = North();
        cachedArray[1] = East();
        cachedArray[2] = South();
        cachedArray[3] = West();

        return cachedArray;
    }

    public BlockPoint[] CardinalFourNeighbors()
    {
        BlockPoint[] n = new BlockPoint[4];
        n[0] = North();
        n[1] = East();
        n[2] = South();
        n[3] = West();

        return n;
    }

    public BlockPoint[] CardinalEightNeighbors()
    {
        BlockPoint[] n = new BlockPoint[8];
        n[0] = North();
        n[1] = NorthEast();
        n[2] = East();
        n[3] = SouthEast();
        n[4] = South();
        n[5] = SouthWest();
        n[6] = West();
        n[7] = NorthWest();

        return n;
    }

    public BlockPoint[] CardinalEightNeighbors(BlockPoint[] cachedArray)
    {
        cachedArray[0] = North();
        cachedArray[1] = NorthEast();
        cachedArray[2] = East();
        cachedArray[3] = SouthEast();
        cachedArray[4] = South();
        cachedArray[5] = SouthWest();
        cachedArray[6] = West();
        cachedArray[7] = NorthWest();

        return cachedArray;
    }

    public BlockPoint[] CardinalEightAndVerticalNeighbors()
    {
        BlockPoint[] n = new BlockPoint[10];
        n[0] = North();
        n[1] = NorthEast();
        n[2] = East();
        n[3] = SouthEast();
        n[4] = South();
        n[5] = SouthWest();
        n[6] = West();
        n[7] = NorthWest();
        n[8] = Up();
        n[9] = Down();

        return n;
    }

    public BlockPoint[] CardinalEightAndVerticalNeighbors(BlockPoint[] cachedArray)
    {
        cachedArray[0] = North();
        cachedArray[1] = NorthEast();
        cachedArray[2] = East();
        cachedArray[3] = SouthEast();
        cachedArray[4] = South();
        cachedArray[5] = SouthWest();
        cachedArray[6] = West();
        cachedArray[7] = NorthWest();
        cachedArray[8] = Up();
        cachedArray[9] = Down();

        return cachedArray;
    }

    public BlockPoint[] AllNeighbors()
    {
        BlockPoint[] n = new BlockPoint[26];

        // Center.
        n[0] = North();
        n[1] = NorthEast();
        n[2] = East();
        n[3] = SouthEast();
        n[4] = South();
        n[5] = SouthWest();
        n[6] = West();
        n[7] = NorthWest();
        n[8] = Up();
        n[9] = Down();

        // Top.
        n[10] = n[8].North();
        n[11] = n[8].NorthEast();
        n[12] = n[8].East();
        n[13] = n[8].SouthEast();
        n[14] = n[8].South();
        n[15] = n[8].SouthWest();
        n[16] = n[8].West();
        n[17] = n[8].NorthWest();

        // Down.
        n[18] = n[9].North();
        n[19] = n[9].NorthEast();
        n[20] = n[9].East();
        n[21] = n[9].SouthEast();
        n[22] = n[9].South();
        n[23] = n[9].SouthWest();
        n[24] = n[9].West();
        n[25] = n[9].NorthWest();

        return n;
    }

    public BlockPoint[] AllNeighbors(BlockPoint[] cachedArray)
    {
        // Center.
        cachedArray[0] = North();
        cachedArray[1] = NorthEast();
        cachedArray[2] = East();
        cachedArray[3] = SouthEast();
        cachedArray[4] = South();
        cachedArray[5] = SouthWest();
        cachedArray[6] = West();
        cachedArray[7] = NorthWest();
        cachedArray[8] = Up();
        cachedArray[9] = Down();

        // Top.
        cachedArray[10] = cachedArray[8].North();
        cachedArray[11] = cachedArray[8].NorthEast();
        cachedArray[12] = cachedArray[8].East();
        cachedArray[13] = cachedArray[8].SouthEast();
        cachedArray[14] = cachedArray[8].South();
        cachedArray[15] = cachedArray[8].SouthWest();
        cachedArray[16] = cachedArray[8].West();
        cachedArray[17] = cachedArray[8].NorthWest();

        // Down.
        cachedArray[18] = cachedArray[9].North();
        cachedArray[19] = cachedArray[9].NorthEast();
        cachedArray[20] = cachedArray[9].East();
        cachedArray[21] = cachedArray[9].SouthEast();
        cachedArray[22] = cachedArray[9].South();
        cachedArray[23] = cachedArray[9].SouthWest();
        cachedArray[24] = cachedArray[9].West();
        cachedArray[25] = cachedArray[9].NorthWest();

        return cachedArray;
    }

    public BlockPoint[] CardinalSecondRow(BlockPoint[] cachedArray)
    {
        cachedArray[0] = new BlockPoint(x - 2, y + 2, z);
        cachedArray[1] = new BlockPoint(x - 1, y + 2, z);
        cachedArray[2] = new BlockPoint(x, y + 2, z);
        cachedArray[3] = new BlockPoint(x + 1, y + 2, z);
        cachedArray[4] = new BlockPoint(x + 2, y + 2, z);
        cachedArray[5] = new BlockPoint(x + 2, y + 1, z);
        cachedArray[6] = new BlockPoint(x + 2, y, z);
        cachedArray[7] = new BlockPoint(x + 1, y - 1, z);
        cachedArray[8] = new BlockPoint(x + 2, y - 2, z);
        cachedArray[9] = new BlockPoint(x + 1, y - 2, z);
        cachedArray[10] = new BlockPoint(x, y - 2, z);
        cachedArray[11] = new BlockPoint(x - 1, y - 2, z);
        cachedArray[12] = new BlockPoint(x - 2, y - 2, z);
        cachedArray[13] = new BlockPoint(x - 2, y - 1, z);
        cachedArray[14] = new BlockPoint(x - 2, y, z);
        cachedArray[15] = new BlockPoint(x - 2, y + 1, z);

        return cachedArray;
    }

    public BlockPoint ClosestCardinalNeighbor(BlockPoint originPoint, System.Func<BlockPoint, bool> filter)
    {
        BlockPoint[] neighbors = CardinalEightNeighbors();
        BlockPoint closest = originPoint;
        float closestDist = float.MaxValue;

        for (int i = 0; i < neighbors.Length; i++)
        {

            if (!filter(neighbors[i])) { continue; }

            BlockPoint a = new BlockPoint(neighbors[i].x, neighbors[i].y, 0);
            BlockPoint b = new BlockPoint(originPoint.x, originPoint.y, 0);
            float dist = BlockPoint.Distance(a, b);

            if (dist >= closestDist) { continue; }

            closestDist = dist;
            closest = neighbors[i];
        }

        return closest;
    }

    public static BlockPoint ClosestFromList(BlockPoint origin, List<Location> blocks)
    {
        if (blocks == null) { return NULL; }

        int closest = int.MaxValue;
        BlockPoint closestPoint = NULL;

        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].Point == NULL) { continue; }
            int d = ManhattanDistance(origin, blocks[i].Point);

            if (d >= closest) { continue; }

            closest = d;
            closestPoint = blocks[i].Point;
        }

        return closestPoint;
    }

    public BlockPoint IndexToAdjacentDirection(int i)
    {
        if (i > 3)
        {
            i = (i & 3);
        }

        return CardinalFourNeighbors()[i];
    }

    public BlockPoint IndexToCardinalDirection(int i)
    {
        if (i > 7)
        {
            i = (i & 7);
        }

        return CardinalEightNeighbors()[i];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2 ToVector2()
    {
        float worldX = ((float)x * Master.Instance.PixelsPerBlockAxis) + (Master.Instance.PixelsPerBlockAxisHalf);
        float worldY = ((float)y * Master.Instance.PixelsPerBlockAxis) + (Master.Instance.PixelsPerBlockAxisHalf);

        return new Vector2(worldX, worldY);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToVector3()
    {
        float worldX = ((float)x * Master.Instance.PixelsPerBlockAxis) + (Master.Instance.PixelsPerBlockAxisHalf);
        float worldY = ((float)y * Master.Instance.PixelsPerBlockAxis) + (Master.Instance.PixelsPerBlockAxisHalf);
        float worldZ = ((float)z * Master.Instance.PixelsPerBlockAxis) + (Master.Instance.PixelsPerBlockAxisHalf); // Snap to mid.

        return new Vector3(worldX, worldY, -worldZ);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToVector3Top()
    {
        float worldX = ((float)x * Master.Instance.PixelsPerBlockAxis) + (Master.Instance.PixelsPerBlockAxisHalf);
        float worldY = ((float)y * Master.Instance.PixelsPerBlockAxis) + (Master.Instance.PixelsPerBlockAxisHalf);
        float worldZ = ((float)z * Master.Instance.PixelsPerBlockAxis) + (Master.Instance.PixelsPerBlockAxis); // Snap to top.

        return new Vector3(worldX, worldY, -worldZ);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToVector3Bot()
    {
        float worldX = ((float)x * Master.Instance.PixelsPerBlockAxis) + (Master.Instance.PixelsPerBlockAxisHalf);
        float worldY = ((float)y * Master.Instance.PixelsPerBlockAxis) + (Master.Instance.PixelsPerBlockAxisHalf);
        float worldZ = ((float)z * Master.Instance.PixelsPerBlockAxis); // Snap to bottom.

        return new Vector3(worldX, worldY, -worldZ);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToCamVector3()
    {
        Vector3 v = ToVector3Bot();
        return new Vector3((int)v.x, (int)v.y, (int)v.z - CAM_Z * Master.Instance.PixelsPerBlockAxis);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToSurfaceVector3()
    {
        Vector3 v = ToVector3Bot();
        return new Vector3((int)v.x, (int)v.y, (int)v.z - SURFACE_Z * Master.Instance.PixelsPerBlockAxis);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToEmptyBlockMeshVector3()
    {
        Vector3 v = ToVector3Bot();
        return new Vector3((int)v.x, (int)v.y, (int)v.z - EMPTY_BLOCK_MESH_Z * Master.Instance.PixelsPerBlockAxis);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToFXVector3()
    {
        Vector3 v = ToVector3Bot();
        return new Vector3((int)v.x, (int)v.y, (int)v.z - FX_Z * Master.Instance.PixelsPerBlockAxis);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToEntityVector3()
    {
        Vector3 v = ToVector3Bot();
        return new Vector3((int)v.x, (int)v.y, (int)v.z - ENTITY_Z * Master.Instance.PixelsPerBlockAxis);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToFishVector3()
    {
        Vector3 v = ToVector3Bot();
        return new Vector3((int)v.x, (int)v.y, (int)v.z - FISH_Z * Master.Instance.PixelsPerBlockAxis);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 ToEntityShadowVector3()
    {
        Vector3 v = ToVector3Bot();
        return new Vector3((int)v.x, (int)v.y, (int)v.z - ENTITY_SHADOW_Z * Master.Instance.PixelsPerBlockAxis);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 GetDirection(BlockPoint a, BlockPoint b)
    {
        float dist = Distance(a, b);

        if (dist <= float.Epsilon) { return Vector3.zero; }

        return DeltaVector(a, b) / dist;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int DistanceSquared(BlockPoint p1, BlockPoint p2)
    {
        return Mathf.Abs(p1.x - p2.x) * Mathf.Abs(p1.x - p2.x) +
               Mathf.Abs(p1.y - p2.y) * Mathf.Abs(p1.y - p2.y) +
               Mathf.Abs(p1.z - p2.z) * Mathf.Abs(p1.z - p2.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BlockPoint DirectionAsBlockPoint(BlockPoint a, BlockPoint b)
    {
        Vector3 dir = GetDirection(a, b);

        float signX = Mathf.Sign(dir.x);
        float signY = Mathf.Sign(dir.y);
        float signZ = Mathf.Sign(dir.z);

        int dirX = (int)((Mathf.Abs(dir.x) + 0.5f) * signX);
        int dirY = (int)((Mathf.Abs(dir.y) + 0.5f) * signY);
        int dirZ = (int)((Mathf.Abs(dir.z) + 0.5f) * signZ);

        return new BlockPoint(dirX, dirY, dirZ);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 DeltaVector(BlockPoint a, BlockPoint b)
    {
        return new Vector3(b.x - a.x, b.y - a.y, b.z - a.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(BlockPoint a, BlockPoint b)
    {
        Vector3 delta = DeltaVector(a, b);
        return Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y + delta.z * delta.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ManhattanDistance(BlockPoint a, BlockPoint b)
    {
        return Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y) + Mathf.Abs(b.z - a.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ManhattanDistance2D(BlockPoint a, BlockPoint b)
    {
        return Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BlockPoint Delta(BlockPoint a, BlockPoint b)
    {
        return new BlockPoint(Mathf.Abs(b.x - a.x), Mathf.Abs(b.y - a.y), Mathf.Abs(b.z - a.z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InsidePoint(BlockPoint a, BlockPoint b)
    {
        return (a.x >= b.x && a.x <= b.x + 1 && a.y >= b.y && a.y <= b.y + 1 && a.z >= b.z && a.z <= b.z + 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint Clamp(int Width, int Height, int Depth)
    {
        return new BlockPoint(Mathf.Clamp(x, 0, Width), Mathf.Clamp(y, 0, Height), Mathf.Clamp(z, 0, Depth));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Magnitude()
    {
        return Mathf.Sqrt(x * x + y * y + z * z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 Normalize()
    {
        return new Vector3(x, y, z) / Magnitude();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BlockPoint Abs()
    {
        return new BlockPoint(Mathf.Abs(x), Mathf.Abs(y), Mathf.Abs(z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetAdjacentDist(BlockPoint a, BlockPoint b)
    {
        BlockPoint delta = Delta(a, b);
        int max = Mathf.Max(delta.x, delta.y, delta.z);
        return max;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAdjacent(BlockPoint a, BlockPoint b)
    {
        BlockPoint delta = Delta(a, b);
        int max = Mathf.Max(delta.x, delta.y, delta.z);
        return max <= 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAdjacentHorizontal(BlockPoint a, BlockPoint b)
    {
        BlockPoint delta = Delta(a, b);
        int max = Mathf.Max(delta.x, delta.y);
        return max <= 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InRange(BlockPoint a, BlockPoint b, int range)
    {
        return (int)BlockPoint.Distance(a, b) <= range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InPointBorder(BlockPoint a, BlockPoint b)
    {
        BlockPoint delta = b - a;
        return Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y), Mathf.Abs(delta.z)) <= 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BlockPoint GetRandomSkyPoint()
    {
        return GetRandomBlockPoint(0, _session.WorldDimensions.Width, 0, _session.WorldDimensions.Height, _session.WorldDimensions.Depth - 1, _session.WorldDimensions.Depth - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BlockPoint GetRandomBorderPoint(int z)
    {
        int randDir = TinyBeast.Random.Range(0, 4);

        switch (randDir)
        {
            default:
            case 0: // North.
                return new BlockPoint(TinyBeast.Random.Range(0, _session.WorldDimensions.Width), _session.WorldDimensions.Height - 1, z);

            case 1: // East.
                return new BlockPoint(_session.WorldDimensions.Width - 1, TinyBeast.Random.Range(0, _session.WorldDimensions.Height), z);

            case 2: // South.
                return new BlockPoint(TinyBeast.Random.Range(0, _session.WorldDimensions.Width), 0, z);

            case 3: // West.
                return new BlockPoint(0, TinyBeast.Random.Range(0, _session.WorldDimensions.Height), z);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BlockPoint GetRandomWorldXYPoint(int z)
    {
        return GetRandomBlockPoint(0, _session.WorldDimensions.Width, 0, _session.WorldDimensions.Height, z, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BlockPoint GetRandomWorldXYZPoint()
    {
        return GetRandomBlockPoint(0, _session.WorldDimensions.Width, 0, _session.WorldDimensions.Height, 0, _session.WorldDimensions.Depth);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BlockPoint GetRandomBlockPoint(int minX, int maxX, int minY, int maxY, int minZ, int maxZ, bool clampToWorldBounds = true)
    {
        BlockPoint bp = new BlockPoint(TinyBeast.Random.Range(minX, maxX),
            TinyBeast.Random.Range(minY, maxY),
            TinyBeast.Random.Range(minZ, maxZ));

        if (clampToWorldBounds)
            return _session.WorldDimensions.ClampToBounds(bp);
        else
            return bp;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3 GetRandomVectorInsidePoint()
    {
        Vector3 worldPos = ToVector3();
        Vector3 v = new Vector3(worldPos.x + TinyBeast.Random.Range(-Master.Instance.PixelsPerBlockAxisHalf, Master.Instance.PixelsPerBlockAxisHalf),
                                worldPos.y + TinyBeast.Random.Range(-Master.Instance.PixelsPerBlockAxisHalf, Master.Instance.PixelsPerBlockAxisHalf),
                                worldPos.z + TinyBeast.Random.Range(-Master.Instance.PixelsPerBlockAxisHalf, Master.Instance.PixelsPerBlockAxisHalf));

        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BlockPoint GetRandomHorizontalOffsetFromPoint(BlockPoint origin, int min, int max)
    {
        return GetRandomOffsetFromPoint(origin, min, max, min, max, 0, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BlockPoint GetRandomOffsetFromPoint(BlockPoint origin, int min, int max)
    {
        return GetRandomOffsetFromPoint(origin, min, max, min, max, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BlockPoint GetRandomOffsetFromPoint(BlockPoint origin, int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
    {
        BlockPoint bp = new BlockPoint(TinyBeast.Random.Range(minX, maxX + 1),
            TinyBeast.Random.Range(minY, maxY + 1),
            TinyBeast.Random.Range(minZ, maxZ + 1));

        return _session.WorldDimensions.ClampToBounds(bp + origin);
    }

    // an example of initial movement could be the delta of b - a
    public static BlockPoint GetVariedCardinalMovement(BlockPoint initialMovement, int variance)
    {
        return new BlockPoint(initialMovement.x + TinyBeast.Random.Range(-variance, variance), initialMovement.y + TinyBeast.Random.Range(-variance, variance), initialMovement.z);
    }

    public BlockPoint MirrorXY(WorldDimensions dimensions)
    {
        return new BlockPoint(dimensions.Width - this.x, dimensions.Height - this.y, this.z);
    }

    public BlockPoint MirrorXYZ(WorldDimensions dimensions)
    {
        return new BlockPoint(dimensions.Width - this.x, dimensions.Height - this.y, dimensions.Depth - this.z);
    }

    public void RenderLineToPoint(BlockPoint end, Color c, float dur = 1.0f)
    {
#if DEV_TESTING
        Debug.DrawLine(ToVector3(), end.ToVector3(), c, dur);
#endif
    }

    public void Render()
    {
#if DEV_TESTING
        Render(Color.cyan, 1.0f);
#endif
    }

    public void Render(Color c, float size, float dur = 0.01f, bool ignorePause = false)
    {
#if DEV_TESTING
        if (GameManager.Instance == null) { return; }
        if (!ignorePause && _session.GameTime.IsPaused) { return; }
        Vector3 worldPos = ToVector3();
        float r = (float)Master.Instance.PixelsPerBlockAxisHalf * size;
        Vector3 tl1 = new Vector3(worldPos.x - r, worldPos.y + r, worldPos.z - r);
        Vector3 tr1 = new Vector3(worldPos.x + r, worldPos.y + r, worldPos.z - r);
        Vector3 br1 = new Vector3(worldPos.x + r, worldPos.y - r, worldPos.z - r);
        Vector3 bl1 = new Vector3(worldPos.x - r, worldPos.y - r, worldPos.z - r);
        Debug.DrawLine(tl1, tr1, c, dur);
        Debug.DrawLine(tr1, br1, c, dur);
        Debug.DrawLine(br1, bl1, c, dur);
        Debug.DrawLine(bl1, tl1, c, dur);
        Vector3 tl2 = new Vector3(worldPos.x - r, worldPos.y + r, worldPos.z + r);
        Vector3 tr2 = new Vector3(worldPos.x + r, worldPos.y + r, worldPos.z + r);
        Vector3 br2 = new Vector3(worldPos.x + r, worldPos.y - r, worldPos.z + r);
        Vector3 bl2 = new Vector3(worldPos.x - r, worldPos.y - r, worldPos.z + r);
        Debug.DrawLine(tl2, tr2, c, dur);
        Debug.DrawLine(tr2, br2, c, dur);
        Debug.DrawLine(br2, bl2, c, dur);
        Debug.DrawLine(bl2, tl2, c, dur);

        Debug.DrawLine(tl2, tl1, c, dur);
        Debug.DrawLine(tr2, tr1, c, dur);
        Debug.DrawLine(br2, br1, c, dur);
        Debug.DrawLine(bl2, bl1, c, dur);
#endif

    }

    public void RenderX(Color c, float size, float dur = 0.01f, bool ignorePause = false)
    {
#if DEV_TESTING
        if (GameManager.Instance == null) { return; }
        if (!ignorePause && _session.GameTime.IsPaused) { return; }
        Vector3 worldPos = ToVector3();
        float r = (float)Master.Instance.PixelsPerBlockAxisHalf * size;
        Vector3 tl1 = new Vector3(worldPos.x - r, worldPos.y + r, worldPos.z - r);
        Vector3 tr1 = new Vector3(worldPos.x + r, worldPos.y + r, worldPos.z - r);
        Vector3 br1 = new Vector3(worldPos.x + r, worldPos.y - r, worldPos.z - r);
        Vector3 bl1 = new Vector3(worldPos.x - r, worldPos.y - r, worldPos.z - r);
        Vector3 tl2 = new Vector3(worldPos.x - r, worldPos.y + r, worldPos.z + r);
        Vector3 tr2 = new Vector3(worldPos.x + r, worldPos.y + r, worldPos.z + r);
        Vector3 br2 = new Vector3(worldPos.x + r, worldPos.y - r, worldPos.z + r);
        Vector3 bl2 = new Vector3(worldPos.x - r, worldPos.y - r, worldPos.z + r);
        Debug.DrawLine(tl1, br2, c, dur);
        Debug.DrawLine(tr1, bl2, c, dur);
        Debug.DrawLine(br1, tl2, c, dur);
        Debug.DrawLine(bl1, tr2, c, dur);
#endif

    }

    public void RenderPlus(Color c, float size, float dur = 0.01f, bool ignorePause = false)
    {
#if DEV_TESTING
        if (GameManager.Instance == null) { return; }
        if (!ignorePause && _session.GameTime.IsPaused) { return; }
        Vector3 worldPos = ToVector3();
        float r = (float)Master.Instance.PixelsPerBlockAxisHalf * size;
        Vector3 n = new Vector3(worldPos.x, worldPos.y + r, worldPos.z);
        Vector3 e = new Vector3(worldPos.x + r, worldPos.y, worldPos.z);
        Vector3 s = new Vector3(worldPos.x, worldPos.y - r, worldPos.z);
        Vector3 w = new Vector3(worldPos.x - r, worldPos.y, worldPos.z);
        Vector3 u = new Vector3(worldPos.x, worldPos.y, worldPos.z + r);
        Vector3 d = new Vector3(worldPos.x, worldPos.y, worldPos.z - r);
        Debug.DrawLine(n, s, c, dur);
        Debug.DrawLine(w, e, c, dur);
        Debug.DrawLine(u, d, c, dur);
#endif

    }

    public void RenderMinus(Color c, float size, float dur = 0.01f, bool ignorePause = false)
    {
#if DEV_TESTING
        if (GameManager.Instance == null) { return; }
        if (!ignorePause && _session.GameTime.IsPaused) { return; }
        Vector3 worldPos = ToVector3();
        float r = (float)Master.Instance.PixelsPerBlockAxisHalf * size;
        Vector3 n = new Vector3(worldPos.x, worldPos.y + r, worldPos.z);
        Vector3 e = new Vector3(worldPos.x + r, worldPos.y, worldPos.z);
        Vector3 s = new Vector3(worldPos.x, worldPos.y - r, worldPos.z);
        Vector3 w = new Vector3(worldPos.x - r, worldPos.y, worldPos.z);
        Vector3 u = new Vector3(worldPos.x, worldPos.y, worldPos.z + r);
        Vector3 d = new Vector3(worldPos.x, worldPos.y, worldPos.z - r);
        //Debug.DrawLine(n, s, c, dur);
        Debug.DrawLine(w, e, c, dur);
        //Debug.DrawLine(u, d, c, dur);
#endif

    }

#endif

    #region OVERRIDES

    public override bool Equals(System.Object obj)
    {
        return obj is BlockPoint && this == (BlockPoint)obj;
    }

    public override int GetHashCode()
    {
        //return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
        unchecked
        {
            // +1 to account for the -1, -1, -1 
            //return ((x + 1) << 18) | ((y + 1) << 9) | (z + 1);
            int hash = x + 1;
            hash = (hash << 9) | (y + 1);  // Concatenate Y to the hash
            hash = (hash << 9) | (z + 1);  // Concatenate Z to the hash

            // Mixing step (optional, depending on range and distribution)
            // hash ^= hash >> 16;
            // hash *= 0x85ebca6b;
            // hash ^= hash >> 13;
            // hash *= 0xc2b2ae35;
            // hash ^= hash >> 16;

            return hash;
        }
    }

    public int GetNoiseXY()
    {
        unchecked
        {
            uint i = (uint)(((x * 1028385129 + y * 945191568 + 2015177) * 141650963) ^ 1028385129);
            i = ((i >> 16) ^ i) * 4256233;
            i = ((i >> 16) ^ i) * 4256249;
            i = ((i >> 16) ^ i);
            return (int)i;
        }
    }

    public int GetNoiseXYZ()
    {
        unchecked
        {
            uint i = (uint)(((x * 1028385129 + y * 945191568 + 2015177) * 141650963 + y * z) ^ 1028385129);
            i = ((i >> 16) ^ i) * 4256233;
            i = ((i >> 16) ^ i) * 4256249;
            i = ((i >> 16) ^ i);
            return (int)i;
        }
    }

    public override string ToString()
    {
        return $"{x}x, {y}y, {z}z";
        //return $"<color=#963232>{x}</color>x, <color=#32963F>{y}</color>y, <color=#0c79e3>{z}</color>z";
    }

    public bool Equals(BlockPoint x, BlockPoint y)
    {
        return x.x == y.x && x.y == y.y && x.z == y.z;
    }

    public int GetHashCode(BlockPoint obj)
    {
        return obj.x.GetHashCode() ^ obj.y.GetHashCode() << 2 ^ obj.z.GetHashCode() >> 2;
    }

    #endregion

    #region OPERATOR OVERLOADS
    public static bool operator ==(BlockPoint a, BlockPoint b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator !=(BlockPoint a, BlockPoint b)
    {
        return !(a == b);
    }

    public static BlockPoint operator +(BlockPoint a, BlockPoint b)
    {
        return new BlockPoint(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static BlockPoint operator -(BlockPoint a, BlockPoint b)
    {
        return new BlockPoint(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static BlockPoint operator *(BlockPoint a, float b)
    {
        return new BlockPoint(a.x * b, a.y * b, a.z * b);
    }

    public static BlockPoint operator /(BlockPoint a, float b)
    {
        return new BlockPoint(a.x / b, a.y / b, a.z / b);
    }
    #endregion

    public static readonly BlockPoint NORTH = new BlockPoint(0, 1, 0);
    public static readonly BlockPoint NORTH_EAST = new BlockPoint(1, 1, 0);
    public static readonly BlockPoint EAST = new BlockPoint(1, 0, 0);
    public static readonly BlockPoint SOUTH_EAST = new BlockPoint(1, -1, 0);
    public static readonly BlockPoint SOUTH = new BlockPoint(0, -1, 0);
    public static readonly BlockPoint SOUTH_WEST = new BlockPoint(-1, -1, 0);
    public static readonly BlockPoint WEST = new BlockPoint(-1, 0, 0);
    public static readonly BlockPoint NORTH_WEST = new BlockPoint(-1, 1, 0);

    public static readonly BlockPoint UP = new BlockPoint(0, 0, 1);
    public static readonly BlockPoint DOWN = new BlockPoint(0, 0, -1);

    public static readonly BlockPoint UP_NORTH = new BlockPoint(0, 1, 1);
    public static readonly BlockPoint UP_NORTH_EAST = new BlockPoint(1, 1, 1);
    public static readonly BlockPoint UP_EAST = new BlockPoint(1, 0, 1);
    public static readonly BlockPoint UP_SOUTH_EAST = new BlockPoint(1, -1, 1);
    public static readonly BlockPoint UP_SOUTH = new BlockPoint(0, -1, 1);
    public static readonly BlockPoint UP_SOUTH_WEST = new BlockPoint(-1, -1, 1);
    public static readonly BlockPoint UP_WEST = new BlockPoint(-1, 0, 1);
    public static readonly BlockPoint UP_NORTH_WEST = new BlockPoint(-1, 1, 1);

    public static readonly BlockPoint DOWN_NORTH = new BlockPoint(0, 1, -1);
    public static readonly BlockPoint DOWN_NORTH_EAST = new BlockPoint(1, 1, -1);
    public static readonly BlockPoint DOWN_EAST = new BlockPoint(1, 0, -1);
    public static readonly BlockPoint DOWN_SOUTH_EAST = new BlockPoint(1, -1, -1);
    public static readonly BlockPoint DOWN_SOUTH = new BlockPoint(0, -1, -1);
    public static readonly BlockPoint DOWN_SOUTH_WEST = new BlockPoint(-1, -1, -1);
    public static readonly BlockPoint DOWN_WEST = new BlockPoint(-1, 0, -1);
    public static readonly BlockPoint DOWN_NORTH_WEST = new BlockPoint(-1, 1, -1);

}