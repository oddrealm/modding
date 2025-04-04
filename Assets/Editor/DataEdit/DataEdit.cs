using UnityEditor;
using UnityEngine;

public class DataEdit : EditorWindow
{
    private static bool _shouldBlockGUI = false;
    private int _currentPageIndex;

    [MenuItem("TinyBeast/DataEdit")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        DataEdit window = (DataEdit)EditorWindow.GetWindow(typeof(DataEdit));
        window.Show();
    }

    public void OnEnable()
    {
        CurrentPage.OnEnable(this);
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
        EditorApplication.update -= OnUpdate;
        EditorApplication.update += OnUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorApplication.update -= OnUpdate;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        _shouldBlockGUI = state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.EnteredPlayMode;
    }

    public void OnUpdate()
    {
        if (CurrentPage.Window == null) CurrentPage.OnEnable(this);
        CurrentPage.Update();

        if (EditorApplication.isPlaying)
        {
            Repaint();
        }
    }

    public void OnDestroy()
    {
        if (CurrentPage == null) { return; }
        CurrentPage.OnDestroy();
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

    public DataEditPage CurrentPage { get { return _pages[_currentPageIndex]; } }

    void OnGUI()
    {
#if ODD_REALM_APP
        if (Application.isPlaying && (Master.Instance == null || Master.Instance.GameState == Master.GameStates.LOADING))
        {
            return;
        }
#endif

        if (_shouldBlockGUI)
        {
#if ODD_REALM_APP
            CurrentPage.RenderMaster();
#endif
            return;
        }

        CurrentPage.RenderGUI();
    }
}
