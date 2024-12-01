using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cave")]
public class GDECaveData : Scriptable
{
    public bool FillUnusedWithSurroundingBlocks;

    [Header("Terrain Layers")]
    public GDEBiomesData.TerrainLayer[] Layers;

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        for (int i = 0; Layers != null && i < Layers.Length; i++)
        {
            if (!string.IsNullOrEmpty(Layers[i].TagID))
            {
                Layers[i].TagUID = DataManager.GetTagObject<GDETagsData>(Layers[i].TagID).TagUID;
            }
        }

        base.OnLoaded();
    }
#endif
}
