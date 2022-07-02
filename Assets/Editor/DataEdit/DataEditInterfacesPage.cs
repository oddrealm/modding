using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataEditInterfacesPage : DataEditPage
{
    public override string PageName { get { return "Interfaces"; } }

    public override void OnSelectionChange()
    {
        ReOrderAndSetInterfaceIndices();
    }

    public override void RenderGUI()
    {
        base.RenderGUI();

        if (GUILayout.Button("Load Interfaces"))
        {
            SetDataDirty();
        }

        if (GUILayout.Button("Re-Order List"))
        {
            ReOrderAndSetInterfaceIndices();
        }

        if (Selection.objects.Length > 0 && GUILayout.Button("+interface_info_" + Selection.objects[0].name))
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                CreateNewInterface(Selection.objects[i].name);
            }

            ReOrderInterfaces();
        }

        _scroll = GUILayout.BeginScrollView(_scroll);

        for (int i = 0; i < _interfaces.Count; i++)
        {
            GDEInterfaceInfoData interfaceData = _interfaces[i];

            if (!string.IsNullOrEmpty(_listFilter) && !interfaceData.OrderKey.Contains(_listFilter)) { continue; }
            if (interfaceData == null) { continue; }
            GUILayout.BeginHorizontal();
            GUI.color = GUI_GetSelectedColor(interfaceData);

            GUI_SelectButton(interfaceData);

            if (GUILayout.Button("-", GUILayout.Width(32)))
            {
                MoveInterfaceDown(interfaceData);
                EditorUtility.SetDirty(interfaceData);
                ReOrderAndSetInterfaceIndices();
            }

            if (GUILayout.Button("+", GUILayout.Width(32)))
            {
                MoveInterfaceUp(interfaceData);
                EditorUtility.SetDirty(interfaceData);
                ReOrderAndSetInterfaceIndices();
            }

            GUILayout.Label(interfaceData.Index.ToString(), GUILayout.Width(64));
            GUILayout.Label(interfaceData.name, GUILayout.Width(240));
            GUILayout.Label(interfaceData.Display);
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}
