using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataEditModelsPage : DataEditPage
{
    public override string PageName { get { return "Models"; } }

    private string _newModelName = "";

    public override void RenderGUI()
    {
        base.RenderGUI();

        if (_models.Count == 0 || GUILayout.Button("Load Models"))
        {
            SetDataDirty();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("New Model Name", GUILayout.Width(128));
        _newModelName = GUILayout.TextField(_newModelName);
        GUILayout.EndHorizontal();

        GUI.color = Color.green;

        if (GUILayout.Button("Create New Model"))
        {
            CreateNewModel(_newModelName, 
                           GetScriptableObjectFromSelection<GDEBlocksData>(Selection.objects), 
                           GetScriptableObjectsFromSelection<GDEItemsData>(Selection.objects),
                           GetScriptableObjectsFromSelection<GDEBlockPlantsData>(Selection.objects),
                           GetScriptableObjectFromSelection<GDEBlockPlatformsData>(Selection.objects));
            _newModelName = "";
        }

        GUI.color = Color.white;

        _scroll = GUILayout.BeginScrollView(_scroll);

        for (int i = 0; i < _models.Count; i++)
        {

            if (_models[i] == null)
            {
                SetDataDirty();
                continue;
            }

            if (!_models[i].name.Contains(_listFilter)) { continue; }

            GUILayout.BeginHorizontal();

            GUI_SelectButton(_models[i]);
            GUI.color = GUI_GetSelectedColor(_models[i]);
            GUILayout.Label(_models[i].ModelIndex.ToString(), GUILayout.Width(64));
            GUILayout.Label(_models[i].name, GUILayout.Width(200));
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}
