using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/InputCommand")]
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
    public HotkeyGroup[] DefaultTriggers;
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
    public bool PlayerCanEdit = true;
    [Header("If false, keys like shift, ctrl, alt can block this input from triggering.")]
    public bool IgnoreModifiers = false;

    public bool HasMouseInput { get; private set; }

#if DEV_TESTING
    public bool PrintEvents = false;
#endif

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();
        HasMouseInput = false;

        for (int i = 0; DefaultTriggers != null && i < DefaultTriggers.Length; i++)
        {
            DefaultTriggers[i].GenerateSaveKey(Key);

            for (int j = 0; j < DefaultTriggers[i].Hotkeys.Length; j++)
            {
                if (DefaultTriggers[i].Hotkeys[j].Mouse != Hotkey.MouseTypes.NONE)
                {
                    HasMouseInput = true;
                }

                if (DefaultTriggers[i].Hotkeys[j].Motion == Hotkey.MotionTypes.NONE)
                {
                    DefaultTriggers[i].Hotkeys[j].Motion = Hotkey.MotionTypes.ANY;
                    Debug.LogError($"Changing {Key} from NONE to ANY.");
                }

                // if (DefaultTriggers[i].Hotkeys[j].Mouse == Hotkey.MouseTypes.NONE)
                // {
                //     if (DefaultTriggers[i].Hotkeys[j].Motion == Hotkey.MotionTypes.ANY)
                //     {
                //         DefaultTriggers[i].Hotkeys[j].Motion = Hotkey.MotionTypes.RELEASE;
                //         Debug.LogError($"Changing {Key} from ANY to RELEASE.");
                //         EditorUtility.SetDirty(this);
                //     }
                // }
            }
        }
    }
#endif
}
