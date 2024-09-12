using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EntityAgeTypes
{
    NONE = 0,
    CHILD = 1,
    ADULT = 2,
    ELDER = 4,
    ALL = CHILD | ADULT | ELDER
}