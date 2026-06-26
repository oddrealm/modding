using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dungeon")]
public class GDEDungeonData : Scriptable
{
    [System.Serializable]
    public struct PostGenSpawn
    {
        public string TagID;
        public string TagObjID;
        public RandomChance Chance;
        public int MinCount;
        public int MaxCount;
    }

    [System.Serializable]
    public struct Requirement
    {
        public string RaceID;
        public bool MustNotHave;
    }

    public Requirement[] Requirements;
    public RandomChance RoomSpawnChances = new RandomChance();
    public string RoomTagID;
    public string CorridorTagID;
    public string WallTagID;
    public PostGenSpawn[] PostGenSpawns;

    [HideInInspector]
    public TagUID RoomTagUID;
    [HideInInspector]
    public TagUID CorridorTagUID;
    [HideInInspector]
    public TagUID WallTagUID;

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();

        if (!string.IsNullOrEmpty(RoomTagID))
        {
            RoomTagUID = DataManager.GetTagObject<GDETagsData>(RoomTagID).TagUID;
        }

        if (!string.IsNullOrEmpty(CorridorTagID))
        {
            CorridorTagUID = DataManager.GetTagObject<GDETagsData>(CorridorTagID).TagUID;
        }

        if (!string.IsNullOrEmpty(WallTagID))
        {
            WallTagUID = DataManager.GetTagObject<GDETagsData>(WallTagID).TagUID;
        }
    }
#endif
}
