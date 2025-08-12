using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Notifications")]
public class GDENotificationsData : Scriptable
{
    [System.Serializable]
    public class NotificationPermissionSettings
    {
        public bool ShowPreview = true;
        public bool CanStackPreviews = true;
        public List<string> Factions = new List<string>();
        public bool FocusCamOnCreate = false;
        public bool PauseGameOnCreate = false;
        public bool HighlightOnCreate = false;
        public string HighlightText = "<sprite=1654>";

        public HashSet<string> FactionsHash { get; private set; } = new HashSet<string>();
        public bool Save { get; private set; } = true;

        public void SetNeedsSave(bool save) { Save = save; }

        public void AddFaction(string faction)
        {
            if (!FactionsHash.Add(faction)) { return; }

            Factions.Add(faction);
        }

        public void RemoveFaction(string faction)
        {
            if (!FactionsHash.Remove(faction)) { return; }

            Factions.Remove(faction);
        }

        public void OnLoaded()
        {
            FactionsHash.Clear();

            for (int i = 0; Factions != null && i < Factions.Count; i++)
            {
                FactionsHash.Add(Factions[i]);
            }
        }
    }

    public string NotifGroup = string.Empty;
    public string SFXGroupID = string.Empty;
    public NotificationPermissionSettings DefaultPermissions;
    public const string NOTIF_GROUP_ALL = "tag_notif_group_all";

    public GDETagsData NotifGroupData { get; private set; }
    public GDETagsData NotifGroupAllData { get; private set; }

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();
        NotifGroupData = DataManager.GetTagObject<GDETagsData>(NotifGroup);
        NotifGroupAllData = DataManager.GetTagObject<GDETagsData>(NOTIF_GROUP_ALL);

        if (TagIDsHash.Add(NotifGroup))
        {
            TagIDs.Add(NotifGroup);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        DefaultPermissions.OnLoaded();
    }
#endif
}
