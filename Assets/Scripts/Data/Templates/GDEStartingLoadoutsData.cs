using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StartingLoadouts")]
public class GDEStartingLoadoutsData : Scriptable
{
    public bool Public = false;
    public bool CanEdit = false;
    public string EntitySpawnGroupID = "";
    public string Items = "";
    public List<string> ContainerIDs = new List<string>();
}
