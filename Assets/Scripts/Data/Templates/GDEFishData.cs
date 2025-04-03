using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Entity/Animal/Fish", order = 15)]
public class GDEFishData : Scriptable
{
    public int Level = 0;
    public string AnimController = "";
    public int IdealDepthMin = 0;
    public int IdealDepthmax = 0;
    public int BaseHealth = 0;
    public int BaseToughness = 0;
    public int BaseEvasion = 0;
    public string ItemSpawnGroupID = "";
    public Color FishColorR;
    public Color FishColorG;
    public Color FishColorB;

    public List<string> Biomes = new List<string>();
    public HashSet<string> BiomesHash = new HashSet<string>();

#if ODD_REALM_APP
    public override void Init()
    {
        BiomesHash.Clear();

        for (int i = 0; i < Biomes.Count; i++)
        {
            BiomesHash.Add(Biomes[i]);
        }

        base.Init();
    }
#endif
}
