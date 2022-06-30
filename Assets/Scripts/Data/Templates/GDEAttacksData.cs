using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attacks")]
public class GDEAttacksData : ScriptableObject
{
	public string Key;
	public string TooltipID = "";
	public bool ShowOnAttacksDisplay = false;
	public int ActivationWeight = 0;
	public ProjectileTypes ProjectileType;
	public FiringTypes FiringType;
	public AttackActivationTypes ActivationType;
	public DamageTypes DamageType;
	public bool AttackFishInRange = false;

	[Header("Output")]
	public SkillTypes SkillUsed = SkillTypes.NONE;
	public bool AddSkillToOutput = true;
	public int MinAmount;
	public int MaxAmount;
	public int Range = 8;

	[Header("Source")]
	public AttackActionTypes SourceAction = AttackActionTypes.ADD;
	public AttributeTypes SourceAttribute = AttributeTypes.NONE;

	[Header("Target")]
	public AttackActionTypes TargetAction = AttackActionTypes.REMOVE;
	public AttributeTypes TargetAttribute = AttributeTypes.HEALTH;

	[Header("Visuals")]
	public EntityAnimationTriggers AnimationTrigger = EntityAnimationTriggers.MELEE_START;
	public EntityAnimationEvents AnimationHitEvent = EntityAnimationEvents.ATTACK_HIT;
	public List<string> OriginFX = new List<string>();
	public List<string> HitFX = new List<string>();
	public List<string> PrimeFX = new List<string>();
	public Color AttackStartTargetColor;
	public float AttackStartTargetColorDuration = 0f;
	public EntityFlashTypes AttackStartTargetColorFlashType = EntityFlashTypes.OUT;
	public Color AttackStartSourceColor;
	public float AttackStartSourceColorDuration = 0f;
	public EntityFlashTypes AttackStartSourceColorFlashType = EntityFlashTypes.OUT;
}
