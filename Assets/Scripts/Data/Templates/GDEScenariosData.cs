using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/Scenarios")]
public class GDEScenariosData : Scriptable
{
    [System.Serializable]
    public class ScenarioEntitySpawn
    {
        public SpawnPointTypes SpawnPointType = SpawnPointTypes.RANDOM_BORDER_GROUND_FALLBACK_WATER;
        public bool OverrideEntityWithNation = false;
        public bool OverrideEntityWithFauna = false;
        public bool AutoGenEntityCount = false;
        public EntitySpawnData SpawnData;
    }

    [System.Serializable]
    public class ScenarioDialogue
    {
        public string DialogueID = "";
    }

    [System.Serializable]
    public class TagObjectRequirement
    {
        public string TagObjectID = "";
        public int Count = 0;
        public ThresholdConditionTypes Comparison = ThresholdConditionTypes.GREATER_THAN;
    }

    [System.Serializable]
    public class Condition
    {
        public ConditionTypes ConditionType = ConditionTypes.OR;
        public TagObjectRequirement TagObjectRequirement = new TagObjectRequirement();
        public bool MustFail = false;
    }

    public bool IsThreat = false;
    public string NotifOnActivate = "";
    public ScenarioEntitySpawn[] EntitySpawns = new ScenarioEntitySpawn[0];
    public ScenarioDialogue[] Dialogues = new ScenarioDialogue[0];
    public Condition[] Conditions = new Condition[0];
    public string PartyID = "";
    public bool IsScheduled = false;
    public RandomChance ActivationChance;
    public int MaxActivations = 0;
    public uint MinKingdomLevel = 1;
    public uint MinMinutesSinceSettlementStart = 1440;
    public uint MinMinutesSinceLastScenario = 1440;
    public uint MinMinutesSinceLastSameScenario = 1440;
    public uint MinTimeOfDay = 0;
    public uint MaxTimeOfDay = 1440;

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        for (int i = 0; i < Dialogues.Length; i++)
        {
            if (string.IsNullOrEmpty(Dialogues[i].DialogueID) || !DataManager.TagObjectExists(Dialogues[i].DialogueID))
            {
                Debug.LogError($"Scenario {name} has a missing dialogue data object!");
            }
        }
        base.OnLoaded();
    }
#endif
}
