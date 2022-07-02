using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataEditBlocksPage : DataEditPage
{
    public override string PageName { get { return "Blocks"; } }

    private string _newBlockName = "";
    private GDEBlocksData _blockToClone;
    private GDEBlueprintsData _blueprintToClone;

    public override void RenderGUI()
    {
        base.RenderGUI();

        if (_blocks.Count == 0 || GUILayout.Button("Load Blocks"))
        {
            SetDataDirty();
        }

        _blockToClone = EditorGUILayout.ObjectField("Block To Clone", _blockToClone, typeof(GDEBlocksData), true) as GDEBlocksData;
        GUI.color = Color.cyan;
        _blueprintToClone = EditorGUILayout.ObjectField("Blueprint To Clone", _blueprintToClone, typeof(GDEBlueprintsData), true) as GDEBlueprintsData;
        GUI.color = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.Label("New Block Name", GUILayout.Width(128));
        _newBlockName = GUILayout.TextField(_newBlockName);
        GUILayout.EndHorizontal();

        GUI.color = Color.green;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("+Block"))
        {
            CreateNewBlock(_newBlockName, _blockToClone);
            _newBlockName = "";
        }

        if (GUILayout.Button("+Block & Blueprint"))
        {
            GDEBlocksData newBlock = CreateNewBlock(_newBlockName, _blockToClone);
            CreateNewBlueprint(_newBlockName, 
                               _blueprintToClone, 
                               LoadScriptableObject<GDEBlockVisualsData>(newBlock.Visuals[0]), LoadScriptableObject<GDEBlockModelData>("model_block_" + _newBlockName),
                               newBlock);
            _newBlockName = "";
        }

        GUILayout.EndHorizontal();
        GUI.color = Color.white;

        _scroll = GUILayout.BeginScrollView(_scroll);

        for (int i = 0; i < _blocks.Count; i++)
        {
            if (_blocks[i] == null)
            {
                SetDataDirty();
                continue;
            }

            if (!_blocks[i].name.Contains(_listFilter)) { continue; }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("<", GUILayout.Width(32)))
            {
                _blockToClone = _blocks[i];
            }

            GUI_SelectButton(_blocks[i]);
            GUI.color = GUI_GetSelectedColor(_blocks[i]);
            GUILayout.Label(_blocks[i].Index.ToString(), GUILayout.Width(64));
            GUILayout.Label(_blocks[i].name, GUILayout.Width(200));
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}
