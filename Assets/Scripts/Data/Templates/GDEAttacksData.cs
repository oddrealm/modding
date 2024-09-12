using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks")]
public class GDEAttacksData : Scriptable
{
	public bool ShowOnAttacksDisplay = false;
	public int ActivationWeight = 0;
	public ProjectileTypes ProjectileType;
	public FiringTypes FiringType;
	public AttackActivationTypes ActivationType;
	public DamageTypes DamageType;
	public bool AttackFishInRange = false;

	[Header("Output")]
	//public SkillTypes SkillUsed = SkillTypes.NONE;
	public string SkillID = "";
	public bool AddSkillToOutput = true;
	public int MinAmount;
	public int MaxAmount;
	public int Range = 14;
	public bool IsRanged { get { return ProjectileType != ProjectileTypes.NONE; } }
	public List<string> StatusesToApplyToTarget = new List<string>();

	[Header("Source")]
	public AttackActionTypes SourceAction = AttackActionTypes.ADD;
	//public AttributeTypes SourceAttribute = AttributeTypes.NONE;
	public string SourceAttributeID;

	[Header("Target")]
	public AttackActionTypes TargetAction = AttackActionTypes.REMOVE;
	//public AttributeTypes TargetAttribute = AttributeTypes.HEALTH;
	public string TargetAttributeID;
	public int TargetAmountMin = 0;
	public int TargetAmountMax = int.MaxValue;

	[Header("Visuals")]
	public EntityAnimationTriggers AnimationTrigger = EntityAnimationTriggers.MELEE_START;
	public EntityAnimationEvents AnimationHitEvent = EntityAnimationEvents.ATTACK_HIT;
	public List<string> OriginFX = new List<string>();
	public List<string> HitFX = new List<string>();
	public List<string> PrimeFX = new List<string>();

	public string FiredSFX;
	public string CompletedSFX;
	public string BlockedSFX;

	public Color AttackStartTargetColor;
	public float AttackStartTargetColorDuration = 0f;
	public EntityFlashTypes AttackStartTargetColorFlashType = EntityFlashTypes.OUT;
	public Color AttackStartSourceColor;
	public float AttackStartSourceColorDuration = 0f;
	public EntityFlashTypes AttackStartSourceColorFlashType = EntityFlashTypes.OUT;

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();

		if (string.IsNullOrEmpty(SkillID))
		{
			Debug.LogError($"{Key} is missing skill!");
			SkillID = "skill_misc";
		}

		if (string.IsNullOrEmpty(TargetAttributeID))
		{
			//Debug.LogError($"{Key} is missing target attribute!");
		}
    }
#endif
}
