using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResearchCategories")]
public class GDEResearchCategoriesData : ScriptableObject
{
	public string Key;
	public int ResearchCategory = 0;
	public string FriendlyName = "";
}
