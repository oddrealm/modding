using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgressionObject : ITooltipContent
{
    // If true, this object will show up as "discovered" in the progression menu and on notifications.
    bool CanShowInProgressUI { get; }
}
