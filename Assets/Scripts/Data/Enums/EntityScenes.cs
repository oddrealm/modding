using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EntityScenes
{
    NONE = 0,
    IN_TILE = 1,
    IN_PARTY = 2,

    ACTIVE = IN_TILE | IN_PARTY
}