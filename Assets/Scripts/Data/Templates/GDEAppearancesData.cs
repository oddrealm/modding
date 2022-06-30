using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Appearances")]
public class GDEAppearancesData : ScriptableObject
{
	public string Key;
	public string Gender = "";
	public string Race = "";
	public string Group = "";
	public int Iteration = 0;
	public EntityAppearanceTypes AppearanceType = EntityAppearanceTypes.ABE;
	public EntityAgeTypes AgeType = EntityAgeTypes.ADULT;
	public ShadowTypes ShadowType = ShadowTypes.MEDIUM;
	public string Portrait = "";
	public float HealthBarOffset = 0.0f;
	public string StealthSightingDialogueGroup = "";
	public List<string> IdleFX = new List<string>();
	public List<string> SleepFX = new List<string>();
	public List<string> DeathFX = new List<string>();
	public List<string> Accessories = new List<string>();
	public string KilledSFX = "";
	public string HitSFX = "";
	public string SelectPositiveSFX = "";
	public string SelectNegativeSFX = "";
	public string OrderPositiveSFX = "";
	public string OrderNegativeSFX = "";
	
	public GDECharacterColorMaskData ColorMask;
	public GDEAnimationSetGroupData Animations;
}
