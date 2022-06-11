using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attributes")]
public class GDEAttributesData : ScriptableObject
{
	public string Key { get { return name; } }
	public string TooltipID = "";
	public int DisplayOrder = 0;
	public int StartMin = 0;
	public int StartDefault = 0;
	public int StartMax = 0;
	public bool ShowPositiveAndNegativeSign = false;
	public string Append = "";
	public AttributeDisplayTypes SmallDisplayType = 0;
	public AttributeDisplayTypes LargeDisplayType = 0;
	public AttributeColorTypes ColorType = 0;
	public AttributeTypes AttributeType = 0;
	public bool NegativeIsPositive = false;
	public string OnAddIndicator = "";
	public string OnRemoveIndicator = "";
}
