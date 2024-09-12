using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackingTypes
{
	ENTITY,
	ITEM,
	NATION
}

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldNation")]
public class GDEOverworldNationData : Scriptable
{
	[System.Serializable]
	public struct DiplomaticTunigData
	{
		public string NationID;
		public DiplomaticRelationshipTypes Relationship;
		[Header("Chances: 1/1000")]
		public int Chances;
    }

	public bool ShowOnMap = true;
	public bool PlayerCanChoose = true;
    public bool Unlocked = false;
	public string Banner = "";
	public Color BannerColor;
	public string KingdomIcon = "<sprite=1029>";
	public string SettlementIcon = "<sprite=1029>";
    public string OverworldVisuals = "";
	public string BorderVisuals = "";
	public List<string> Loadouts = new List<string>();
	public List<string> Races = new List<string>();
	public List<string> DiscoveredRaces = new List<string>();
	public string GenerateSettlementNameKey = "";
	public string GenerateKingdomNameKey = "";

	[Header("Chances: 1/1000")]
	public int ChanceToSendMerchant = 50;
	public string MerchantPartyID = "party_merchant";
	public int ChanceToMigrate = 50;
    public string MigrantPartyID = "party_migrant";
    public int ChanceToRaid = 50;
    public string RaidingPartyID = "party_raiding";
    public int ChanceToExpandTerritory = 50;
    public string ColonistPartyID = "party_colonist";

	public string NewPartyID = "party_empty";

    public int KingdomSizeMax = 25;
    public int SettlementSimPopMax = 50;
	public int SettlementSimPopMin = 5;
	public List<string> Landmarks = new List<string>();
    public List<string> Biomes = new List<string>();
    public HashSet<string> BiomesHash = new HashSet<string>();
    public List<DiplomaticTunigData> DiplomaticTuning = new List<DiplomaticTunigData>();

	public int MaxLeaders = 1;
	public List<string> Leaders = new List<string>();

    public override bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking()
        {
            TagID = "tag_nations",
            TagObjectID = Key,
            HideIfZero = true,
            StartDisabled = false,
            TrackingType = TrackingTypes.NATION
        };

        return true;
    }

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();

		BiomesHash = new HashSet<string>(Biomes);
    }
#endif
}
