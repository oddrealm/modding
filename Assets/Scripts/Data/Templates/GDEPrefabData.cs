using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Prefab")]
public class GDEPrefabData : Scriptable
{
    public readonly struct PrefabPaint
    {
        public readonly uint locationUID;
        public readonly LocationData data;

        public PrefabPaint(uint locationUID, LocationData data)
        {
            this.locationUID = locationUID;
            this.data = data;
        }
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
    public Vector3Int Buffer = new();
    public LocationData[] Locations = new LocationData[] { };

#if ODD_REALM_APP
    public BlockPoint OriginOffset { get; private set; }
    public BlockPoint Size { get; private set; }
    public BlockPoint MinBounds { get; private set; }
    public BlockPoint MaxBounds { get; private set; }

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
            // Breakpoint.
        }

        if (SeedOffset == 0)
        {
            SeedOffset = Random.Range(0, 1000000);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
#endif
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int minZ = int.MaxValue;
        Size = BlockPoint.ZERO;
        MinBounds = BlockPoint.ZERO;
        MaxBounds = BlockPoint.ZERO;
        OriginOffset = BlockPoint.ZERO;

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

            if (l.X > Size.x) { Size = new(l.X, Size.y, Size.z); }
            if (l.Y > Size.y) { Size = new(Size.x, l.Y, Size.z); }
            if (l.Z > Size.z) { Size = new(Size.x, Size.y, l.Z); }
        }

        OriginOffset = new BlockPoint(minX, minY, minZ);
        MinBounds = BlockPoint.ZERO - new BlockPoint(Buffer.x, Buffer.y, 0);
        MaxBounds = Size + new BlockPoint(Buffer.x, Buffer.y, 0);
    }
#endif
}
