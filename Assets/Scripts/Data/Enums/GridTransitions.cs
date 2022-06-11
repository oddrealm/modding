using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum GridTransitions
{
    NONE = 0,
    NORTH_WEST = 1,
    NORTH = 2,
    NORTH_EAST = 4,
    WEST = 8,
    EAST = 16,
    SOUTH_WEST = 32,
    SOUTH = 64,
    SOUTH_EAST = 128,
    ALL = NORTH_WEST | NORTH | NORTH_EAST | WEST | EAST | SOUTH_WEST | SOUTH | SOUTH_EAST
}