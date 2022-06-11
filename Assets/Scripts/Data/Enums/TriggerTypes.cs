using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum TriggerTypes
{
    NONE = 0,
    ON_ENTITY_ENTER = 1,
    ON_INTERACT = 2,
    //ON_VISIBLE = 4,
    ON_LOOT = 8,
    ON_ENTITY_CAUGHT = 16,
    ON_HAS_ITEMS = 32
}