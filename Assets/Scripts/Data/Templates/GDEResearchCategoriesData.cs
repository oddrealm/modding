using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/ResearchCategories")]
public class GDEResearchCategoriesData : Scriptable
{
    public bool Hide = false;
    public string FriendlyName = "";
    public int OrderInList = 0;
    public string[] RaceRequirements = new string[] { };
    public HashSet<string> RaceRequirementsHash { get; private set; } = new HashSet<string>();

    [System.Serializable]
    public struct ResearchBuff
    {
        public string TagObjectID;
    }

    [Header("Having these in the settlement reduces the research time")]
    public ResearchBuff[] ResearchBuffs = new ResearchBuff[0];

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();

        for (int i = 0; i < RaceRequirements.Length; i++)
        {
            RaceRequirementsHash.Add(RaceRequirements[i]);
        }
    }
#endif
}
