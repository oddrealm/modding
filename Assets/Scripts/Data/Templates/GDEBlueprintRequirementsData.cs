using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BlueprintRequirements")]
public class GDEBlueprintRequirementsData : ScriptableObject
{
	[System.Serializable]
	public class ItemRequirement
    {
		public string ID = "";
		public int Count = 1;
		public ItemCategories Category = ItemCategories.NONE;

		public ItemRequirement(string id, int count, ItemCategories cat)
        {
			ID = id;
			Count = count;
			Category = cat;
        }
    }

	[System.Serializable]
	public class EntityRequirement
    {
		public string RaceID = "";

		public ConditionTypes StatusConditionType = ConditionTypes.AND;
		public List<string> Statuses = new List<string>();
		public EntityIntelligenceTypes IntelligenceType = EntityIntelligenceTypes.ALL;

		
	}

	public string Key { get { return name; } }
	[Header("Blocks")]
	public bool CanStackWithSameJobType = false;
	public BlockPermissionTypes PermissionType = BlockPermissionTypes.NONE;
	public ConditionTypes BlockTagConditionType = ConditionTypes.AND;
	public List<string> BlockTags = new List<string>();
	public List<string> WorkstationTags = new List<string>();
	public BlueprintWorkPointTypes WorkPointLocation = BlueprintWorkPointTypes.ADJACENT;
	[Header("Entities")]
	public List<EntityRequirement> Entities = new List<EntityRequirement>();
	[Header("Targets")]
	public List<EntityRequirement> Targets = new List<EntityRequirement>();
	[Header("Rooms")]
	public List<string> Rooms = new List<string>();
	[Header("Attacks")]
	public List<string> Attacks = new List<string>();
    [Header("Skills")]
    public string Skill = "";
    [Header("Items")]
	public string SpecialItemTooltipID;
	public AttributeTypes[] DietAttributesForItemFilter;
	public ConditionTypes ItemConditionType = ConditionTypes.AND;
	public List<ItemRequirement> Items = new List<ItemRequirement>();

	public void Clone(GDEBlueprintRequirementsData other)
    {
		if (other == null) { return; }

		BlockTagConditionType = other.BlockTagConditionType;
		BlockTags = new List<string>(other.BlockTags);
		WorkstationTags = new List<string>(other.WorkstationTags);
		Rooms = new List<string>(other.Rooms);
		Items = new List<ItemRequirement>(other.Items);
	}
}
