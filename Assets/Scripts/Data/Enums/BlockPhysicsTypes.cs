using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum BlockPhysicsTypes
{
    NONE = 0,
    LIQUID = 1,
    GRAVITY = 2,
    GAS = 4,
    FIRE = 8,
    MELT = 16,
    FREEZE = 32,
    VOLATILE = 64,
    FLOAT = 128,
    SOLUBLE = 256,
    HEAT_SOURCE = 512,
    HEAT_TRANSFER = 1024,
    HEAT_SINK = 2048,
    ITEM_CACHE = 4096,

    MOVEMENT = LIQUID | GRAVITY | GAS | FIRE | FLOAT
}