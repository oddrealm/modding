using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataEditItemsPage : DataEditPage
{
    public override string PageName { get { return "Items"; } }

    private string _newItemName = "";
    private GDEItemsData _itemToClone;

    public override void RenderGUI()
    {
        base.RenderGUI();

        if (_items.Count == 0 || GUILayout.Button("Load Items"))
        {
            SetDataDirty();
        }

        _itemToClone = EditorGUILayout.ObjectField("Item To Clone", _itemToClone, typeof(GDEItemsData), true) as GDEItemsData;

        GUILayout.BeginHorizontal();
        GUILayout.Label("New Item Name", GUILayout.Width(128));
        _newItemName = GUILayout.TextField(_newItemName);
        GUILayout.EndHorizontal();

        GUI.color = Color.green;

        if (GUILayout.Button("Create New Item"))
        {
            CreateNewItem(_newItemName, _itemToClone);
            _newItemName = "";
        }

        GUI.color = Color.white;

        _scroll = GUILayout.BeginScrollView(_scroll);

        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == null)
            {
                SetDataDirty();
                continue;
            }

            if (!_items[i].name.Contains(_listFilter)) { continue; }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("<", GUILayout.Width(32)))
            {
                _itemToClone = _items[i];
            }

            GUI_SelectButton(_items[i]);
            GUI.color = GUI_GetSelectedColor(_items[i]);
            GUILayout.Label(_items[i].Index.ToString(), GUILayout.Width(64));
            GUILayout.Label(_items[i].name, GUILayout.Width(200));
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}
