using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum InputStateTypes
{
    NONE = 0,
    SYSTEM = 1,
    CAMERA = 2,
    HUD_INPUT = 4,
    OVERWORLD_INPUT = 8,
    TIME = 16,
    SELECTION_INPUT = 32,
    SEARCH = 64,
    NON_SYS = CAMERA | HUD_INPUT /*| SELECTION_INPUT*/ | OVERWORLD_INPUT | TIME | SEARCH,
    ALL = SYSTEM | CAMERA | HUD_INPUT | SELECTION_INPUT | OVERWORLD_INPUT | TIME | SEARCH
}