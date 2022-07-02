using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class DataEditSearchPage : DataEditPage
{
    public override string PageName { get { return "Search"; } }

    public override void RenderGUI()
    {
        base.RenderGUI();

        GUILayout.Label("Search", GUILayout.Width(100));
        _searchInput = GUILayout.TextField(_searchInput);

        GUILayout.Label("Replace", GUILayout.Width(100));
        _searchReplace = GUILayout.TextField(_searchReplace);

        GUILayout.BeginHorizontal();

        GUI.color = Color.green;

        if (_activeLoads != null && _activeLoads.Count > 0)
        {
            GUILayout.Label("Searching Groups: " + _activeLoads.Count);

            for (int i = 0; i < _activeLoads.Count; i++)
            {
                if (!_activeLoads[i].IsDone) { continue; }

                _activeLoads.RemoveAt(i);
                break;
            }
        }
        else
        {
            if (GUILayout.Button("Search"))
            {
                _searchedObjects.Clear();

                List<string> resourcePaths = GetFilePathsForDir(Application.dataPath + "/Resources/Data");
                _searchedObjects.AddRange(LoadObjects(resourcePaths, _searchInput, Resources.Load, SearchFilter));
                List<string> resourceMovedPaths = GetFilePathsForDir(Application.dataPath + "/Resources_moved/Data");
                _searchedObjects.AddRange(LoadObjects(resourceMovedPaths, _searchInput, (string path) => {
                    path = "Assets/Resources_moved/" + path + ".asset";
                    return AssetDatabase.LoadAssetAtPath<Object>(path);
                }, SearchFilter));
            }
        }

        GUI.color = Color.yellow;

        if (_searchedObjects.Count > 0 && GUILayout.Button("Replace All"))
        {
            for (int i = 0; i < _searchedObjects.Count; i++)
            {
                ReplaceStringInField(_searchedObjects[i], _searchInput, _searchReplace);
            }
        }

        GUI.color = Color.white;
        GUILayout.EndHorizontal();

        _scroll = GUILayout.BeginScrollView(_scroll);
        for (int i = 0; i < _searchedObjects.Count; i++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_searchedObjects[i].name))
            {
                Selection.objects = new Object[] { _searchedObjects[i] };
                GUI.FocusControl(null);
            }

            if (GUILayout.Button("Replace", GUILayout.Width(100)))
            {
                ReplaceStringInField(_searchedObjects[i], _searchInput, _searchReplace);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

}
