using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ToolbarMenus")]
public class GDEToolbarMenusData : Scriptable
{
    public SelectionTypes SelectionType = SelectionTypes.OBJECT;
    public string InputID = "";
}
