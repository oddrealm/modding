using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Realm/OverworldLodeSpawns", order = 34)]
public class GDEOverworldLodeSpawnsData : Scriptable
{
    public int Shape = 0;
    public int SpawnCount = 0;
    public int MinZ = 0;
    public int MaxZ = 0;
    public List<string> Composition = new List<string>();
}
