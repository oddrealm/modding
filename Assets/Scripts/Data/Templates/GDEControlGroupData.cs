using Assets.GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ControlGroup")]
public class GDEControlGroupData : Scriptable
{
    public int KeyIndex;
    public string InputID;
}
