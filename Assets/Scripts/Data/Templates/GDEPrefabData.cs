using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Prefab")]
public class GDEPrefabData : Scriptable
{
    public struct PrefabPaint
    {
        public uint LocationUID;
        public LocationData Data;
    }

    [System.Serializable]
    public struct LocationData
    {
        public string TagObjectID;
        public string TagObjectID0;
        public string TagObjectID1;
        public string TagObjectID2;

        public int X;
        public int Y;
        public int Z;

        public BlockPoint Point { get { return new BlockPoint(X, Y, Z); } }
        public bool HasTagObj { get { return !string.IsNullOrEmpty(TagObjectID); } }
    }

    [System.Serializable]
    public class Column
    {
        public LocationData[] Locations;
    }

    [System.Serializable]
    public class PrefabLayer
    {
        public Column[] Columns;
    }


    public int SeedOffset = 0;
    public bool UseRandomRotation = true;

    public LocationData[] Locations = new LocationData[] { };

#if ODD_REALM_APP
    public BlockPoint OriginOffset { get; private set; }

    public override void OnLoaded()
    {
        base.OnLoaded();

        if (Locations == null || Locations.Length == 0)
        {
            Debug.LogError($"PrefabData {Key} has no locations.");
        }

#if DEV_TESTING
        if (DEBUG)
        {
            int j = 0;
        }

        if (SeedOffset == 0)
        {
            SeedOffset = Random.Range(0, 1000000);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        int minX = 8;
        int minY = 8;
        int minZ = 8;

        for (int i = 0; i < Locations.Length; i++)
        {
            LocationData l = Locations[i];

            if (l.Z < minZ ||
                (l.Z == minZ && l.Y < minY) ||
                (l.Z == minZ && l.Y == minY && l.X < minX))
            {
                minX = l.X;
                minY = l.Y;
                minZ = l.Z;
            }
        }

        OriginOffset = new BlockPoint(minX, minY, minZ);
    }
#endif
}
