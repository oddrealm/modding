using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Profession")]
public class GDEProfessionData : ScriptableObject
{
	public string Key { get { return name; } }
	public string ResearchKey = "";
	public string TooltipID = "";
	public bool VisibleToPlayer = false;
	public string GlobalIndicator = "";
	public SkillTypes SkillsActiveByDefault;
	public SkillTypes SkillsToIncreaseOnLevelUp;
	public ProfessionTypes ProfessionType;
	public List<float> Color = new List<float>();
}
