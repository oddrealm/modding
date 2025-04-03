using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StartingLoadouts", order = 0)]
public class GDEStartingLoadoutsData : Scriptable
{
    public bool Public = false;
    public bool CanEdit = false;
    public string EntitySpawnGroupID = "";
    public string Items = "";
    public List<string> ContainerIDs = new List<string>();
    public int StartingRen = 100;
}
