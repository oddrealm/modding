using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum FactionTypes
{
    NONE = 0,
    PLAYER = 1,
    NEUTRAL = 2,
    HOSTILE = 4,
    CAPTURED = 8,
    ALL = PLAYER | NEUTRAL | HOSTILE | CAPTURED
}
