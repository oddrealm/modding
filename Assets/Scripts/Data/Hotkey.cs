using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public struct Hotkey
{
    public enum MouseTypes
    {
        NONE = 0,
        LEFT_MOUSE = 1,
        RIGHT_MOUSE = 2,
        MIDDLE_MOUSE = 3,
        SCROLL_UP = 4,
        SCROLL_DOWN = 5
    }

    public bool Down;
    public bool Hold;
    public bool Up;

    public KeyCode Key;
    public MouseTypes Mouse;

    public int UID
    {
        get
        {
            if (Key != KeyCode.None) { return (int)Key; }
            if (Mouse != MouseTypes.NONE) { return (int)Mouse + 999; }
            return 0;
        }
    }

#if ODD_REALM_APP
    public string GetDisplay()
    {
        string txt = "???";
        string inline = "<sprite=1576>";

        if (Key != KeyCode.None)
        {
            txt = InputReceiverManager.GetKeyName(Key);
        }
        else if (Mouse != MouseTypes.NONE)
        {
            switch (Mouse)
            {
                case MouseTypes.LEFT_MOUSE:
                    txt = "L-Mouse";
                    inline = "<sprite=1113>";
                    break;

                case MouseTypes.RIGHT_MOUSE:
                    txt = "R-Mouse";
                    inline = "<sprite=1114>";
                    break;

                case MouseTypes.MIDDLE_MOUSE:
                    txt = "M-Mouse";
                    inline = "<sprite=1112>";
                    break;

                case MouseTypes.SCROLL_UP:
                    txt = "Scroll Up";
                    inline = "<sprite=1112>";
                    break;

                case MouseTypes.SCROLL_DOWN:
                    txt = "Scroll Down";
                    inline = "<sprite=1112>";
                    break;
            }
        }

        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
        txt = myTI.ToTitleCase(txt.ToLower());

        return inline + txt;
    }

    public bool IsTriggered()
    {
        if (Mouse != MouseTypes.NONE)
        {
            if (Mouse != MouseTypes.SCROLL_UP && Mouse != MouseTypes.SCROLL_DOWN)
            {
                if (!(Hold && Input.GetMouseButton((int)Mouse - 1)) &&
                    !(Down && Input.GetMouseButtonDown((int)Mouse - 1)) &&
                    !(Up && Input.GetMouseButtonUp((int)Mouse - 1))) { return false; }
            }
            else
            {
                if (InputReceiverManager.Instance.IsMouseOverUI()) { return false; }

                float scrollInput = Input.GetAxis("Mouse ScrollWheel");

                if (scrollInput >= -float.Epsilon && Mouse == MouseTypes.SCROLL_UP)
                {
                    return false;
                }

                if (scrollInput <= float.Epsilon && Mouse == MouseTypes.SCROLL_DOWN)
                {
                    return false;
                }
            }

            return true;
        }

        if (Key != KeyCode.None)
        {
            if (!(Hold && Input.GetKey(Key)) &&
                !(Down && Input.GetKeyDown(Key)) &&
                !(Up && Input.GetKeyUp(Key))) { return false; }

            return true;
        }

        return false;
    }
#endif
}
