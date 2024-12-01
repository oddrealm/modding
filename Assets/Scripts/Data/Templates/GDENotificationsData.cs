using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Notifications")]
public class GDENotificationsData : Scriptable
{
    [System.Serializable]
    public class NotificationPermissionSettings
    {
        public bool Save { get; private set; } = true;
        public void SetNeedsSave(bool save) { Save = save; }

        public bool ShowPreview = true;
        public bool CanStackPreviews = true;
        public List<string> Factions = new List<string>();
        [System.NonSerialized]
        public HashSet<string> FactionsHash = new HashSet<string>();
        public bool FocusCamOnCreate = false;
        public bool PauseGameOnCreate = false;
        public bool HighlightOnCreate = false;
        public string HighlightText = "<sprite=1654>";

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

    [System.Serializable]
    public class NotificationPermission
    {
        public string GroupID = "";
        public NotificationPermissionSettings Settings = new NotificationPermissionSettings();
    }

    public Color FlashColor = Color.white;
    public string SFXGroupID = "";

    public NotificationPermissionSettings DefaultPermissions;

    public NotificationPermission[] OverridePermissions;

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();

        DefaultPermissions.OnLoaded();

        for (int i = 0; OverridePermissions != null && i < OverridePermissions.Length; i++)
        {
            OverridePermissions[i].Settings.OnLoaded();
        }
    }
#endif
}
