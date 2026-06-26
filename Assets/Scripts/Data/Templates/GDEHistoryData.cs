using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/History")]
public class GDEHistoryData : Scriptable
{
    [System.Serializable]
    public struct HistoryDisplay
    {
        public string displayText;
        public string objectID;
        public bool useInstance;
        public string prevHistoryID;
        public bool useObjFromPrevHistory;
        public string goToID;
        public string onSuccessGoTo;
        public string onFailGoTo;
        public RandomChance chance;
    }

    public string GroupID = string.Empty;
    public string[] RequirementTagObjIDs = System.Array.Empty<string>();
    public string DisplayName = "Characteristic";
    public HistoryDisplay[] Displays = System.Array.Empty<HistoryDisplay>();
    public int MaxOccurrences = 0; // <= 0 means unlimited
    public bool UseForRandomGen = true;
    public RandomChance ChanceToOccur = new(1, 100);
    public int MinRequiredTime = -1;
    public int MaxRequiredTime = -1;
    public BuffData[] Buffs = System.Array.Empty<BuffData>();
    public string[] StatusesToAdd = System.Array.Empty<string>();
    public string[] StatusesToRemove = System.Array.Empty<string>();
    public string[] DisabledFeatures = System.Array.Empty<string>();

    public HashSet<string> RequirementTagObjIDsHash { get; private set; } = new();
    public bool HasRequirements { get { return RequirementTagObjIDs != null && RequirementTagObjIDs.Length > 0; } }
    public bool HasAugment { get { return AugmentCount > 0; } }
    public int AugmentCount { get { return Buffs.Length + DisabledFeatures.Length; } }

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        RequirementTagObjIDsHash.Clear();

        if (RequirementTagObjIDs != null && RequirementTagObjIDs.Length > 0)
        {
            RequirementTagObjIDsHash = new HashSet<string>(RequirementTagObjIDs);
        }

        base.OnLoaded();
    }
#endif
}
