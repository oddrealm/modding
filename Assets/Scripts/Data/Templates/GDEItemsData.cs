using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Items")]
public class GDEItemsData : ScriptableObject
{
	public string Key { get { return name; } }
	public int Index = 0;

	public string TooltipID = "";
	public string Visuals = "";
	public GDECharacterColorMaskData CharacterColorMask;
	public GDECharacterAccessoryData AccessoryData;
	public string PickUpSFX = "";
	public string DropSFX = "";
	public List<string> Tags = new List<string>();
	public ItemCategories Category = ItemCategories.NONE;
	public BlockPermissionTypes Permissions = BlockPermissionTypes.NONE;
	public string Inventory = "";
	public string Buffs = "";
	public string AttackGroup = "";
	public bool StartDiscovered = false;
	public int MaxCountInMerchantList = 0;
	public string GenerateNameKey = "";
	public int MerchantBuyValue = 0;
	public int MerchantSellValue = 0;
	public int TuneRating = 0;
	public int RarityType = 0;
	public int DecayRate = 0;
	public string DecayItem = "";
	public bool HasMeltThreshold = false;
	public int MeltThreshold = 0;
	public int FlammableChance = 0;
	public int LightStrength = 0;
	public int LightSize = 0;
	public int StackSize = 1;
	public int JobOutputMod = 1;

	public void Clone(GDEItemsData other)
    {
		if (other == null) { return; }

		TooltipID = other.TooltipID;
		Visuals = other.Visuals;
		CharacterColorMask = other.CharacterColorMask;
		AccessoryData = other.AccessoryData;
		PickUpSFX = other.PickUpSFX;
		DropSFX = other.DropSFX;
		Tags = new List<string>(other.Tags);
		Category = other.Category;
		Inventory = other.Inventory;
		Permissions = other.Permissions;
		Buffs = other.Buffs;
		AttackGroup = other.AttackGroup;
		StartDiscovered = other.StartDiscovered;
		MaxCountInMerchantList = other.MaxCountInMerchantList;
		GenerateNameKey = other.GenerateNameKey;
		MerchantBuyValue = other.MerchantBuyValue;
		MerchantSellValue = other.MerchantSellValue;
		TuneRating = other.TuneRating;
		RarityType = other.RarityType;
		DecayRate = other.DecayRate;
		DecayItem = other.DecayItem;
		HasMeltThreshold = other.HasMeltThreshold;
		MeltThreshold = other.MeltThreshold;
		FlammableChance = other.FlammableChance;
		LightStrength = other.LightStrength;
		LightSize = other.LightSize;
		StackSize = other.StackSize;
}
}
