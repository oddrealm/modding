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

    public Dictionary<string, int> StartingEntityCountsByID { get; private set; } = new();
    public List<string> StatusesInSpawnData { get; private set; } = new();

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();
        StatusesInSpawnData.Clear();
        StartingEntityCountsByID.Clear();

        if (!string.IsNullOrEmpty(EntitySpawnGroupID))
        {
            GDEEntitySpawnGroupsData.Spawn[] entitySpawns = DataManager.GetEntitySpawnGroupSchemas(EntitySpawnGroupID);

            for (int i = 0; entitySpawns != null && i < entitySpawns.Length; i++)
            {
                if (entitySpawns[i].EntityCount <= 0) { continue; }

                if (StartingEntityCountsByID.ContainsKey(entitySpawns[i].EntityId))
                {
                    StartingEntityCountsByID[entitySpawns[i].EntityId] += entitySpawns[i].EntityCount;
                }
                else
                {
                    StartingEntityCountsByID.Add(entitySpawns[i].EntityId, entitySpawns[i].EntityCount);
                }

                if (entitySpawns[i].AddedStatuses.Count > 0)
                {
                    for (int j = 0; j < entitySpawns[i].AddedStatuses.Count; j++)
                    {
                        if (!StatusesInSpawnData.Contains(entitySpawns[i].AddedStatuses[j]))
                        {
                            StatusesInSpawnData.Add(entitySpawns[i].AddedStatuses[j]);
                        }
                    }
                }
            }
        }
    }
#endif
}
