using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Faction")]
public class GDEFactionData : ScriptableObject
{
    public string Key;

    public FactionTypes FactionType = FactionTypes.NONE;
    public FactionTypes EnemyFactionType = FactionTypes.NONE;
    public bool IsPlayerControlled = false;
    public bool CanRespec = false;
    public string TooltipID = "";
    public Color SelectionColor = Color.white;
    public Color CursorHoverColor = Color.white;
    //public bool LimitMovementToRooms = false;
}
