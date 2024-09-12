
[System.Flags]
public enum BlockDirectionFlags
{
    NONE,
    NORTH = 1,
    EAST = 2,
    SOUTH = 4,
    WEST = 8,
    UP = 16,
    DOWN = 32,
    NORTH_EAST = NORTH | EAST,
    SOUTH_EAST = SOUTH | EAST,
    SOUTH_WEST = SOUTH | WEST,
    NORTH_WEST = NORTH | WEST,
    HORIZONTAL = NORTH | EAST | SOUTH | WEST,
    VERTICAL = UP | DOWN,
    ALL = NORTH | EAST | SOUTH | WEST | UP | DOWN
}
