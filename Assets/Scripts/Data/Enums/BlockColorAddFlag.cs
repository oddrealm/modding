using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum BlockColorAddFlag
{
    NONE = 0,
    MULTIPLY = 1,
    RED_MASK = 2,
    GREEN_MASK = 4,
    BLUE_MASK = 8,
}