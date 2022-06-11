using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EntityIntelligenceTypes
{
    NONE = 0,
    SAPIENT = 1,
    NON_SAPIENT = 2,

    ALL = SAPIENT | NON_SAPIENT
}
