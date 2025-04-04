using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TagObjectSpawn")]
public class GDETagObjectSpawnData : Scriptable
{
    public int SeedOffset;
    public int MaxIterations = 1;
    public int MinIterations = 1;

    public TagObjectSpawn[] Spawns;

    public WeightedSpawns SpawnWeights = new WeightedSpawns();

#if ODD_REALM_APP
    public override void OnLoaded()
    {
#if DEV_TESTING
        if (DEBUG)
        {
            int j = 0;
        }

        if (SeedOffset == 0)
        {
            SeedOffset = Random.Range(0, 1000000);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
#endif
        SpawnWeights.Rebuild(Spawns);

        base.OnLoaded();
    }
#endif
}

[System.Serializable]
public struct TagObjectSpawn
{
    public string Comment;
    public int MinCount;
    public int MaxCount;
    public bool LimitMaxCountByLocationItemMax;
    public bool DisableRoll;
    public float Weight;
    public string TagObjectID;
    public string TagID;
    [HideInInspector]
    public TagUID TagUID;

    public bool IsNULL
    {
        get
        {
            return string.IsNullOrEmpty(TagID) && string.IsNullOrEmpty(TagObjectID);
        }
    }
}

public class WeightedSpawns
{
    public bool FillEmptyWeightWithNULL = true;

    [HideInInspector]
    public int TotalWeight;

    [HideInInspector]
    public List<int> SpawnWeights = new List<int>();

    [HideInInspector]
    public List<TagObjectSpawn> Spawns = new List<TagObjectSpawn>();

    [HideInInspector]
    public List<TagObjectSpawn> GuaranteedSpawns = new List<TagObjectSpawn>();

    private List<TagObjectSpawn> _rollResultsCache = new List<TagObjectSpawn>();

#if ODD_REALM_APP
    public List<TagObjectSpawn> RollRandomPool()
    {
        _rollResultsCache.Clear();

        _rollResultsCache.AddRange(GuaranteedSpawns);

        for (int i = 0; i < Spawns.Count; i++)
        {
            int roll = TinyBeast.Random.Range(0, TotalWeight);
            TagObjectSpawn randomSpawn = GetSpawnFromRoll(roll);

            _rollResultsCache.Add(randomSpawn);
        }

        return _rollResultsCache;
    }

    public TagObjectSpawn GetSpawnFromRoll(int roll)
    {
        if (Spawns.Count == 0)
        {
            return new TagObjectSpawn();
        }

        if (Spawns.Count == 1)
        {
            return Spawns[0];
        }

        if (roll >= TotalWeight)
        {
            return Spawns[Spawns.Count - 1];
        }

        int index = SpawnWeights.BinarySearch(roll);

        if (index < 0)
        {
            // BinarySearch returns the complement of the index if the value isn't exactly found,
            // which is the next higher value in the list, exactly what we need.
            index = ~index;
        }

        TagObjectSpawn spawn = Spawns[index];

        return spawn;
    }

    public void Rebuild(TagObjectSpawn[] spawns)
    {
        TotalWeight = 0;
        Spawns.Clear();
        SpawnWeights.Clear();
        GuaranteedSpawns.Clear();

        float totalWeightNormal = 0f;

        for (int i = 0; spawns != null && i < spawns.Length; i++)
        {
            if (spawns[i].DisableRoll)
            {
                if (!string.IsNullOrEmpty(spawns[i].TagID))
                {
                    ITag tagData = DataManager.GetTagData(spawns[i].TagID);
                    spawns[i].TagUID = tagData.TagUID;
#if DEV_TESTING
                    if (tagData.TagID == "tag_missing")
                    {
                        Debug.LogError($"Missing TagUID!");
                    }
#endif
                }

                GuaranteedSpawns.Add(spawns[i]);
                continue;
            }

            TagObjectSpawn spawn = new TagObjectSpawn();

            if (!string.IsNullOrEmpty(spawns[i].TagID))
            {
                ITag tagData = DataManager.GetTagData(spawns[i].TagID);
                spawns[i].TagUID = tagData.TagUID;
#if DEV_TESTING
                if (tagData.TagID == "tag_missing")
                {
                    Debug.LogError($"Missing TagUID!");
                }
#endif
                spawn = spawns[i];
            }
            else if (!string.IsNullOrEmpty(spawns[i].TagObjectID))
            {
                spawn = spawns[i];
            }

            totalWeightNormal += spawns[i].Weight;
            int w = Mathf.Max(1, (int)(spawns[i].Weight * 100000));
            TotalWeight += w;
            Spawns.Add(spawn);
            SpawnWeights.Add(TotalWeight);
        }

        // If the total weight is less than 1, fill the rest with NULL to pad the pool.
        if (totalWeightNormal < (1f - float.Epsilon) && FillEmptyWeightWithNULL)
        {
            TagObjectSpawn nullSpawn = new TagObjectSpawn();
            nullSpawn.TagID = "";
            nullSpawn.TagUID = TagUID.NULL;
            nullSpawn.Weight = 1f - totalWeightNormal;
            nullSpawn.MinCount = 1;
            nullSpawn.MaxCount = 1;
            int w = Mathf.Max(1, (int)(nullSpawn.Weight * 100000));
            TotalWeight += w;
            Spawns.Add(nullSpawn);
            SpawnWeights.Add(TotalWeight);
        }
    }
#endif
}