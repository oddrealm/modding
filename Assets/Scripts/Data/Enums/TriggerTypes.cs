using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerTypes
{
    NONE = 0,
    ON_ENTITY_ENTER = 1,
    ON_ENTITY_EXIT = 2,
    ON_INTERACT = 3,
    ON_WORK_FINISHED = 4,
    ON_NEIGHBOR = 5,
    ON_NEIGHBOR_FAIL = 6,
    ON_TIME = 7,
    ON_SPAWN = 8,
    ON_WORK_FORCE_ACTIVATE = 9,
    ON_ITEM_ADD = 10,
    ON_ITEM_REMOVE = 11,
    ON_BLOCK_REMOVED = 12
}