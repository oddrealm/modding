using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Linq;
using System.Globalization;

public class DataEdit : EditorWindow
{
    [MenuItem("Tools/DataEdit")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        DataEdit window = (DataEdit)EditorWindow.GetWindow(typeof(DataEdit));
        window.Show();
    }

    public void OnEnable()
    {
        CurrentPage.OnEnable(this);
    }

    public void Update()
    {
        if (CurrentPage.Window == null) CurrentPage.OnEnable(this);
        CurrentPage.Update();
    }

    public void OnDestroy()
    {
        CurrentPage.OnDestroy();

        //Debug.LogError("Destroyed");
    }

    public void OnSelectionChange()
    {
        Repaint();
        CurrentPage.OnSelectionChange();
    }

    private DataEditPage[] _pages = new DataEditPage[]
    {
        new DataEditTagPage()
    };

    private int _currentPageIndex;
    public DataEditPage CurrentPage { get { return _pages[_currentPageIndex]; } }

    void OnGUI()
    {
#if ODD_REALM_APP
        if (Application.isPlaying && (Master.Instance == null || Master.Instance.GameState == Master.GameStates.LOADING))
        {
            return;
        }
#endif

        GUILayout.BeginHorizontal();

        for (int i = 0; i < _pages.Length; i++)
        {
            GUI.color = _currentPageIndex == i ? Color.cyan : Color.gray;
            if (GUILayout.Button(_pages[i].PageName)) _currentPageIndex = i;
        }

        GUI.color = Color.white;
        GUILayout.EndHorizontal();

        CurrentPage.RenderGUI();
    }
}
