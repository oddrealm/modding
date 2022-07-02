using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataEditBlueprintsPage : DataEditPage
{
    public override string PageName { get { return "Blueprints"; } }

    private string _newBlueprintName = "";
    private GDEBlueprintsData _blueprintToClone;
    private GDEBlockVisualsData _visualsToClone;
    private GDEBlockModelData _modelToClone;
    private GDEBlocksData _blockToClone;

    public override void RenderGUI()
    {
        base.RenderGUI();

        if (_blueprints.Count == 0 || GUILayout.Button("Load Blueprints"))
        {
            SetDataDirty();
        }

        _blueprintToClone = EditorGUILayout.ObjectField("Blueprint To Clone", _blueprintToClone, typeof(GDEBlueprintsData), true) as GDEBlueprintsData;
        _visualsToClone = EditorGUILayout.ObjectField("Visuals To Clone", _visualsToClone, typeof(GDEBlockVisualsData), true) as GDEBlockVisualsData;
        _modelToClone = EditorGUILayout.ObjectField("Model To Clone (Optional)", _modelToClone, typeof(GDEBlockModelData), true) as GDEBlockModelData;
        _blockToClone = EditorGUILayout.ObjectField("Block To Clone (Optional)", _blockToClone, typeof(GDEBlocksData), true) as GDEBlocksData;

        GUILayout.BeginHorizontal();
        GUILayout.Label("New Blueprint Name", GUILayout.Width(128));
        _newBlueprintName = GUILayout.TextField(_newBlueprintName);
        GUILayout.EndHorizontal();

        GUI.color = Color.green;

        if (GUILayout.Button("Create New Blueprint"))
        {
            CreateNewBlueprint(_newBlueprintName, _blueprintToClone, _visualsToClone, _modelToClone, _blockToClone);
            _newBlueprintName = "";
        }

        GUI.color = Color.white;

        _scroll = GUILayout.BeginScrollView(_scroll);

        for (int i = 0; i < _blueprints.Count; i++)
        {
            if (_blueprints[i] == null)
            {
                SetDataDirty();
                continue;
            }

            if (!_blueprints[i].name.Contains(_listFilter)) { continue; }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("<", GUILayout.Width(32)))
            {
                _blueprintToClone = _blueprints[i];
            }

            GUI_SelectButton(_blueprints[i]);
            GUI.color = GUI_GetSelectedColor(_blueprints[i]);
            GUILayout.Label(_blueprints[i].name, GUILayout.Width(200));
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}
