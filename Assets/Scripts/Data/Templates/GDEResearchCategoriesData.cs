using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResearchCategories")]
public class GDEResearchCategoriesData : Scriptable
{
	public string FriendlyName = "";
	public int OrderInList = 0;

    [System.Serializable]
    public struct ResearchBuff
    {
        public string TagObjectID;
    }

    [Header("Having these in the settlement reduces the research time")]
    public ResearchBuff[] ResearchBuffs = new ResearchBuff[0];
}
