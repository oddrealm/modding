
[System.Serializable]
public struct HotkeyGroup
{
    [System.NonSerialized]
    public string SaveKey;
    public Hotkey[] Hotkeys;

#if ODD_REALM_APP
    public bool IsTriggered()
    {
        if (Hotkeys == null || Hotkeys.Length == 0) { return false; }
        if (!string.IsNullOrEmpty(SaveKey) &&
            SaveLoadManager.Instance.PlayerSettings != null &&
            SaveLoadManager.Instance.PlayerSettings.IsInputHotkeyGroupDisabled(SaveKey))
        {
            return false;
        }

        for (int i = 0; i < Hotkeys.Length; i++)
        {
            if (!Hotkeys[i].IsTriggered()) { return false; }
        }

        return true;
    }

    public void GenerateSaveKey(string inputID)
    {
        SaveKey = inputID;

        for (int i = 0; i < Hotkeys.Length; i++)
        {
            SaveKey += "_" + Hotkeys[i].UID;
        }
    }
#endif
}
