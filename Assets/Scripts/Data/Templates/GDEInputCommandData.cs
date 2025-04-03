using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Input/InputCommand", order = 21)]
public class GDEInputCommandData : Scriptable
{
    public enum InputCategories
    {
        NONE,
        CAMERA,
        UI,
        WORLD,
        OVERWORLD,
        SYSTEM,
        TIME
    }

    public InputCategories Category = InputCategories.UI;

    public InputStateTypes InputState = InputStateTypes.ALL;
    [Header("If this input is also triggered, we won't trigger")]
    public string[] DeferTo = new string[] { };
    public HotkeyGroup[] DefaultTriggers;
    //[Header("Clear input listeners when session unloads")]
    //public bool IsSessionInput;
    [Header("Fire Activate() each frame it's active")]
    public bool ActiveUpdate = true; // Fire Activate event while active
    [Header("The intervals to call Activate() when active")]
    public float ActiveUpdateDelaySeconds = 0.0f;
    [Header("Allow activate when mouse is over UI elements")]
    public bool CanTriggerOverUI = true;
    [Header("Allow activate when mouse is not in screen bounds")]
    public bool CanTriggerOutsideScreen = true;
    [Header("If true, the input will stay active until it is triggered again")]
    public bool IsToggle = false;
    [Header("True: UI buttons force an input to stay on (i.e., show labels). False: UI buttons active the input once on click (i.e., stop entity).")]
    public bool BtnsForceActive = false;

    public int MaxIncrement = 1;

    public bool PlayerCanEdit = true;

#if DEV_TESTING
    public bool PrintEvents = false;
#endif

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();

        for (int i = 0; DefaultTriggers != null && i < DefaultTriggers.Length; i++)
        {
            DefaultTriggers[i].GenerateSaveKey(Key);
        }
    }
#endif
}