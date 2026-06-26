using System.Collections.Generic;
using UnityEngine;

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

    [System.Serializable]
    public struct ItemRequirement
    {
        public string ItemID;
        public int Amount;
    }

    [System.Serializable]
    public struct Action
    {
        public string ActionID;
        public int KingdomPointCost;
        public List<ItemRequirement> ItemRequirements;

        public readonly bool IsNULL { get { return string.IsNullOrEmpty(ActionID); } }
    }

    public int NewGameOrder;
    public bool ShowOnMap = true;
    public bool PlayerCanChoose = true;
    public bool Unlocked = false;
    public string Banner = "";
    public Color BannerColor;
    public string CaravanIcon = "";
    public string KingdomIcon = "<sprite=1029>";
    public string SettlementIcon = "<sprite=1029>";
    public string KingdomSpriteID = string.Empty;
    public string SettlementSpriteID = string.Empty;
    public string NationTypeDisplay = "Kingdom";
    public string OverworldVisuals = "";
    public string BorderVisuals = "";
    public string RaidSpawnGroupID = "spawns_raid_default";
    public List<string> Loadouts = new();
    public List<string> Races = new();
    public List<string> DiscoveredRaces = new();
    public List<string> RaidEventEntityIDs = new();
    public List<string> VictoryRaidDialogueIDs = new();
    public List<string> DefeatRaidDialogueIDs = new();
    public string GenerateSettlementNameKey = "";
    public string GenerateKingdomNameKey = "";
    public string[] GameOverScenarios = new string[]
    {
        "scenario_settler_wave"
    };
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
    public List<string> Landmarks = new();
    public List<string> Biomes = new();
    public HashSet<string> BiomesHash = new();
    public List<DiplomaticTunigData> DiplomaticTuning = new();
    public int MaxLeaders = 1;
    public List<string> Leaders = new();
    public int RevealDepthOnSettlementStart = -1;
    public Action[] Actions = new Action[]{
        new(){
            ActionID = ACTION_START_SETTLEMENT,
            ItemRequirements = new(){
                new()
                {
                    ItemID = "item_wood_log",
                    Amount = 20,
                },
                new()
                {
                    ItemID = "item_stone_chunk",
                    Amount = 10,
                }
            }
        }
    };
    public EntitySpawnData LeaderSpawnData;
    [System.Serializable]
    public struct PerkData
    {
        public string Title;
        public string Description;
    }
    public List<PerkData> Perks = new();

    public const string ACTION_START_SETTLEMENT = "start_settlement";
    public const string ACTION_CLAIM_TERRITORY = "claim_territory";
    public const string ACTION_BUILD_ROAD = "build_road";

    public Sprite KingdomSprite { get; private set; }
    public Sprite BannerSprite { get; private set; }
    public Sprite SettlementSprite { get; private set; }
    public Dictionary<string, Action> ActionsByID { get; private set; } = new();
    public List<GDEResearchData> StartingResearch { get; private set; } = new();
    public GDERacesData FirstRaceData { get; private set; }

    public override bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking()
        {
            TagID = "tag_nations",
            TagObjectID = Key,
            HideIfZero = true,
            StartDisabled = false,
        };

        return true;
    }

    public Action GetActionByID(string actionID)
    {
        if (ActionsByID.TryGetValue(actionID, out Action action))
        {
            return action;
        }

        Debug.LogError($"Action ID {actionID} not found in nation {Key}");
        return NULL_ACTION;
    }

    public static Action NULL_ACTION = new()
    {
        ActionID = string.Empty,
        ItemRequirements = new List<ItemRequirement>(),
    };

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();

        if (string.IsNullOrEmpty(RaidSpawnGroupID))
        {
            Debug.LogError($"RaidSpawnGroupID is null or empty for nation {Key}");
        }

        ActionsByID.Clear();

        for (int i = 0; i < Actions.Length; i++)
        {
            Action action = Actions[i];

            if (!ActionsByID.ContainsKey(action.ActionID))
            {
                ActionsByID.Add(action.ActionID, action);
            }
            else
            {
                Debug.LogError($"Duplicate action ID {action.ActionID} in nation {Key}");
            }
        }

        if (Races == null || Races.Count == 0)
        {
            Races = new() { "race_cat" };

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        StartingResearch.Clear();
        GDEResearchData basics = DataManager.GetTagObject<GDEResearchData>("research_industry_basics0");
        StartingResearch.Add(basics);

        for (int i = 0; i < Races.Count; i++)
        {
            GDERacesData raceData = DataManager.GetTagObject<GDERacesData>(Races[i]);

            if (FirstRaceData == null)
            {
                FirstRaceData = raceData;
            }

            for (int j = 0; j < raceData.DefaultActiveResearch.Count; j++)
            {
                string researchID = raceData.DefaultActiveResearch[j];

                GDEResearchData researchData = DataManager.GetTagObject<GDEResearchData>(researchID);

                if (researchData == null) { continue; }

                StartingResearch.Add(researchData);
            }
        }

        if (!string.IsNullOrEmpty(KingdomSpriteID))
        {
            KingdomSprite = GlobalSettingsManager.GetIcon(KingdomSpriteID);
        }

        if (!string.IsNullOrEmpty(Banner))
        {
            BannerSprite = GlobalSettingsManager.GetIcon(Banner);
        }

        if (!string.IsNullOrEmpty(SettlementSpriteID))
        {
            SettlementSprite = GlobalSettingsManager.GetIcon(SettlementSpriteID);
        }

        BiomesHash = new HashSet<string>(Biomes);

        for (int i = 0; i < DiplomaticTuning.Count; i++)
        {
            DiplomaticTunigData tuning = DiplomaticTuning[i];

            if (!DataManager.TagObjectExists(tuning.NationID))
            {
                Debug.LogError($"Invalid nation ID in diplomatic tuning: {tuning.NationID} for {Key}");
            }

        }
    }
#endif
}
