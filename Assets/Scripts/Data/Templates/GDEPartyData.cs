using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Party")]
public class GDEPartyData : Scriptable
{
    [System.Serializable]
    public struct MemberTuning
    {
        public string EntityID;
        public string TuningID;
    }

    public string ArrivalNotif = "notif_overworld_party_arrival";

    [Header("Auto-Pop Settings:")]
    public int MaxMemberSize = 5;
    public int MinMemberSize = 1;
    public bool AutoGenMemberSize = false;

    [Header("Member Spawning:")]
    [Header("If false, party members will use MemberTuning linearly when spawned.")]
    public bool RandomizeMembers = false;
    public List<MemberTuning> MemberTunings = new List<MemberTuning>();
    [Header("Used instead of the party's nation relationship with player.")]
    public string FactionOverride = "";

}
