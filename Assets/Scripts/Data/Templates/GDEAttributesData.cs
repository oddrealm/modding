using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attributes")]
public class GDEAttributesData : Scriptable
{
	public bool ShowInAttributeDisplay = true;
	public int StartMin = 0;
	public int StartDefault = 0;
	public int StartMax = 0;
	public bool StartMaxIsStartBase = false;
	public bool ShowPositiveAndNegativeSign = false;
	public AttributeDisplayTypes DisplayType = AttributeDisplayTypes.AMOUNT;
	public AttributeColorTypes ColorType = 0;
	public bool NegativeIsPositive = false;
	public string OnAddIndicator = "";
	public string OnRemoveIndicator = "";
	public bool ShowActionText = false;
}
