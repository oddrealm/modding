using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Blocks")]
public class GDEBlocksData : ScriptableObject
{
	public string Key { get { return name; } }
	public int Index = 0;

	public List<string> Tags = new List<string>();
	public int PhysicsHeatSource = 0;
	public float PhysicsHeatPass = 0.0f;
	public string TooltipID = "";
	public List<string> Visuals = new List<string>();
	public FactionTypes TriggerFaction = 0;
	public TriggerTypes Triggers = 0;
	public TriggerActionTypes TriggerActions = 0;
	public List<string> TriggerStatuses = new List<string>();
	public List<string> TriggerAttacks = new List<string>();
	public string TriggerSFX = "";
	public bool CanSupportWeight = false;
	public bool CanHavePlants = false;
	public bool IsFertile = false;
	public bool CanHaveFish = false;
	public int SoilQuality = 0;
	public BlockPhysicsTypes PhysicsTypes = 0;
	public int PhysicHeatSinkTemp = 0;
	public BlockPhysicsTypes FXPhysicsTypes = 0;
	public List<string> PhysicsUnstableFX = new List<string>();
	public List<string> PhysicsFillFX = new List<string>();
	public int PhysicsClearBlock = 0;
	public int PhysicsSolidBlock = 0;
	public int PhysicsLiquidBlock = 0;
	public int PhysicsGasBlock = 0;
	public int MinTemperature = 0;
	public int MaxTemperature = 0;
	public int Toughness = 0;
	public int MaxItemCount = 0;
	public ItemCategories ItemCategoryRestriction = 0;
	public bool CanShowItems = false;
	public bool RestrictItemsToSameKey = false;
	public BlockPermissionTypes Permissions = 0;
	public string AddSFX = "";
	public string RemoveSFX = "";
	public string InteractSFX = "";
	public int FlammableChance = 0;
	public int DecayBuff = 0;
	public int Level = 0;
	public string RemoveItemDrops = "";
	public float VerticalMovementMod = 0.0f;
	public float HorizontalMovementMod = 0.0f;
	public bool CanBeLocked = false;
	public int LockedMovementFlags = 0;
	public int RestrictedMovementFlags = 0;
	public float MovementCost = 0.0f;
	public bool VerticalMoveRequiresClimb = false;
	public bool ForceFocusDirection = false;
	public bool IsObstruction = false;
	public bool CanHaveRoom = true;
	public bool IsVerticalAccess = false;

	public void Clone(GDEBlocksData other)
    {
		if (other == null) { return; }

		Tags = new List<string>(other.Tags);
		PhysicsHeatSource = other.PhysicsHeatSource;
		PhysicsHeatPass = other.PhysicsHeatPass;
		TooltipID = other.TooltipID;
		Visuals = new List<string>(other.Visuals);
		TriggerFaction = other.TriggerFaction;
		Triggers = other.Triggers;
		TriggerActions = other.TriggerActions;
		TriggerStatuses = new List<string>(other.TriggerStatuses);
		TriggerAttacks = new List<string>(other.TriggerAttacks);
		TriggerSFX = other.TriggerSFX;
		CanSupportWeight = other.CanSupportWeight;
		CanHavePlants = other.CanHavePlants;
		IsFertile = other.IsFertile;
		CanHaveFish = other.CanHaveFish;
		SoilQuality = other.SoilQuality;
		PhysicsTypes = other.PhysicsTypes;
		PhysicHeatSinkTemp = other.PhysicHeatSinkTemp;
		FXPhysicsTypes = other.FXPhysicsTypes;
		PhysicsUnstableFX = new List<string>(other.PhysicsUnstableFX);
		PhysicsFillFX = new List<string>(other.PhysicsFillFX);
		PhysicsClearBlock = other.PhysicsClearBlock;
		PhysicsSolidBlock = other.PhysicsSolidBlock;
		PhysicsLiquidBlock = other.PhysicsLiquidBlock;
		PhysicsGasBlock = other.PhysicsGasBlock;
		MinTemperature = other.MinTemperature;
		MaxTemperature = other.MaxTemperature;
		Toughness = other.Toughness;
		MaxItemCount = other.MaxItemCount;
		ItemCategoryRestriction = other.ItemCategoryRestriction;
		CanShowItems = other.CanShowItems;
		RestrictItemsToSameKey = other.RestrictItemsToSameKey;
		Permissions = other.Permissions;
		AddSFX = other.AddSFX;
		RemoveSFX = other.RemoveSFX;
		InteractSFX = other.InteractSFX;
		FlammableChance = other.FlammableChance;
		DecayBuff = other.DecayBuff;
		Level = other.Level;
		RemoveItemDrops = other.RemoveItemDrops;
		VerticalMovementMod = other.VerticalMovementMod;
		HorizontalMovementMod = other.HorizontalMovementMod;
		CanBeLocked = other.CanBeLocked;
		LockedMovementFlags = other.LockedMovementFlags;
		RestrictedMovementFlags = other.RestrictedMovementFlags;
		MovementCost = other.MovementCost;
		VerticalMoveRequiresClimb = other.VerticalMoveRequiresClimb;
		ForceFocusDirection = other.ForceFocusDirection;
		IsObstruction = other.IsObstruction;
		IsVerticalAccess = other.IsVerticalAccess;
}
}
