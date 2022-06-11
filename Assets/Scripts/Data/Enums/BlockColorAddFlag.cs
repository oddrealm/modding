using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum BlockColorAddFlag
{
    NONE = 0,
    MULT = 1,
    RED_REPLACE = 2,
    GREEN_REPLACE = 4,
    BLUE_REPLACE = 8,
}