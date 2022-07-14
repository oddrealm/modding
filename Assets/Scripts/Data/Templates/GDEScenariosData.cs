using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Scenarios")]
public class GDEScenariosData : ScriptableObject
{
	public string Key;
	public int Index = 0;
	public int ActivationType = 0;
	public bool IsThreat = false;
	public string TooltipID = "";
	public string TileNoteTooltipID = "";
	public bool TryQueueOnNewGame = false;
	public int TileChance = 0;
	public int ActivationChance = 0;
	public int MinGameDays = 0;
	public int MinSettlerLevel = 0;
	public int MaxSettlerLevel = 0;
	public int MaxIDActivations = 0;
	public int MaxTagActivations = 0;
	public int IDInterval = 0;
	public int TagInterval = 0;
	public string Tag = "";
	public string RequiredTag = "";
	public List<string> RequiredEntityTagsInTile = new List<string>();
	public List<string> RequiredRoomFunctions = new List<string>();
	public List<string> PermittedNations = new List<string>();
	public List<int> PermittedWeather = new List<int>();
	public List<string> PermittedBiomes = new List<string>();
	public List<string> PermittedRaces = new List<string>();
	public List<string> PermittedSeasons = new List<string>();
	public string NotificationOnActivate = "";
	public string Components = "";

	public ScenarioActions CheckConditionActions;
	public ScenarioActions ActivateActions;
	public ScenarioActions ActivateFailActions;
	public ScenarioActions Output0Actions;
	public ScenarioActions Output1Actions;
	public ScenarioActions Output2Actions;
	public ScenarioActions CompletedActions;

    [System.Serializable]
    public class ScenarioActions
    {
        public Lookup[] Lookups;
        public SpawnEntity[] SpawnEntities;
        public SpawnItem[] SpawnItems;
        public ChangeFaction[] ChangeFactions;
        public LeaveMap[] LeaveMaps;
        public DestroyItem[] DestroyItems;
        public StartDialogue[] StartDialogues;
        public MoveCamera[] MoveCameras;
        public AddScenarioTag[] AddScenarioTags;
        public StartMerchant[] StartMerchants;
        public SetEntityTag[] SetEntityTags;
        public GenerateModelPrefab[] GenerateModelPrefabs;
        public PlaySound[] PlaySounds;
        public ChangeBrightnessLevel[] ChangeBrightnessLevels;
    }

    [System.Serializable]
    public abstract class ScenarioActionData
    {

    }

    [System.Serializable]
    public class Lookup : ScenarioActionData
    {
        public string EntityCollectTag = "";
        public string EntityID = "";
        public int EntityScene = -1;
        public int EntityAgeMin = -1;
        public int EntityAgeType = -1;
        public int MaxMinsSinceSpawn = -1;
        public int EntityCollectCount = 0;
        public int EntityCollectFaction = 0;
        public bool ItemFindRandom;
        public string ItemCollectTag = "";
        public int ItemCollectCount = 0;
        public string BlockCollectTag = "";
        public int BlockCollectCount = 0;
        public int MaxZ = -1;
        public int MinZ = -1;
    }

    [System.Serializable]
    public class ChangeBrightnessLevel : ScenarioActionData
    {
        public float Brightness;
        public float Duration;
    }

    [System.Serializable]
    public class PlaySound : ScenarioActionData
    {
        public string SoundID;
        public string SoundType;
    }

    [System.Serializable]
    public class GenerateModelPrefab : ScenarioActionData
    {
        public string ModelPrefabGenID;
        public string SpawnTag;
    }

    [System.Serializable]
    public class SetEntityTag : ScenarioActionData
    {
        public string EntityTag;
        public string Focus;
    }

    [System.Serializable]
    public class StartMerchant : ScenarioActionData
    {
        public List<string> ItemGroupIDs = new List<string>();
        public string FarewellDialogueID;
    }

    [System.Serializable]
    public class AddScenarioTag : ScenarioActionData
    {
        public string Tag;
    }

    [System.Serializable]
    public class SpawnItem : ScenarioActionData
    {
        public string SpawnTag;
        public string GroupID;
    }

    [System.Serializable]
    public class MoveCamera : ScenarioActionData
    {
        public string Focus;
        public bool Immediate;
    }

    [System.Serializable]
    public class StartDialogue : ScenarioActionData
    {
        public string DialogueID;
        public string DialogueGroupID;
        public bool TryGetUnusedDialogue;
    }

    [System.Serializable]
    public class DestroyItem : ScenarioActionData
    {
        public string Focus;
        public int Count;
        public bool DestroyAll;
    }

    [System.Serializable]
    public class LeaveMap : ScenarioActionData
    {
        public bool ClosestPoint = true;
        public bool Immediate;
        public string Focus;
        public string Reason;
    }

    [System.Serializable]
    public class ChangeFaction : ScenarioActionData
    {
        public string Focus;
        public int Faction;
    }

    [System.Serializable]
    public class SpawnEntity : ScenarioActionData
    {
        public string SpawnTag;
        public string GroupID;
        public bool VarySpawnPoints = false;
        public int UniformPriorityDefault = 3;
    }
}
