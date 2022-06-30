using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/InterfaceInfo")]
public class GDEInterfaceInfoData : ScriptableObject
{
	public string Key;
	public string OrderKey 
	{
		get 
		{
			return Category + SubCategory + Resource + Group + Unique; 
		} 
	}
	public string Display
	{
		get
		{
			return "[" + Category + "][" + SubCategory + "][" + Resource + "][" + Group + "][" + Unique + "]";
		}
	}
	public int Index = 0;
	public string Category = "A_none";
	public string SubCategory = "00";
	public string Resource = "";
	public string Group = "";
	public string Unique = "";

	public void Clone(GDEInterfaceInfoData other)
    {
		Category = other.Category;
		SubCategory = other.SubCategory;
		Resource = other.Resource;
		Group = other.Group;
		Unique = other.Unique;
	}
}
