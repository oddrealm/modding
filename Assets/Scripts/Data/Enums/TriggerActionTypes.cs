using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum TriggerActionTypes
{
    NONE = 0,
    DROP_ENTITY = 1,
    CAPTURE_SMALL = 2,
    CAPTURE_MEDIUM = 4,
    CAPTURE_LARGE = 8,
    ATTACK_USE_RANDOM = 16,
    EXPLODE = 32,
    ATTACK_USE_ALL = 64,
    ADD_STATUSES = 128,

    ATTACK = ATTACK_USE_ALL | ATTACK_USE_RANDOM
}
