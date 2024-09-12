using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum SelectionTargetTypes
{
    NONE = 0,
    ENTITY = 1,
    FISH = 2,
    LOCATION = 4,
    ROOM_CLUSTER = 8,
    OVERWORLD_PARTY = 16,
    OVERWORLD_TILE = 32,
    JOB = 64,
    OVERWORLD_NATION = 128,
    OVERWORLD_SETTLEMENT = 256,

    ENTITY_FISH_TARGETS = ENTITY | FISH,
    WORLD_POINT_TARGETS = LOCATION | ROOM_CLUSTER | JOB,
    ALL = ENTITY | FISH | LOCATION | ROOM_CLUSTER | JOB
}