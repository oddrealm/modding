using Assets.GameData;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RoomTemplates")]
public class GDERoomTemplatesData : Scriptable
{
    [System.Serializable]
    public struct RoomAutoJob
    {
        public string BlueprintID;
        public AutoJobSettings Settings;
        public TagObjectSetting[] ResourcePermissions;
    }

    [Header("Category")]
    public string CategoryID = "room_category_homes";

    [Header("Research")]
    public string ResearchKey = "";

    [Header("Max rooms that can be placed in a settlement.")]
    public int MaxRoomAmount = -1;

    [Header("Occupants")]
    public List<string> OccupantGroups = new List<string>();

    [Header("Visuals")]
    public List<string> Visuals = new List<string>();

    [Header("Jobs")]
    public List<RoomAutoJob> DefaultAutoJobs = new List<RoomAutoJob>();

    public override string ObjectTypeDisplay { get { return "Rooms"; } }
}
