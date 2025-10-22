using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public abstract class DataEditPage
{
    protected bool _dataNeedsReload = true;
    protected string _searchInput = "";
    protected string _searchReplace = "";
    protected string _listFilter = "";
    protected List<AsyncOperationHandle<IList<UnityEngine.Object>>> _activeLoads =
        new List<AsyncOperationHandle<IList<UnityEngine.Object>>>();
    protected List<UnityEngine.Object> _searchedObjects = new List<UnityEngine.Object>();
    protected Dictionary<string, Sprite> _tooltipSprites = new Dictionary<string, Sprite>();
    protected static List<Scriptable> _allData = new List<Scriptable>();
    protected static Dictionary<string, List<Scriptable>> _allDataByType =
        new Dictionary<string, List<Scriptable>>();
    protected static readonly TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;
    private Dictionary<string, List<string>> _dataKeysByType = new();
    private HashSet<string> _expandedKeys = new HashSet<string>();
    protected DataEdit _window;
    protected List<TuningKeyWordHelper> _helpers = new List<TuningKeyWordHelper>();
    protected bool _useHelpers = false;
    protected bool _showHelpers = true;
    protected Vector2 _helpersScroll;
    protected bool _disableHelperMismatchGUI = true;
    protected List<string> _scriptTypes = new List<string>();
    private List<string> ids = new List<string>();
    private int _unusedTransitionKey = 1;
    private HashSet<int> _indexHash = new HashSet<int>();

    public abstract string PageName { get; }
    public DataEdit Window
    {
        get { return _window; }
    }

    private const string SAVE_KEY_EXPANDED_KEYS = "save_key_expanded_keys";
    private readonly Color COLOR_PURPLE = new Color(0.7f, 0f, 0.7f, 1f);

    private bool IsExpanded(string key)
    {
        return _expandedKeys.Contains(key);
    }

    private void SetExpanded(string key, bool expanded)
    {
        if (expanded)
        {
            _expandedKeys.Add(key);
        }
        else
        {
            _expandedKeys.Remove(key);
        }

        MARK_NEEDS_SAVE();
    }

    public virtual void OnEnable(DataEdit window)
    {
        _window = window;
        LoadSaveData();
        _expandedKeys = new HashSet<string>(LOAD_STRINGS(SAVE_KEY_EXPANDED_KEYS));

        if (_expandedKeys.Count > 1000)
        {
            _expandedKeys.Clear();
        }
    }

    public virtual void OnDisable() { }

    public virtual void OnDestroy() { }

    private bool _needsSave;

    protected void MARK_NEEDS_SAVE()
    {
        _needsSave = true;
    }

    protected virtual void SAVE()
    {
        SAVE(SAVE_KEY_EXPANDED_KEYS, new List<string>(_expandedKeys));
    }

    public virtual void Update()
    {
        if (_needsSave)
        {
            SAVE();
            _needsSave = false;
        }

        //if (_dataNeedsReload)
        //{
        //    _dataNeedsReload = false;
        //    //LoadAllData();
        //}
    }

    public virtual void OnSelectionChange() { }

    protected void RenderNextAvailableIndex(
        int listCount,
        System.Func<int, int> getIndex,
        System.Action<int> onIndexAlreadyUsed
    )
    {
        if (listCount == 0)
        {
            return;
        }
        //int nextModelIndex = -1;
        _indexHash.Clear();
        int lastIndex = getIndex(listCount - 1);

        for (int i = 0; i < listCount; i++)
        {
            int index = getIndex(i);

            if (!_indexHash.Add(index))
            {
                onIndexAlreadyUsed?.Invoke(i);
            }

            //if (getIndex(i) == i) { continue; }

            //nextModelIndex = i;
            //break;
        }

        int nextModelIndex = -1;

        for (int i = 0; i < lastIndex + 1; i++)
        {
            if (_indexHash.Contains(i))
            {
                continue;
            }

            nextModelIndex = i;
            break;
        }

        if (nextModelIndex == -1)
        {
            nextModelIndex = lastIndex + 1;
        }

        GUILayout.Label("Next available Index: " + nextModelIndex);
    }

    private string _fooTxt = "foo txt";
    private int _fooInt = 0;
    private string _newName = "";
    private List<string> _unassignedBlueprints = new List<string>();
    private HashSet<string> _ignoredBlueprints = new HashSet<string>();
    private Vector2 _unassignedBlueprintScroll;
    private List<string> _selectedIDs = new List<string>();
    protected string _copiedText;
    private GameObject _masterObj;
#if ODD_REALM_APP
    private Master _master;
#endif

    protected virtual void MarkDataNeedsReload()
    {
        _dataNeedsReload = true;
    }

    protected virtual void MarkVisibleDirty() { }

    public virtual void RenderGUI()
    {
        if (_dataNeedsReload)
        {
            LoadAllData();
            _dataNeedsReload = false;
        }

        if (Event.current.keyCode == KeyCode.Escape)
        {
            GUI.FocusControl(null);
        }

#if ODD_REALM_APP
        RenderMaster();
#endif
        BEGIN_HOR();

        if (GUILayout.Button("Reload Data"))
        {
            LoadAllData();
        }

        GUI.color = Color.yellow;
        if (GUILayout.Button("SAVE ALL"))
        {
            if (EditorUtility.DisplayDialog("Save All Data?", "", "OK", "Cancel"))
            {
                for (int i = 0; i < DataLabels.AllLabels.Length; i++)
                {
                    DataUtility.ImportAllScriptables(
                        DataLabels.AllLabels[i],
                        EditorUtility.SetDirty
                    );
                }

                AssetDatabase.SaveAssets();
            }
        }

        GUI.color = Color.magenta;

        if (GUILayout.Button("FOO"))
        {
            FOO(_fooTxt, _fooInt);
        }

        _fooTxt = TEXT_INPUT(_fooTxt);
        _fooInt = INT_INPUT(_fooInt);
        GUI.color = Color.white;

        if (Selection.objects != null && Selection.objects.Length > 0)
        {
            DELETE_BTN(CLEAR_SELECT);
            BTN($"{Selection.objects.Length} Selected", () => { });
        }

        END_HOR();
    }

#if ODD_REALM_APP
    public virtual void RenderMaster()
    {
        if (_masterObj == null || _master == null)
        {
            _masterObj = GameObject.Find("master");

            if (_masterObj != null)
            {
                _master = _masterObj.GetComponent<Master>();
            }
        }

        if (_master == null)
        {
            return;
        }

        if (!Application.isPlaying)
        {
            BEGIN_HOR();
            BTN(
                "SPLASH",
                Color.white,
                () =>
                {
                    _master.LoadType = Master.QuickLoadTypes.SPLASH_SCREEN;
                    EditorApplication.isPlaying = true;
                },
                GUILayout.Height(24)
            );
            BTN(
                "LAST SAVE",
                Color.white,
                () =>
                {
                    _master.LoadType = Master.QuickLoadTypes.LAST_SAVE;
                    EditorApplication.isPlaying = true;
                },
                GUILayout.Height(24)
            );
            BTN(
                "OVERWORLD",
                Color.white,
                () =>
                {
                    _master.LoadType = Master.QuickLoadTypes.INTO_OVERWORLD_MAP;
                    EditorApplication.isPlaying = true;
                },
                GUILayout.Height(24)
            );
            BTN(
                "GAME",
                Color.green,
                () =>
                {
                    _master.LoadType = Master.QuickLoadTypes.INTO_GAME;
                    EditorApplication.isPlaying = true;
                },
                GUILayout.Height(24)
            );
            END_HOR();
        }

        bool showSettings = EXPAND_TOGGLE("Settings", Color.white, 32, "master_settings");

        if (showSettings)
        {
            List<string> loadoutKeys = GetDataKeysByType<GDEStartingLoadoutsData>();
            List<string> nationKeys = GetDataKeysByType<GDEOverworldNationData>();
            List<string> biomeKeys = GetDataKeysByType<GDEBiomesData>();
            EditorGUI.BeginChangeCheck();
            BEGIN_INDENT();
            LABEL("New Game:", Color.grey);
            _master.LoadoutID = DROP_DOWN("Loadout", _master.LoadoutID, loadoutKeys);
            _master.NationID = DROP_DOWN("Nation", _master.NationID, nationKeys);
            _master.BiomeID = DROP_DOWN("Biome", _master.BiomeID, biomeKeys);
            LABEL("GUI:", Color.grey);
            _master.ShowGUIStats = GUILayout.Toggle(_master.ShowGUIStats, "Show GUI Stats");
            _master.ShowTutorialStats = GUILayout.Toggle(
                _master.ShowTutorialStats,
                "Show Tutorial Stats"
            );
            _master.ShowSaveLoadTimesGUI = GUILayout.Toggle(
                _master.ShowSaveLoadTimesGUI,
                "Show Save Load Times"
            );
            _master.ShowJobGUI = GUILayout.Toggle(_master.ShowJobGUI, "Show Job GUI");
            _master.ShowWorldGUI = GUILayout.Toggle(_master.ShowWorldGUI, "Show World GUI");
            _master.ShowProgressGUI = GUILayout.Toggle(
                _master.ShowProgressGUI,
                "Show Progress GUI"
            );
            _master.ShowAllInstanceTags = GUILayout.Toggle(
                _master.ShowAllInstanceTags,
                "Show All Instance Tags"
            );
            LABEL("Misc:", Color.grey);
            _master.DebugTriggers = GUILayout.Toggle(_master.DebugTriggers, "Debug Triggers");
            _master.DebugDirty = GUILayout.Toggle(_master.DebugDirty, "Debug Dirty");
            _master.DebugWork = GUILayout.Toggle(_master.DebugWork, "Debug Work");
            _master.DebugSkylight = GUILayout.Toggle(_master.DebugSkylight, "Debug Skylight");
            _master.DebugEntityMovement = GUILayout.Toggle(_master.DebugEntityMovement, "Debug Entity Movement");
            _master.DebugEntityState = GUILayout.Toggle(_master.DebugEntityState, "Debug Entity State");
            _master.DebugDiets = GUILayout.Toggle(_master.DebugDiets, "Debug Diets");
            _master.DebugTuneRating = GUILayout.Toggle(_master.DebugTuneRating, "Debug Tune Rating");
            if (_master.DebugTuneRating)
            {
                _master.ForcedDebugTuneRating = INT_INPUT(_master.ForcedDebugTuneRating, "Forced Debug Tune Rating");
            }
            _master.DebugBlockRebuild = GUILayout.Toggle(_master.DebugBlockRebuild, "Debug Block Rebuild");
            _master.DebugPlants = GUILayout.Toggle(_master.DebugPlants, "Debug Plants");
            _master.DebugParties = GUILayout.Toggle(_master.DebugParties, "Debug Parties");
            _master.DebugCombat = GUILayout.Toggle(_master.DebugCombat, "Debug Combat");
            _master.DebugFishSpawn = GUILayout.Toggle(_master.DebugFishSpawn, "Debug Fish Spawn");
            _master.DebugFishing = GUILayout.Toggle(_master.DebugFishing, "Debug Fishing");
            _master.UnlockAllTech = GUILayout.Toggle(_master.UnlockAllTech, "Unlock All Tech");
            _master.DisableSkillCheckFails = GUILayout.Toggle(_master.DisableSkillCheckFails, "Disable Skill Check Fails");
            _master.DiscoverAllCodex = GUILayout.Toggle(_master.DiscoverAllCodex, "Discover All Codex");
            _master.RandomizeSeed = GUILayout.Toggle(_master.RandomizeSeed, "Randomize Seed");
            _master.Seed = (uint)INT_INPUT((int)_master.Seed, "Seed");
            _master.NewGameElapsedTime = INT_INPUT(_master.NewGameElapsedTime, "Elapsed Time");
            END_INDENT();

            if (EditorGUI.EndChangeCheck())
            {
                if (!Application.isPlaying)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
            }
        }

        SPACE(8);

        if (EditorApplication.isPlaying)
        {
#if DEV_TESTING
            _master.RenderGUI();
#endif
        }
    }
#endif

    protected virtual void FOO(string fooTxt, int fooInt) { }

    private void OverwriteFields(object a, object b)
    {
        FieldInfo[] af = a.GetType().GetFields();
        FieldInfo[] bf = b.GetType().GetFields();

        for (int n = 0; n < af.Length; n++)
        {
            object o = af[n].GetValue(a);
            bf[n].SetValue(b, o);
        }
    }

    private static void RenameAsset(UnityEngine.Object obj, string newName)
    {
        var path = AssetDatabase.GetAssetPath(obj);
        AssetDatabase.RenameAsset(path, newName);
        AssetDatabase.SaveAssets();
    }

    //void RenameSelectedPrefabs(string newName)
    //{
    //    foreach (GameObject obj in Selection.gameObjects)
    //    {
    //        string path = AssetDatabase.GetAssetPath(obj);
    //        string newPath = Path.GetDirectoryName(path) + "/" + newName + ".prefab";

    //        AssetDatabase.RenameAsset(path, newName);
    //        AssetDatabase.SaveAssets();

    //        // Update the scriptable address if needed here
    //        // Example: UpdateScriptableAddress(obj, newPath);
    //    }
    //}
    protected void GUI_SelectButton(UnityEngine.Object objToSelect)
    {
        if (GUILayout.Button("Select", GUILayout.Width(128)))
        {
            UnityEditor.Selection.objects = new UnityEngine.Object[] { objToSelect };
            GUI.FocusControl(null);
        }
    }

    // TOOLS

    private List<UnityEngine.Object> _lockedSelections = new List<UnityEngine.Object>();

    protected void ClearLockedSelections()
    {
        _lockedSelections.Clear();
    }

    protected void LockSelections()
    {
        ClearLockedSelections();

        for (int i = 0; i < UnityEditor.Selection.objects.Length; i++)
        {
            _lockedSelections.Add(UnityEditor.Selection.objects[i]);
        }
    }

    private string _token = "";
    private string _append = "";

    protected void DuplicateLockedBasedOnCurrentSelections(
        string prevName,
        string token,
        string append
    )
    {
        //if (string.IsNullOrEmpty(token)) { return; }

        for (int i = 0; i < UnityEditor.Selection.objects.Length; i++)
        {
            string newName = UnityEditor.Selection.objects[i].name;

            if (!string.IsNullOrEmpty(token))
            {
                newName = newName.Replace(token, "") + append;
            }

            DuplicateSelections(prevName, newName);
        }
    }

    protected void DuplicateSelections(string prevName, string newName)
    {
        UnityEngine.Object[] selections =
            _lockedSelections.Count > 0
                ? _lockedSelections.ToArray()
                : UnityEditor.Selection.objects;

        for (int i = 0; i < selections.Length; i++)
        {
            ScriptableObject s = selections[i] as ScriptableObject;

            if (!s.name.Contains(prevName))
            {
                Debug.LogError(s.name + " does not contain " + prevName);
                continue;
            }

            if (s != null)
            {
                string name = s.name.Replace(prevName, newName);
                ScriptableObject newObj = DataUtility.Clone(s, name);
                DataUtility.ReplaceTextInFields(newObj, prevName, newName);
            }
        }
    }

    protected void DuplicateSelectionAndUseLockNames(
        string lockedNameToRemove,
        string selectionNameToReplace
    )
    {
        if (UnityEditor.Selection.objects.Length == 0 || _lockedSelections.Count == 0)
        {
            return;
        }

        ScriptableObject objectToDupe = UnityEditor.Selection.objects[0] as ScriptableObject;

        if (objectToDupe == null)
        {
            return;
        }

        for (int i = 0; i < _lockedSelections.Count; i++)
        {
            ScriptableObject s = _lockedSelections[i] as ScriptableObject;

            if (!s.name.Contains(lockedNameToRemove))
            {
                Debug.LogError(s.name + " does not contain " + lockedNameToRemove);
                continue;
            }

            string prevName = objectToDupe.name;
            string newName = s.name.Replace(lockedNameToRemove, "");
            newName = objectToDupe.name.Replace(selectionNameToReplace, newName);
            ScriptableObject newObj = DataUtility.Clone(objectToDupe, newName);
            DataUtility.ReplaceTextInFields(newObj, prevName, newName);
        }
    }

    protected GDEAttacksData CreateNewAttack(string attackName)
    {
        GDEAttacksData attack = CreateScriptableObject<GDEAttacksData>(attackName);

        SetDataDirty();

        return attack;
    }

    protected bool ListFilter(UnityEngine.Object o)
    {
        if (o == null)
        {
            return false;
        }
        if (!string.IsNullOrEmpty(_listFilter) && !o.name.Contains(_listFilter))
        {
            return false;
        }

        return true;
    }

    protected static bool SearchFilter(UnityEngine.Object o, string searchInput)
    {
        if (o == null)
        {
            return false;
        }
        if (o.name.Contains(searchInput))
        {
            return true;
        }

        FieldInfo[] fields = o.GetType().GetFields();

        for (int j = 0; j < fields.Length; j++)
        {
            FieldInfo f = fields[j];
            object fVal = f.GetValue(o);

            if (SearchFieldFilter(fVal, searchInput))
            {
                return true;
            }
        }

        return false;
    }

    protected static bool SearchFieldFilter(object fVal, string searchInput)
    {
        if (fVal is string)
        {
            string s = fVal as string;

            if (s.Contains(searchInput))
            {
                return true;
            }
        }
        if (fVal is string[])
        {
            string[] s = fVal as string[];

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Contains(searchInput))
                {
                    return true;
                }
            }
        }
        if (fVal is List<string>)
        {
            List<string> s = fVal as List<string>;

            for (int i = 0; i < s.Count; i++)
            {
                if (s[i].Contains(searchInput))
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected bool NameExistsInList<T>(string name, List<T> l)
        where T : ScriptableObject
    {
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].name == name)
            {
                return true;
            }
        }

        return false;
    }

    // Data loading

    protected void SetDataDirty()
    {
        _dataNeedsReload = true;
    }

    protected virtual void LoadAllData()
    {
        _dataNeedsReload = false;
        _allData.Clear();
        _allDataByType.Clear();
        _scriptsByTag.Clear();
        _scriptsByKey.Clear();
        _scriptTypes.Clear();
        _dataKeysByType.Clear();

        // Load all labels.
        float loaded = 0f;

        for (int i = 0; i < DataLabels.AllLabels.Length; i++)
        {
            EditorUtility.DisplayProgressBar("Loading scripts", "", loaded);
            string label = DataLabels.AllLabels[i];

            DataUtility.ImportAllScriptables(
                label,
                (Scriptable script) =>
                {
                    _allData.Add(script);
                    script.UpdateKey();

                    if (_scriptsByKey.TryGetValue(script.Key, out var prevScript))
                    {
                        Debug.LogError(
                            $"Duplicate key! \"{script.name}.{script.Key}\" already in use by \"{prevScript.name}.{prevScript.Key}\""
                        );
                    }
                    else
                    {
                        _scriptsByKey.Add(script.Key, script);
                    }

                    if (
                        !_allDataByType.TryGetValue(
                            script.GetType().Name,
                            out List<Scriptable> list
                        )
                    )
                    {
                        list = new List<Scriptable>();
                        string scriptType = script.GetType().Name;
                        _allDataByType.Add(scriptType, list);
                        _scriptTypes.Add(scriptType);
                    }

                    list.Add(script);
                }
            );

            loaded = i + 1 / (float)DataLabels.AllLabels.Length;
        }

        EditorUtility.ClearProgressBar();

        for (int i = 0; i < _allData.Count; i++)
        {
            Scriptable script = _allData[i];

            // Load child tag objs.
            for (int n = 0; n < script.TagCount; n++)
            {
                string tagID = script.GetTagID(n);

                if (!_scriptsByTag.TryGetValue(tagID, out var list))
                {
                    list = new List<Scriptable>();
                    _scriptsByTag.Add(tagID, list);
                }

                list.Add(script);
            }
        }
    }

    public List<Scriptable> GetDataByType<T>()
        where T : Scriptable
    {
        if (_allDataByType.TryGetValue(typeof(T).Name, out var list))
        {
            return list;
        }

        return null;
    }

    public List<string> GetDataKeysByType<T>()
        where T : Scriptable
    {
        string typeName = typeof(T).Name;

        if (_dataKeysByType.TryGetValue(typeName, out var list))
        {
            return list;
        }

        list = new List<string>();

        List<Scriptable> data = GetDataByType<T>();

        for (int i = 0; data != null && i < data.Count; i++)
        {
            list.Add(data[i].Key);
        }

        _dataKeysByType.Add(typeName, list);

        return list;
    }

    private static Dictionary<string, Scriptable> _scriptsByKey =
        new Dictionary<string, Scriptable>();
    private static Dictionary<string, List<Scriptable>> _scriptsByTag =
        new Dictionary<string, List<Scriptable>>();

    public int GetScriptsByTagCount(Scriptable tag)
    {
        if (_scriptsByTag.TryGetValue(tag.name, out var list))
        {
            return list == null ? 0 : list.Count;
        }

        return 0;
    }

    public bool ScriptExists<T>(string key)
        where T : Scriptable
    {
        if (string.IsNullOrEmpty(key))
        {
            return false;
        }
        return GetDataByID<T>(key) != null;
    }

    private string _lastClosestKey = "";
    private Scriptable _lastClosestScript = null;

    public Scriptable GetClosestScript(string key)
    {
        if (key == _lastClosestKey && _lastClosestScript != null)
        {
            return _lastClosestScript;
        }

        Scriptable closest = null;

        int minimumDistance = int.MaxValue;

        for (int i = 0; i < _allData.Count; i++)
        {
            int distance = ComputeLevenshteinDistance(_allData[i].Key, key);

            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                closest = _allData[i];
            }
        }

        _lastClosestKey = key;
        _lastClosestScript = closest;

        return closest;
    }

    private static int ComputeLevenshteinDistance(string a, string b)
    {
        int n = a.Length;
        int m = b.Length;
        int[,] d = new int[n + 1, m + 1];

        if (n == 0)
            return m;
        if (m == 0)
            return n;

        for (int i = 0; i <= n; d[i, 0] = i++)
            ;
        for (int j = 0; j <= m; d[0, j] = j++)
            ;

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (b[j - 1] == a[i - 1]) ? 0 : 1;

                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost
                );
            }
        }

        return d[n, m];
    }

    public bool ScriptExists(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return false;
        }
        return _scriptsByKey.ContainsKey(key);
    }

    public bool TryGetDataByID<T>(string key, out T v)
        where T : Scriptable
    {
        v = null;

        if (_scriptsByKey.TryGetValue(key, out var script))
        {
            v = script as T;

            return v != null;
        }

        return false;
    }

    public T GetDataByID<T>(string key)
        where T : Scriptable
    {
        if (string.IsNullOrEmpty(key))
        {
            return null;
        }
        if (_scriptsByKey.TryGetValue(key, out var script))
        {
            return script as T;
        }

        return null;
    }

    public Scriptable GetDataByID(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return null;
        }
        if (_scriptsByKey.TryGetValue(key, out var script))
        {
            return script;
        }

        return null;
    }

    private List<Scriptable> NULL_TAG_LIST = new List<Scriptable>();

    public List<Scriptable> GetScriptsByTag(Scriptable tag)
    {
        if (_scriptsByTag.TryGetValue(tag.name, out var list))
        {
            return list;
        }

        return NULL_TAG_LIST;
    }

    protected void AddTagToScripts(UnityEngine.Object[] objs, Scriptable tag)
    {
        for (int n = 0; n < objs.Length; n++)
        {
            if (objs[n] is Scriptable scriptObj)
            {
                if (scriptObj.TagIDs.Contains(tag.Key))
                {
                    continue;
                }

                AddTagToScript(scriptObj, tag);
            }
        }
    }

    protected virtual void AddTagToScript(Scriptable script, Scriptable tag)
    {
        if (tag == script)
        {
            Debug.LogError("Cannot add tag to itself!");
            return;
        }

        if (!_scriptsByTag.TryGetValue(tag.name, out var list))
        {
            list = new List<Scriptable>();
            _scriptsByTag.Add(tag.name, list);
        }

        if (!list.Contains(script))
        {
            list.Add(script);
            script.AddTag(tag.Key);
        }

        MARK_DIRTY(script);
        MARK_DIRTY(tag);
    }

    protected virtual void RemoveTagFromScript(Scriptable script, Scriptable tag)
    {
        if (script == null || tag == null)
        {
            return;
        }

        if (_scriptsByTag.TryGetValue(tag.name, out var list))
        {
            list.Remove(script);

            script.RemoveTag(tag.Key);

            MARK_DIRTY(script);
            MARK_DIRTY(tag);
        }
    }

    protected bool TryGetSprite(string id, out Sprite sprite)
    {
        sprite = LoadSprite(id);

        return sprite != null;
    }

    protected Sprite GetSprite(ITooltipContent scriptObj)
    {
        Sprite sprite = null;

        if (scriptObj == null || scriptObj.TooltipData == null)
        {
            return sprite;
        }

        return LoadSprite(scriptObj.TooltipIcon);
    }

    protected Sprite LoadSprite(string id)
    {
        if (_tooltipSprites.TryGetValue(id, out Sprite sprite))
        {
            return sprite;
        }

        sprite = Resources.Load<Sprite>("Art/Icons/" + id);

        if (sprite != null)
        {
            _tooltipSprites.Add(id, sprite);
        }

        return sprite;
    }

    protected void LoadSaveData()
    {
        //if (_saveData != null) { return; }

        //string[] files = Directory.GetFiles(Application.dataPath + "/Resources_moved/DataEdit/");

        //for (int i = 0; files != null && i < files.Length; i++)
        //{
        //    if (files[i].Contains(".meta")) { continue; }
        //    string path = "Assets" + files[i].Replace(Application.dataPath, "");
        //    _saveData = AssetDatabase.LoadAssetAtPath<DataEditSaveData>(path);
        //}

        //if (_saveData == null)
        //{
        //    Debug.Log("No save data found for data edit page.");
        //}
    }

    public static IEnumerator LoadAddressableData<T>(List<T> l, string group, System.Action onDone)
        where T : ScriptableObject
    {
        AsyncOperationHandle<IList<T>> dataHandler = Addressables.LoadAssetsAsync<T>(
            group,
            (T data) =>
            {
                if (data == null)
                {
                    return;
                }

                string key = data.name;

                if (string.IsNullOrEmpty(key))
                {
                    Debug.LogError("Invalid dictionary key: " + key);
                    return;
                }

                l.Add(data);
            }
        );

        while (!dataHandler.IsDone)
        {
            yield return null;
        }

        onDone?.Invoke();
    }

    // Indexing

    protected static void ReplaceStringInField(
        UnityEngine.Object o,
        string searchInput,
        string replaceInput
    )
    {
        if (string.IsNullOrEmpty(searchInput) || string.IsNullOrEmpty(replaceInput))
        {
            return;
        }

        FieldInfo[] fields = o.GetType().GetFields();

        for (int j = 0; j < fields.Length; j++)
        {
            FieldInfo f = fields[j];
            object fVal = f.GetValue(o);

            if (fVal is string)
            {
                string s = fVal as string;

                if (s.Contains(searchInput))
                {
                    string prev = s;
                    s = s.Replace(searchInput, replaceInput);
                    Debug.Log(prev + " -> " + s);
                    f.SetValue(o, s);
                    EditorUtility.SetDirty(o);
                }
            }
            if (fVal is string[])
            {
                string[] s = fVal as string[];

                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i].Contains(searchInput))
                    {
                        string prev = s[i];
                        s[i] = s[i].Replace(searchInput, replaceInput);
                        Debug.Log(prev + " -> " + s[i]);
                        EditorUtility.SetDirty(o);
                    }
                }
            }
            if (fVal is List<string>)
            {
                List<string> s = fVal as List<string>;

                for (int i = 0; i < s.Count; i++)
                {
                    if (s[i].Contains(searchInput))
                    {
                        string prev = s[i];
                        s[i] = s[i].Replace(searchInput, replaceInput);
                        Debug.Log(prev + " -> " + s[i]);
                        EditorUtility.SetDirty(o);
                    }
                }
            }
        }
    }

    protected static List<UnityEngine.Object> LoadObjects(
        List<string> filePaths,
        string searchInput,
        System.Func<string, UnityEngine.Object> load,
        System.Func<UnityEngine.Object, string, bool> objectFilter
    )
    {
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();

        for (int i = 0; i < filePaths.Count; i++)
        {
            UnityEngine.Object o = load(filePaths[i]);

            if (o == null)
            {
                continue;
            }
            if (!objectFilter(o, searchInput))
            {
                continue;
            }

            objs.Add(o);
        }

        return objs;
    }

    protected static List<string> GetGroupsForDir(string dirPath)
    {
        List<string> folders = new List<string>();
        string[] dirs = Directory.GetDirectories(dirPath);

        for (int i = 0; dirs != null && i < dirs.Length; i++)
        {
            DirectoryInfo dir = new DirectoryInfo(dirs[i]);

            if (dir == null)
            {
                continue;
            }

            folders.Add(dir.Name);
        }

        return folders;
    }

    protected static List<string> GetFilePathsForDir(string dirPath)
    {
        List<string> filePaths = new List<string>();
        string[] dirs = Directory.GetDirectories(dirPath);

        for (int i = 0; dirs != null && i < dirs.Length; i++)
        {
            DirectoryInfo dir = new DirectoryInfo(dirs[i]);

            if (dir == null)
            {
                continue;
            }
            if (dir.Name == "Animations")
            {
                continue;
            }

            FileInfo[] files = dir.GetFiles();

            for (int n = 0; files != null && n < files.Length; n++)
            {
                FileInfo file = files[n];

                if (file.Extension != ".asset")
                {
                    continue;
                }

                string path = "Data/" + dir.Name + "/" + file.Name.Replace(".asset", "");
                filePaths.Add(path);
            }
        }

        return filePaths;
    }

    protected static ScriptableObject LoadScriptableObject(string name, string typeName)
    {
        ScriptableObject prev = Resources.Load<ScriptableObject>(
            "Data/" + typeName.Replace("GDE", "").Replace("Data", "") + "/" + name
        );

        return prev;
    }

    protected static T LoadScriptableObject<T>(string name)
        where T : ScriptableObject
    {
        T prev = Resources.Load<T>(
            "Data/" + typeof(T).Name.Replace("GDE", "").Replace("Data", "") + "/" + name
        );

        if (prev == null)
        {
            prev = DataUtility.ImportDataSingle<T>(name);
        }

        return prev;
    }

    protected static T CreateScriptableObject<T>(string name)
        where T : ScriptableObject
    {
        T prev = LoadScriptableObject<T>(name);

        if (prev != null)
        {
            return prev;
        }

        return CreateScriptableObject(name, typeof(T).Name) as T;
    }

    protected static ScriptableObject CreateScriptableObject(string name, string typeName)
    {
        ScriptableObject newSO = ScriptableObject.CreateInstance(typeName);

        if (newSO == null)
        {
            Debug.LogError("Could not create " + typeName);
            return null;
        }

        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        string dataFolder = typeName.Replace("GDE", "").Replace("Data", "");
        string labelName = dataFolder.ToLower();
        string scriptableObjectPath = string.Format(
            "Assets/Resources_moved/Data/{0}/{1}.asset",
            dataFolder,
            name
        );

        FieldInfo keyField = newSO.GetType().GetField("Key");

        if (keyField != null)
        {
            keyField.SetValue(newSO, name);
        }

        AssetDatabase.CreateAsset(newSO, scriptableObjectPath);
        AssetDatabase.SaveAssets();

        var guid = AssetDatabase.AssetPathToGUID(scriptableObjectPath);
        var entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
        entry.labels.Add(labelName);
        //entry.address = custom_address;
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

        AssetDatabase.SaveAssets();

        return newSO;
    }

    protected static T GetScriptableObjectFromSelection<T>(object[] selections)
        where T : ScriptableObject
    {
        for (int i = 0; i < selections.Length; i++)
        {
            T s = selections[i] as T;

            if (s == null)
            {
                continue;
            }

            return s;
        }

        return null;
    }

    protected static List<T> GetScriptableObjectsFromSelection<T>(object[] selections)
        where T : ScriptableObject
    {
        List<T> l = null;

        for (int i = 0; i < selections.Length; i++)
        {
            T s = selections[i] as T;

            if (s == null)
            {
                continue;
            }

            if (l == null)
                l = new List<T>();
            l.Add(s);
        }

        return l;
    }

    protected string GetFriendlyName(string name)
    {
        return _textInfo.ToTitleCase(name.Replace("_", " "));
    }

    protected void POSITIONS(SimPositions positions)
    {
        BEGIN_HOR(32);
        BTN(
            "NW",
            positions.HasPosition(BlockPoint.ZERO.NorthWest()) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO.NorthWest());
            },
            32,
            false,
            true
        );
        BTN(
            "N",
            positions.HasPosition(BlockPoint.ZERO.North()) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO.North());
            },
            32,
            false,
            true
        );
        BTN(
            "NE",
            positions.HasPosition(BlockPoint.ZERO.NorthEast()) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO.NorthEast());
            },
            32,
            false,
            true
        );
        BTN(
            "U",
            positions.HasPosition(BlockPoint.ZERO.Up()) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO.Up());
            },
            32,
            false,
            true
        );
        END_HOR();

        BEGIN_HOR(32);
        BTN(
            "W",
            positions.HasPosition(BlockPoint.ZERO.West()) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO.West());
            },
            32,
            false,
            true
        );
        BTN(
            "x",
            positions.HasPosition(BlockPoint.ZERO) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO);
            },
            32,
            false,
            true
        );
        BTN(
            "E",
            positions.HasPosition(BlockPoint.ZERO.East()) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO.East());
            },
            32,
            false,
            true
        );
        END_HOR();

        BEGIN_HOR(32);
        BTN(
            "SW",
            positions.HasPosition(BlockPoint.ZERO.SouthWest()) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO.SouthWest());
            },
            32,
            false,
            true
        );
        BTN(
            "S",
            positions.HasPosition(BlockPoint.ZERO.South()) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO.South());
            },
            32,
            false,
            true
        );
        BTN(
            "SE",
            positions.HasPosition(BlockPoint.ZERO.SouthEast()) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO.SouthEast());
            },
            32,
            false,
            true
        );
        BTN(
            "D",
            positions.HasPosition(BlockPoint.ZERO.Down()) ? Color.white : Color.grey,
            () =>
            {
                positions.TogglePosition(BlockPoint.ZERO.Down());
            },
            32,
            false,
            true
        );
        END_HOR();
    }

    protected void BEGIN_INDENT(int t = 1)
    {
        GUILayout.BeginHorizontal();
        TAB(t);
        GUILayout.BeginVertical();
    }

    protected void END_INDENT(Color clr = default)
    {
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        DRAW_BACKGROUND(clr);
        SPACE(8);
    }

    protected float TAB(int t = 1)
    {
        float f = 16f * (float)t;
        SPACE(f);
        return f;
    }

    protected void SPACE(float w)
    {
        GUILayout.Space(w);
    }

    /// <summary>
    /// Returns the height that has been rendered up to this point.
    /// i.e., The height of layouts from functions like: GUILayout.Label()
    /// This would return the height of that rendered label.
    /// </summary>
    /// <returns></returns>
    protected float RENDERED_HEIGHT()
    {
        Rect r = GUILayoutUtility.GetLastRect();
        return r.position.y + r.height;
    }

    protected float WINDOW_HEIGHT()
    {
        return _window.position.height;
    }

    protected int WINDOW_SEGMENTS(float entryWidth)
    {
        return (int)(WINDOW_WIDTH() / entryWidth);
    }

    protected float WINDOW_WIDTH()
    {
        return EditorGUIUtility.currentViewWidth;
    }

    protected float WINDOW_WIDTH_HALF()
    {
        return WINDOW_WIDTH() / 2f;
    }

    protected float WINDOW_WIDTH_QUART()
    {
        return WINDOW_WIDTH() / 4f;
    }

    protected float TXT_WIDTH(string txt)
    {
        Vector2 v = GUI.skin.label.CalcSize(new GUIContent(txt));

        return v.x;
    }

    protected int SelectionCount
    {
        get { return Selection.objects != null ? Selection.objects.Length : 0; }
    }

    protected bool IS_SELECTED(UnityEngine.Object obj)
    {
        if (Selection.objects == null)
        {
            return false;
        }

        for (int i = 0; i < Selection.objects.Length; i++)
        {
            if (Selection.objects[i] == obj)
            {
                return true;
            }
        }

        return false;
    }

    protected void CLEAR_SELECT()
    {
        UnityEditor.Selection.objects = new UnityEngine.Object[0] { };
    }

    protected void DESELECT(UnityEngine.Object o)
    {
        List<UnityEngine.Object> list = new List<UnityEngine.Object>(UnityEditor.Selection.objects);
        list.Remove(o);
        UnityEditor.Selection.objects = list.ToArray();
    }

    protected void SELECT(UnityEngine.Object o)
    {
        bool clearPrevious = Event.current.modifiers != EventModifiers.Control;

        SELECT(o, clearPrevious);
    }

    protected void SELECT(UnityEngine.Object o, bool clearPrevious)
    {
        if (o == null)
        {
            CLEAR_SELECT();
            return;
        }

        if (clearPrevious)
        {
            UnityEditor.Selection.objects = new UnityEngine.Object[] { o };
        }
        else
        {
            List<UnityEngine.Object> list = new List<UnityEngine.Object>();
            list.Add(o);
            list.AddRange(UnityEditor.Selection.objects);
            list = list.Distinct().ToList();
            UnityEditor.Selection.objects = list.ToArray();
        }
    }

    protected void BEGIN_CLR_SELECTED(string name)
    {
        if (UnityEditor.Selection.objects == null)
        {
            return;
        }
        GUI.color =
            UnityEditor.Selection.objects.Length > 0
            && UnityEditor.Selection.objects[0].name == name
                ? Color.green
                : Color.white;
    }

    protected void ERROR(string err)
    {
        LABEL("ERROR: " + err, Color.red);
    }

    protected Color COLOR_INPUT(Color clr, int width = -1)
    {
        if (width > 0)
        {
            return EditorGUILayout.ColorField(clr, GUILayout.Width(width));
        }
        else
        {
            return EditorGUILayout.ColorField(clr);
        }
    }

    protected void BEGIN_CLR(Color pass, Color fail, System.Func<bool> condition)
    {
        if (condition())
            BEGIN_CLR(pass);
        else
            BEGIN_CLR(fail);
    }

    protected void BEGIN_CLR(Color c)
    {
        GUI.color = c;
    }

    protected void END_CLR()
    {
        GUI.color = Color.white;
    }

    protected Color CLR_SELECTED(UnityEngine.Object objToCompare)
    {
        if (UnityEditor.Selection.objects == null)
        {
            return Color.white;
        }

        for (int i = 0; i < UnityEditor.Selection.objects.Length; i++)
        {
            if (UnityEditor.Selection.objects[i] == objToCompare)
            {
                return Color.green;
            }
        }

        return Color.white;
    }

    protected void FLEX_SPACE()
    {
        GUILayout.FlexibleSpace();
    }

    protected void EXPAND_WIDTH()
    {
        GUILayout.Label("HERE", GUILayout.ExpandWidth(true));
    }

    protected void BEGIN_FOCUS_INPUT(string f)
    {
        GUI.SetNextControlName(f);
    }

    //protected bool CanReadKeys()
    //{
    //    // We don't want input active when a text box is selected.
    //    return GUI.GetNameOfFocusedControl() != "text_input";
    //}

    protected void READ_FOCUS_ENTER(string f, System.Action onEnter)
    {
        if (
            Event.current.isKey
            && Event.current.keyCode == KeyCode.Return
            && GUI.GetNameOfFocusedControl() == f
        )
        {
            onEnter?.Invoke();
            _window?.Repaint();
        }
    }

    protected void READ_KEY(KeyCode key, System.Action onKey)
    {
        //if (!CanReadKeys()) { return; }
        if (Event.current.isKey && Event.current.keyCode == key)
        {
            onKey?.Invoke();
            _window?.Repaint();
        }
    }

    protected void READ_KEY_DOWN(KeyCode key, System.Action onKey)
    {
        //if (!CanReadKeys()) { return; }
        if (
            Event.current.isKey
            && Event.current.type == EventType.KeyDown
            && Event.current.keyCode == key
        )
        {
            onKey?.Invoke();
            _window?.Repaint();
        }
    }

    protected void READ_KEY_UP(KeyCode key, System.Action onKey)
    {
        //if (!CanReadKeys()) { return; }
        if (
            Event.current.isKey
            && Event.current.type == EventType.KeyUp
            && Event.current.keyCode == key
        )
        {
            onKey?.Invoke();
            _window?.Repaint();
        }
    }

    protected void BOX(int width, int height, Color clr)
    {
        GUILayout.Label("", GUILayout.Width(0), GUILayout.Height(0));
        Rect pos = GUILayoutUtility.GetLastRect();
        Rect rect = new Rect(pos.x - 1, pos.y + 3, width, height);
        EditorGUI.DrawRect(rect, clr);
    }

    protected T DROP_DOWN_MASK<T>(T o, string label, int width = -1)
        where T : Enum
    {
        BEGIN_HOR();

        if (!string.IsNullOrEmpty(label))
        {
            LABEL(label);
        }

        o = DROP_DOWN_MASK<T>(o, width);
        END_HOR();
        return o;
    }

    protected T DROP_DOWN_MASK<T>(T o, int width = -1)
        where T : Enum
    {
        if (width > 0)
        {
            return (T)(object)EditorGUILayout.EnumFlagsField(o, GUILayout.Width(width));
        }
        else
        {
            return (T)(object)EditorGUILayout.EnumFlagsField(o);
        }
    }

    protected T DROP_DOWN<T>(T o, string label, int width = -1)
        where T : Enum
    {
        BEGIN_HOR();

        if (!string.IsNullOrEmpty(label))
        {
            LABEL(label);
        }

        T e = (T)DROP_DOWN(o, width);
        END_HOR();
        return e;
    }

    protected T DROP_DOWN<T>(T o, int width = -1)
        where T : Enum
    {
        if (width > 0)
        {
            o = (T)(object)EditorGUILayout.EnumPopup(o, GUILayout.Width(width));
        }
        else
        {
            o = (T)(object)EditorGUILayout.EnumPopup(o, GUILayout.ExpandWidth(true));
        }

        return o;
    }

    protected string DROP_DOWN(string label, string selectionID, List<string> keys, int width = -1)
    {
        int index = 0;

        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] != selectionID)
            {
                continue;
            }
            index = i;
            break;
        }

        BEGIN_HOR();
        LABEL(label);

        if (width > 0)
        {
            index = EditorGUILayout.Popup(index, keys.ToArray(), GUILayout.Width(width));
        }
        else
        {
            index = EditorGUILayout.Popup(index, keys.ToArray());
        }

        END_HOR();

        if (index >= keys.Count)
        {
            return selectionID;
        }

        return keys[index];
    }

    protected string DROP_DOWN(string label, string selectionID, string[] keys, int width = -1)
    {
        int index = 0;

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] != selectionID)
            {
                continue;
            }
            index = i;
            break;
        }

        BEGIN_HOR();
        LABEL(label);

        if (width > 0)
        {
            index = EditorGUILayout.Popup(index, keys.ToArray(), GUILayout.Width(width));
        }
        else
        {
            index = EditorGUILayout.Popup(index, keys.ToArray());
        }

        END_HOR();

        if (index >= keys.Length)
        {
            return selectionID;
        }

        return keys[index];
    }
    protected AnimationCurve ANIM_CURVE(AnimationCurve curve, string label)
    {
        if (!string.IsNullOrEmpty(label))
        {
            LABEL(label);
        }

        curve = EditorGUILayout.CurveField(curve);

        return curve;
    }

    protected List<T> TOGGLE_LIST<T>(
        string label,
        List<T> toggleKeys,
        int listSize,
        System.Func<int, T> getListKey,
        System.Func<int, string> getToggleDisplay
    )
    {
        for (int i = 0; i < listSize; i++)
        {
            T key = getListKey(i);
            bool containsBiome = toggleKeys.Contains(key);
            string toggleDisplay = getToggleDisplay(i);
            bool newState = TOGGLE(
                containsBiome,
                toggleDisplay,
                toggleDisplay,
                label,
                containsBiome ? Color.green : Color.grey,
                true
            );

            if (newState != containsBiome)
            {
                if (newState)
                {
                    toggleKeys.Add(key);
                }
                else
                {
                    toggleKeys.Remove(key);
                }
            }
        }

        return toggleKeys;
    }

    private Dictionary<string, int> _expandedCounts = new Dictionary<string, int>();
    private Dictionary<string, Dictionary<string, string>> _saveKeys = new();

    protected bool EXPAND_TOGGLE(
        string labelTxt,
        Color txtColor,
        int width = -1,
        string saveGroup = ""
    )
    {
        string saveKey = labelTxt;

        if (!string.IsNullOrEmpty(saveGroup))
        {
            if (!_saveKeys.TryGetValue(labelTxt, out var keys))
            {
                keys = new Dictionary<string, string>();
                _saveKeys.Add(labelTxt, keys);
            }

            if (!keys.TryGetValue(saveGroup, out saveKey))
            {
                saveKey = labelTxt + "_" + saveGroup;
                keys.Add(saveGroup, saveKey);
            }
        }

        bool isExpanded = IsExpanded(saveKey);
        bool newExpanded = TOGGLE(isExpanded, "[-]", "[+]", labelTxt, Color.white, txtColor, width);

        if (newExpanded != isExpanded)
        {
            SetExpanded(saveKey, newExpanded);
        }

        return newExpanded;
    }

    protected bool TOGGLE(
        bool v,
        string onTxt,
        string offTxt,
        string labelTxt,
        Color txtColor,
        bool expandWidth
    )
    {
        Color c = (v ? txtColor : Color.grey);
        BEGIN_HOR();
        BTN(
            v ? onTxt : offTxt,
            c,
            () =>
            {
                v = !v;
            },
            -1,
            expandWidth,
            false
        );
        LABEL(labelTxt, txtColor);
        END_HOR();

        return v;
    }

    protected bool TOGGLE(
        bool v,
        string onTxt,
        string offTxt,
        string labelTxt,
        Color txtColor,
        int width = -1
    )
    {
        Color c = (v ? txtColor : Color.grey);
        BEGIN_HOR();
        BTN(
            v ? onTxt : offTxt,
            c,
            () =>
            {
                v = !v;
            },
            width == -1 ? TOGGLE_BTN_SIZE : width,
            false,
            false
        );
        LABEL(labelTxt, txtColor);
        END_HOR();

        return v;
    }

    protected bool TOGGLE(
        bool v,
        string onTxt,
        string offTxt,
        string labelTxt,
        Color btnColor,
        Color txtColor,
        int width = -1
    )
    {
        btnColor = (v ? btnColor : Color.grey);
        BEGIN_HOR();
        BTN(
            v ? onTxt : offTxt,
            btnColor,
            () =>
            {
                v = !v;
            },
            width == -1 ? TOGGLE_BTN_SIZE : width,
            false,
            false
        );
        LABEL(labelTxt, txtColor);
        END_HOR();

        return v;
    }

    protected bool TOGGLE(bool v, string onTxt, string offTxt, int width = -1)
    {
        Color c = (v ? Color.green : Color.grey);
        BTN(
            v ? onTxt : offTxt,
            c,
            () =>
            {
                v = !v;
            },
            width == -1 ? TOGGLE_BTN_SIZE : width
        );

        return v;
    }

    protected bool TOGGLE(bool v, int width = -1)
    {
        Color c = (v ? Color.green : Color.grey);
        BTN(
            v ? "ON" : "OFF",
            c,
            () =>
            {
                v = !v;
            },
            width == -1 ? TOGGLE_BTN_SIZE : width
        );

        return v;
    }

    protected bool TOGGLE(bool v, string txt, int width = -1)
    {
        return TOGGLE(v, txt, GUI.color, width);
    }

    protected bool TOGGLE(bool v, string txt, Color txtColor, int width = -1)
    {
        Color c = (v ? Color.green : Color.grey);
        BEGIN_HOR();
        BTN(
            v ? "ON" : "OFF",
            c,
            () =>
            {
                v = !v;
            },
            width == -1 ? TOGGLE_BTN_SIZE : width,
            false,
            false
        );
        LABEL(txt, txtColor);
        END_HOR();

        return v;
    }

    protected void ASSERT_WARNING_ICON(bool assertionPassed)
    {
        if (!assertionPassed)
        {
            WARNING_ICON();
        }
    }

    protected void WARNING_ICON()
    {
        BEGIN_HOR();
        LABEL("[");
        LABEL("!", Color.red);
        LABEL("]");
        END_HOR();
    }

    protected void WARNING(string warning)
    {
        LABEL(warning, Color.red);
    }

    protected void ASSERT_WARNING(bool assertionPassed, string warning)
    {
        if (!assertionPassed)
        {
            LABEL(warning, Color.red);
        }
    }

    protected void ASSERT_WARNING(System.Func<bool> condition, string warning)
    {
        if (!condition())
        {
            LABEL(warning, Color.red);
        }
    }

    protected void DELETE_BTN(System.Action onDelete)
    {
        BTN("x", Color.red, onDelete, 16);
    }

    protected string PASTE_BTN(string txt)
    {
        BTN(
            "p",
            Color.yellow,
            () =>
            {
                txt = _copiedText;
            },
            18
        );

        return txt;
    }

    protected void CLEAR_COPY()
    {
        _copiedText = "";
        GUIUtility.systemCopyBuffer = "";
    }

    protected void COPY_BTN(string copyTxt)
    {
        BTN(
            "c",
            Color.yellow,
            () =>
            {
                _copiedText = copyTxt;
                GUIUtility.systemCopyBuffer = copyTxt;
            },
            16
        );
    }

    protected string COMMENT(string txt)
    {
        BEGIN_CLR(ColorUtility.commentGreen);
        txt = TEXT_INPUT(txt, "Comment");
        END_CLR();

        return txt;
    }

    protected void CREATE_SCRIPT_BTN<T>(string name)
        where T : Scriptable
    {
        BTN(
            $"New ({typeof(T).Name})",
            Color.green,
            () =>
            {
                if (ScriptExists(name))
                {
                    return;
                }
                CreateScriptableObject<T>(name);
                MarkDataNeedsReload();
            }
        );
    }

    protected void CREATE_SCRIPT_BTN(string name, string typeName, string label = "")
    {
        if (string.IsNullOrEmpty(label))
        {
            label = $"New {typeName}";
        }

        BTN(
            label,
            Color.green,
            () =>
            {
                if (ScriptExists(name))
                {
                    return;
                }
                CreateScriptableObject(name, typeName);
                MarkDataNeedsReload();
            }
        );
    }

    protected void BTN(
        string label,
        Color c,
        System.Action onPress,
        int width = -1,
        bool expandWidth = true,
        bool expandHeight = false
    )
    {
        Color clr = GUI.color;
        BEGIN_CLR(c);
        BTN(label, onPress, width, expandWidth, expandHeight);
        BEGIN_CLR(clr);
    }

    protected void BTN(
        string label,
        System.Action onPress,
        int width = -1,
        bool expandWidth = true,
        bool expandHeight = false
    )
    {
        if (width != -1)
        {
            if (
                GUILayout.Button(
                    label,
                    GUILayout.Width(width + 1),
                    GUILayout.ExpandWidth(false),
                    GUILayout.ExpandHeight(expandHeight)
                )
            )
            {
                onPress?.Invoke();
            }
        }
        else
        {
            if (
                GUILayout.Button(
                    label,
                    GUILayout.ExpandWidth(expandWidth),
                    GUILayout.ExpandHeight(expandHeight)
                )
            )
            {
                onPress?.Invoke();
            }
        }
    }

    protected void BTN(string label, Color c, System.Action onPress, GUILayoutOption option)
    {
        Color clr = GUI.color;
        BEGIN_CLR(c);
        BTN(label, onPress, option);
        BEGIN_CLR(clr);
    }

    protected void BTN(string label, System.Action onPress, GUILayoutOption option)
    {
        if (GUILayout.Button(label, option))
        {
            onPress?.Invoke();
        }
    }

    protected int INT_INPUT(int v, string label, int width = -1)
    {
        BEGIN_HOR();
        LABEL(label);
        v = INT_INPUT(v, width);
        END_HOR();

        return v;
    }

    protected int INT_INPUT(int v, string label, Color clr, int width = -1)
    {
        BEGIN_HOR();
        LABEL(label, clr);
        v = INT_INPUT(v, clr, width);
        END_HOR();

        return v;
    }

    protected int INT_INPUT(int v, int width = -1)
    {
        return INT_INPUT(v, GUI.color, width);
    }

    protected int INT_INPUT(int v, Color c, int width = -1)
    {
        BEGIN_CLR(c);

        if (width > 0)
        {
            v = EditorGUILayout.IntField(v, GUILayout.Width(width));
        }
        else
        {
            v = EditorGUILayout.IntField(v);
        }

        END_CLR();

        return v;
    }

    protected float FLOAT_INPUT01(float v, int width = -1)
    {
        return Mathf.Clamp01(FLOAT_INPUT(v, "", Color.white, width));
    }

    protected float FLOAT_INPUT(float v, int width = -1)
    {
        return FLOAT_INPUT(v, "", Color.white, width);
    }

    protected float FLOAT_INPUT(float v, string txt, Color c = default, int width = -1)
    {
        if (!string.IsNullOrEmpty(txt))
        {
            BEGIN_HOR();
            LABEL(txt);
        }

        BEGIN_CLR(c);

        if (width > 0)
        {
            v = EditorGUILayout.FloatField(v, GUILayout.Width(width));
        }
        else
        {
            v = EditorGUILayout.FloatField(v);
        }

        END_CLR();

        if (!string.IsNullOrEmpty(txt))
        {
            END_HOR();
        }

        return v;
    }

    protected string DATA_ID_INPUT<T>(string dataID, string label, out T data)
        where T : Scriptable
    {
        data = GetDataByID<T>(dataID);

        BEGIN_HOR();
        bool isEmpty = string.IsNullOrEmpty(dataID);
        bool dataFound = data != null;
        Color clr = GUI.color;
        BEGIN_CLR(
            clr,
            Color.Lerp(clr, Color.black, 0.4f),
            () =>
            {
                return !isEmpty;
            }
        );
        BEGIN_CLR(
            GUI.color,
            Color.red,
            () =>
            {
                return isEmpty || dataFound;
            }
        );
        dataID = TEXT_INPUT(dataID, label);
        //ASSERT_WARNING(isEmpty || data != null, "Does not exists!");

        if (!isEmpty && data == null)
        {
            if (TryGetDataByID<T>(dataID, out var script))
            {
                data = script;
            }
            else
            {
                CREATE_SCRIPT_BTN(dataID, typeof(T).Name);
            }
        }

        if (data != null && data is Scriptable s)
        {
            BTN(
                "[]",
                ColorUtility.selectedGold,
                () =>
                {
                    SELECT(s);
                },
                18
            );
        }
        END_HOR();

        BEGIN_CLR(clr);

        return dataID;
    }

    protected string TEXT_INPUT(string txt, string label, int width = -1)
    {
        BEGIN_HOR();
        LABEL(label);
        txt = TEXT_INPUT(txt, width);
        END_HOR();

        return txt;
    }

    protected string TEXT_INPUT(string txt, int width = -1, int height = -1)
    {
        Color c = GUI.color;

        //BEGIN_HOR();
        //if (string.IsNullOrEmpty(txt) && width == -1)
        //{
        //    width = 24;
        //}

        //bool isSelected = GUI.GetNameOfFocusedControl() == "TEXT_INPUT";

        //if (isSelected)
        {
            BEGIN_DISABLED(string.IsNullOrEmpty(txt));
            COPY_BTN(txt);
            END_DISABLED();

            BEGIN_DISABLED(string.IsNullOrEmpty(_copiedText));
            txt = PASTE_BTN(txt);
            END_DISABLED();

            BEGIN_DISABLED(string.IsNullOrEmpty(txt));
            BTN(
                "clr",
                Color.white,
                () =>
                {
                    txt = "";
                },
                24
            );
            END_DISABLED();
        }

        GUI.color = c;

        if (width > 0 && height > 0)
        {
            txt = GUILayout.TextField(txt, GUILayout.Width(width), GUILayout.Height(height));
        }
        else if (width > 0)
        {
            txt = GUILayout.TextField(txt, GUILayout.Width(width));
        }
        else if (height > 0)
        {
            txt = GUILayout.TextField(txt, GUILayout.Height(height));
        }
        else
        {
            txt = GUILayout.TextField(txt, GUILayout.ExpandWidth(true), GUILayout.MinWidth(32));
        }

        //END_HOR();

        return txt;
    }

    private string _newSurrogateTagObject = "";

    protected static readonly Color SIM_COLOR = ColorUtility.ToColor32("f27f00ff");
    protected static readonly Color TARGET_COLOR = ColorUtility.ToColor32("00a303ff");
    protected static readonly Color ACTION_COLOR = ColorUtility.ToColor32("48bedbff");
    protected static readonly Color CONDITION_COLOR = ColorUtility.ToColor32("d242ecff");

    protected void RENDER_CONDITIONS_TO_ADD(
        ISimulationConditionSource source,
        bool allowNestedConditions = true
    )
    {
        SimulationCondition[] conditions = source.GetConditions();

        if (conditions == null)
        {
            conditions = new SimulationCondition[] { };
            source.SetConditions(conditions);
        }

        // Add condition
        string conditionToAdd = "";

        foreach (string key in SimulationCondition.Conditions.Keys)
        {
            //if (!allowNestedConditions && SimulationCondition.CanHaveChildrenConditions.Contains(key)) { continue; }
            BTN(
                "+" + key.Replace("SimCondition", ""),
                Color.Lerp(CONDITION_COLOR, Color.green, 0.2f),
                () =>
                {
                    conditionToAdd = key;
                }
            );
        }

        if (!string.IsNullOrEmpty(conditionToAdd))
        {
            SimulationCondition[] prev = conditions;
            conditions = new SimulationCondition[prev.Length + 1];

            for (int i = 0; i < prev.Length; i++)
            {
                conditions[i] = prev[i];
            }

            conditions[conditions.Length - 1] = SimulationCondition.GetNewCondition(conditionToAdd);
            source.SetConditions(conditions);
        }
    }

    protected void RENDER_ACTIONS_TO_ADD(
        ISimulationActionSource source,
        bool allowNestedActions = true
    )
    {
        SimulationAction[] actions = source.GetActions();

        if (actions == null)
        {
            actions = new SimulationAction[] { };
            source.SetActions(actions);
        }

        // Add action
        string actionToAdd = "";

        foreach (string key in SimulationAction.Actions.Keys)
        {
            //if (!allowNestedActions && SimulationAction.CanHaveChildrenActions.Contains(key)) { continue; }
            BTN(
                "+" + key.Replace("SimAction", ""),
                Color.Lerp(ACTION_COLOR, Color.green, 0.2f),
                () =>
                {
                    actionToAdd = key;
                }
            );
        }

        if (!string.IsNullOrEmpty(actionToAdd))
        {
            SimulationAction[] prev = actions;
            actions = new SimulationAction[prev.Length + 1];

            for (int i = 0; i < prev.Length; i++)
            {
                actions[i] = prev[i];
            }

            actions[actions.Length - 1] = SimulationAction.GetNewAction(actionToAdd);
            source.SetActions(actions);
        }
    }

    protected void HandleSimulationConditionOutput(
        EntryOutput output,
        ISimulationConditionSource source
    )
    {
        SimulationCondition[] prevArr = source.GetConditions();

        if (output.RemoveIndex > -1)
        {
            SimulationCondition[] newArr = new SimulationCondition[prevArr.Length - 1];

            for (int i = output.RemoveIndex + 1; i < prevArr.Length; i++)
            {
                prevArr[i - 1] = prevArr[i];
            }

            for (int i = 0; i < newArr.Length; i++)
            {
                newArr[i] = prevArr[i];
            }

            source.SetConditions(newArr);
        }

        if (output.ShiftUpIndex != -1)
        {
            SimulationCondition temp = prevArr[output.ShiftUpIndex - 1];
            prevArr[output.ShiftUpIndex - 1] = prevArr[output.ShiftUpIndex];
            prevArr[output.ShiftUpIndex] = temp;
        }

        if (output.ShiftDownIndex != -1)
        {
            SimulationCondition temp = prevArr[output.ShiftDownIndex + 1];
            prevArr[output.ShiftDownIndex + 1] = prevArr[output.ShiftDownIndex];
            prevArr[output.ShiftDownIndex] = temp;
        }
    }

    protected void HandleSimulationActionOutput(EntryOutput output, ISimulationActionSource source)
    {
        SimulationAction[] prevArr = source.GetActions();

        if (output.RemoveIndex > -1)
        {
            SimulationAction[] newArr = new SimulationAction[prevArr.Length - 1];

            for (int i = output.RemoveIndex + 1; i < prevArr.Length; i++)
            {
                prevArr[i - 1] = prevArr[i];
            }

            for (int i = 0; i < newArr.Length; i++)
            {
                newArr[i] = prevArr[i];
            }

            source.SetActions(newArr);
        }

        if (output.ShiftUpIndex != -1)
        {
            SimulationAction temp = prevArr[output.ShiftUpIndex - 1];
            prevArr[output.ShiftUpIndex - 1] = prevArr[output.ShiftUpIndex];
            prevArr[output.ShiftUpIndex] = temp;
        }

        if (output.ShiftDownIndex != -1)
        {
            SimulationAction temp = prevArr[output.ShiftDownIndex + 1];
            prevArr[output.ShiftDownIndex + 1] = prevArr[output.ShiftDownIndex];
            prevArr[output.ShiftDownIndex] = temp;
        }
    }

    protected void LABELS(string txt, float v, float width = -1f)
    {
        BEGIN_HOR();
        LABEL(txt, width);
        LABEL(v.ToString(), width);
        END_HOR();
    }

    protected void LABELS(string txt, int v, float width = -1f)
    {
        BEGIN_HOR();
        LABEL(txt, width);
        LABEL(v.ToString(), width);
        END_HOR();
    }

    protected void LABEL(float v, Color clr, float width = -1f)
    {
        LABEL(v.ToString(), clr, width);
    }

    protected void LABEL(int v, Color clr, float width = -1f)
    {
        if (!_intToString.TryGetValue(v, out var str))
        {
            str = v.ToString();
            _intToString.Add(v, str);
        }
        LABEL(str, clr, width);
    }

    protected void LABEL(float v, float width = -1f)
    {
        LABEL(v.ToString(), width);
    }

    protected void LABEL(int v, float width = -1f)
    {
        if (!_intToString.TryGetValue(v, out var str))
        {
            str = v.ToString();
            _intToString.Add(v, str);
        }
        LABEL(str, width);
    }

    private Dictionary<int, string> _intToString = new Dictionary<int, string>();

    protected void LABEL(string label, Color c, float width = -1f)
    {
        if (string.IsNullOrEmpty(label))
        {
            return;
        }

        Color prevClr = GUI.color;
        BEGIN_CLR(c);

        LABEL(label, width);

        BEGIN_CLR(prevClr);
    }

    protected void LABEL(string label, float width = -1f)
    {
        if (width > 0)
        {
            GUILayout.Label(label, GUILayout.Width(width));
        }
        else
        {
            GUILayout.Label(label, GUILayout.ExpandWidth(false));
        }
    }

    protected void LABEL(int v, bool expandWidth)
    {
        LABEL(v.ToString(), expandWidth);
    }

    protected void LABEL(string label, bool expandWidth)
    {
        GUILayout.Label(label, GUILayout.ExpandWidth(expandWidth));
    }

    protected void BEGIN_SPRITE(Sprite sprite)
    {
        if (sprite == null)
        {
            return;
        }
        if (_window == null)
        {
            return;
        }

        Texture2D texture = sprite.texture;
        GUILayout.BeginVertical(GUILayout.Width(texture.width * 3));
        GUILayout.Label(
            "",
            GUILayout.Width(texture.width * 3 + 12),
            GUILayout.Height(texture.height * 3)
        );
        Rect pos = GUILayoutUtility.GetLastRect();
        Rect rect = new Rect(pos.x + 16, pos.y, sprite.rect.width * 3, sprite.rect.height * 3);
        EditorGUI.DrawRect(
            new Rect(pos.x + 16, pos.y, rect.width, rect.height),
            new Color(0.1f, 0.1f, 0.1f, 1f)
        );
        GUI.DrawTexture(rect, texture);
        GUILayout.EndVertical();
    }

    protected void BEGIN_HOR(int height = -1)
    {
        if (height > 0)
        {
            GUILayout.BeginHorizontal(GUILayout.Height(height));
        }
        else
        {
            GUILayout.BeginHorizontal();
        }
    }

    protected void END_HOR()
    {
        GUILayout.EndHorizontal();
    }

    protected void BEGIN_VERT()
    {
        GUILayout.BeginVertical();
    }

    protected void END_VERT()
    {
        GUILayout.EndVertical();
    }

    protected void DRAW_BACKGROUND(Color clr = default)
    {
        Rect rect = GUILayoutUtility.GetLastRect();
        Color bgColor = new Color(clr.r, clr.g, clr.b, 0.2f);
        //EditorGUI.DrawRect(rect, bgColor);

        // Top-left corner
        EditorGUI.DrawRect(new Rect(rect.xMin + 8, rect.yMin, 3f, rect.height), bgColor);

        // Top-right corner
        //EditorGUI.DrawRect(new Rect(rect.xMax - cornerRectSize, rect.yMin, cornerRectSize, cornerRectSize), bgColor);

        // Bottom-left corner
        //EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - cornerRectSize, cornerRectSize, cornerRectSize), bgColor);

        // Bottom-right corner
        //EditorGUI.DrawRect(new Rect(rect.xMax - cornerRectSize, rect.yMax - cornerRectSize, cornerRectSize, cornerRectSize), bgColor);
    }

    private Dictionary<string, Vector2> _scrollViews = new Dictionary<string, Vector2>();

    protected void BEGIN_SCROLL(string key, float height)
    {
        if (!_scrollViews.TryGetValue(key, out var scroll))
        {
            _scrollViews.Add(key, new Vector2());
        }

        scroll = GUILayout.BeginScrollView(scroll, GUILayout.Height(height));
        _scrollViews[key] = scroll;
    }

    protected void BEGIN_SCROLL(string key = "default")
    {
        if (!_scrollViews.TryGetValue(key, out var scroll))
        {
            _scrollViews.Add(key, new Vector2());
        }

        scroll = GUILayout.BeginScrollView(scroll);
        _scrollViews[key] = scroll;
    }

    protected void END_SCROLL()
    {
        GUILayout.EndScrollView();
    }

    protected void BEGIN_DISABLED(bool enabled)
    {
        EditorGUI.BeginDisabledGroup(enabled);
    }

    protected void END_DISABLED()
    {
        EditorGUI.EndDisabledGroup();
    }

    protected void BEGIN_KEYWORD_CLR(KeywordHelperMatchStates state)
    {
        switch (state)
        {
            default:
            case KeywordHelperMatchStates.NO_KEYWORD_FOUND:
                BEGIN_CLR(Color.white);
                break;
            case KeywordHelperMatchStates.OUT_OF_BOUNDS:
                BEGIN_CLR(Color.yellow);
                break;
            case KeywordHelperMatchStates.IN_BOUNDS:
                BEGIN_CLR(Color.green);
                break;
        }
    }

    protected void MARK_DIRTY(UnityEngine.Object o)
    {
        if (EditorApplication.isUpdating || EditorApplication.isCompiling)
        {
            return;
        }

        EditorUtility.SetDirty(o);
    }

    public struct EntryOutput
    {
        public EntryOutput(
            bool changed,
            int remove,
            int shiftUp,
            int shiftDown,
            bool expand,
            bool enabled
        )
        {
            Changed = changed;
            RemoveIndex = remove;
            ShiftUpIndex = shiftUp;
            ShiftDownIndex = shiftDown;
            Expanded = expand;
            Enabled = enabled;
        }

        public bool Changed;
        public int RemoveIndex;
        public int ShiftUpIndex;
        public int ShiftDownIndex;
        public bool Expanded;
        public bool Enabled;
    }

    protected EntryOutput LIST_ENTRY_HEADER(
        int index,
        int listLength,
        string name,
        bool expanded,
        bool enabled,
        Color clr
    )
    {
        int removeIndex = -1;
        int shiftUpIndex = -1;
        int shiftDownIndex = -1;
        bool changed = false;

        BEGIN_HOR(30);
        BTN(
            "x",
            Color.red,
            () =>
            {
                changed = true;
                removeIndex = index;
            },
            16,
            false,
            true
        );
        BEGIN_DISABLED(index == 0);
        BTN(
            "^",
            () =>
            {
                changed = true;
                shiftUpIndex = index;
            },
            30,
            false,
            true
        );
        END_DISABLED();
        BEGIN_DISABLED(index == listLength - 1);
        BTN(
            "v",
            () =>
            {
                changed = true;
                shiftDownIndex = index;
            },
            30,
            false,
            true
        );
        END_DISABLED();

        BTN(
            enabled ? "E" : "D",
            enabled ? Color.green : Color.gray,
            () =>
            {
                changed = true;
                enabled = !enabled;
            },
            30,
            false,
            true
        );

        BTN(
            name,
            expanded ? clr : clr * Color.gray,
            () =>
            {
                changed = true;
                expanded = !expanded;
            },
            -1,
            true,
            true
        );

        END_HOR();

        return new EntryOutput()
        {
            Changed = changed,
            RemoveIndex = removeIndex,
            ShiftUpIndex = shiftUpIndex,
            ShiftDownIndex = shiftDownIndex,
            Expanded = expanded,
            Enabled = enabled,
        };
    }

    protected static void AddSimulation(InstanceSimulation sim, GDESimulationData simScriptObj)
    {
        InstanceSimulation[] simulations = simScriptObj.Simulations;

        if (simulations == null || simulations.Length == 0)
        {
            simulations = new InstanceSimulation[1];
            simulations[0] = sim;
            simScriptObj.Simulations = simulations;

            return;
        }

        InstanceSimulation[] prev = simulations;

        simulations = new InstanceSimulation[prev.Length + 1];

        for (int i = 0; i < prev.Length; i++)
        {
            simulations[i] = prev[i];
        }

        simulations[simulations.Length - 1] = sim;
        simScriptObj.Simulations = simulations;
    }

    protected static void RemoveSimulation(int index, GDESimulationData simScriptObj)
    {
        InstanceSimulation[] simulations = simScriptObj.Simulations;

        if (simulations == null || simulations.Length == 0)
        {
            return;
        }

        InstanceSimulation[] prev = simulations;
        prev[index] = null;

        for (int i = index + 1; i < prev.Length; i++)
        {
            prev[i - 1] = prev[i];
        }

        simulations = new InstanceSimulation[prev.Length - 1];

        for (int i = 0; i < simulations.Length; i++)
        {
            simulations[i] = prev[i];
        }

        simScriptObj.Simulations = simulations;
    }

    protected static void SwapSimulations(int a, int b, GDESimulationData simScriptObj)
    {
        InstanceSimulation[] simulations = simScriptObj.Simulations;

        InstanceSimulation temp = simulations[a];
        simulations[a] = simulations[b];
        simulations[b] = temp;
        simScriptObj.Simulations = simulations;
    }

    protected EntryOutput RenderSim(
        int index,
        ISimulationData simData,
        GDESimulationData simScriptObj,
        InstanceSimulation sim
    )
    {
        if (sim == null)
        {
            return new EntryOutput();
        }
        Color simColor = sim.IsEnabled ? SIM_COLOR : Color.gray;
        int simCount = simScriptObj.Simulations.Length;
        EntryOutput simOutput = LIST_ENTRY_HEADER(
            index,
            simCount,
            sim.Comment,
            sim.DebugMaximized,
            sim.IsEnabled,
            simColor
        );

        sim.IsEnabled = simOutput.Enabled;

        if (!simOutput.Expanded)
        {
            return simOutput;
        }

        BEGIN_HOR();
        sim.Comment = TEXT_INPUT(sim.Comment);
        END_HOR();

        BEGIN_INDENT();

        BEGIN_DISABLED(!sim.IsEnabled);
        sim.DebugBreakpoint = TOGGLE(sim.DebugBreakpoint, "Breakpoint");

        // Target
        if (sim.Target == null)
            sim.Target = new SimulationTarget();
        sim.Target = RenderSimTarget(sim.Target);

        // Conditions.
        RenderSimConditions(sim, simData);

        // Actions
        RenderSimActions(sim, simData);

        END_INDENT();
        END_DISABLED();

        return simOutput;
    }

    protected SimulationTarget RenderSimTarget(SimulationTarget target)
    {
        if (target == null)
        {
            return null;
        }
        LABEL(target.GetType().Name, TARGET_COLOR * 0.9f);

        BEGIN_INDENT(1);
        LABEL("Target Positions");
        POSITIONS(target.TargetPositions);

        target.TargetPositions.IterateToEnd = TOGGLE(
            target.TargetPositions.IterateToEnd,
            "Iterate To End"
        );

        LABEL("Max Dist");
        target.TargetPositions.MaxDistance = INT_INPUT(target.TargetPositions.MaxDistance);

        if (!target.TargetPositions.IterateToEnd)
        {
            LABEL("Min Dist");
            target.TargetPositions.MinDistance = INT_INPUT(target.TargetPositions.MinDistance);
        }

        target.TargetPositions.MaxDistance = Mathf.Max(
            target.TargetPositions.MaxDistance,
            target.TargetPositions.MinDistance
        );

        LABEL("Check All Positions");
        target.TargetPositions.CheckAll = TOGGLE(target.TargetPositions.CheckAll);

        if (target.TargetPositions.Positions.Length == 0)
        {
            LABEL("No positions are being used!", Color.yellow);
        }

        END_INDENT();
        return target;
    }

    protected void RenderSimConditions(
        ISimulationConditionSource sim,
        ISimulationData simData,
        bool allowNestedConditions = true
    )
    {
        // Condtiions
        LABEL("Conditions", CONDITION_COLOR * 0.9f);

        // sim.RequireAllConditionsToBeMet = TOGGLE(sim.RequireAllConditionsToBeMet, "Require All Conditions To Pass");

        SimulationCondition[] simConditions = sim.GetConditions();

        // Render conditions
        for (int i = 0; simConditions != null && i < simConditions.Length; i++)
        {
            if (simConditions[i] == null)
            {
                simConditions[i] = new SimulationCondition();
            }

            SimulationCondition condition = simConditions[i];

            EntryOutput output = RenderSimCondition(i, simData, sim, condition);

            if (output.Changed)
            {
                HandleSimulationConditionOutput(output, sim);
                break;
            }
        }

        BEGIN_INDENT();
        RENDER_CONDITIONS_TO_ADD(sim, allowNestedConditions);
        END_INDENT();
    }

    protected EntryOutput RenderSimCondition(
        int index,
        ISimulationData simData,
        ISimulationConditionSource sim,
        SimulationCondition condition
    )
    {
        BEGIN_INDENT();

        SimulationCondition[] conditions = sim.GetConditions();

        Color conditionColor = condition.Enabled ? CONDITION_COLOR : Color.gray;
        EntryOutput output = LIST_ENTRY_HEADER(
            index,
            conditions.Length,
            condition.GetDisplayName(),
            condition.Expanded,
            condition.Enabled,
            conditionColor
        );
        condition.Expanded = output.Expanded;
        condition.Enabled = output.Enabled;

        if (condition.Expanded && condition.Enabled)
        {
            BEGIN_INDENT();

            BEGIN_HOR();
            condition.Outcome = TOGGLE(condition.Outcome, "MUST PASS", "MUST FAIL", 100);
            FLEX_SPACE();
            condition.DebugBreakpoint = TOGGLE(
                condition.DebugBreakpoint,
                "Breakpoint",
                Color.yellow
            );
            //LABEL(string.Format("{0}", condition.Outcome ? "MUST PASS" : "MUST FAIL"), condition.Outcome ? Color.green : Color.red);
            condition.TEST = TOGGLE(condition.TEST, "TEST", Color.yellow);
            condition.FORCE_PASS = TOGGLE(condition.FORCE_PASS, "FORCE PASS", Color.yellow);
            //FLEX_SPACE();
            END_HOR();

            //LABEL("ConditionID (Optional) - This can be used to filter nested conditions.");
            //condition.ConditionID = TEXT_INPUT(condition.ConditionID);

            //if (condition is CheckInternalCondition internalSimCondition)
            //{
            //    GUI.color = Color.red;

            //    LABEL("EventIDMatch (Optional) - Keep empty if you want all nested conditions to be checked.");
            //    internalSimCondition.EventIDMatch = TEXT_INPUT(internalSimCondition.EventIDMatch);
            //    RenderSimConditions(simData, simData, false);
            //    GUI.color = Color.white;
            //}
            //else
            if (condition is HasSimStateSimCondition hasSimStateCondition)
            {
                string[] simStates = simData.GetSimStates();

                for (int i = 0; simStates != null && i < simStates.Length; i++)
                {
                    bool isMatch = simStates[i] == hasSimStateCondition.StateID;
                    Color c = isMatch ? Color.green : Color.grey;

                    BTN(
                        simStates[i],
                        c,
                        () =>
                        {
                            if (hasSimStateCondition.StateID == simStates[i])
                            {
                                hasSimStateCondition.StateID = "";
                            }
                            else
                            {
                                hasSimStateCondition.StateID = simStates[i];
                            }
                        }
                    );

                    SimOptions[] simOptions = simData.GetOptions();

                    if (simOptions != null && isMatch)
                    {
                        BEGIN_INDENT();

                        for (int n = 0; n < simOptions.Length; n++)
                        {
                            Color cc =
                                simOptions[n].OptionID == hasSimStateCondition.OptionID
                                    ? Color.green
                                    : Color.grey;

                            if (string.IsNullOrEmpty(simOptions[n].TagObjectID))
                            {
                                cc = Color.red;
                            }

                            BTN(
                                simOptions[n].OptionID + "->" + simOptions[n].TagObjectID,
                                cc,
                                () =>
                                {
                                    if (hasSimStateCondition.OptionID == simOptions[n].OptionID)
                                    {
                                        hasSimStateCondition.OptionID = "";
                                    }
                                    else
                                    {
                                        hasSimStateCondition.OptionID = simOptions[n].OptionID;
                                    }
                                }
                            );
                        }

                        END_INDENT();
                    }
                }

                if (simStates == null || simStates.Length == 0)
                {
                    ERROR(simData + " does not have any sim states implemented!");
                }
            }
            else if (condition is HasSkylightSimCondition hasSkylightCondition)
            {
                LABEL("Max Amount (0-7)");
                hasSkylightCondition.MaxAmount = Mathf.Clamp(
                    INT_INPUT(hasSkylightCondition.MaxAmount),
                    0,
                    7
                );

                LABEL("Min Amount (0-7)");
                hasSkylightCondition.MinAmount = Mathf.Clamp(
                    INT_INPUT(hasSkylightCondition.MinAmount),
                    0,
                    7
                );
            }
            else if (condition is MaxGroupRadiusSimCondition maxRadiusCondition)
            {
                LABEL("Max Radius %");
                maxRadiusCondition.MaxRadiusPercent = FLOAT_INPUT(
                    maxRadiusCondition.MaxRadiusPercent
                );
            }
            else if (condition is InGroupVerticalBoundsSimCondition vertBoundsCondition)
            {
                LABEL("Top Buffer");
                vertBoundsCondition.TopBuffer = INT_INPUT(vertBoundsCondition.TopBuffer);

                LABEL("Bottom Buffer");
                vertBoundsCondition.BottomBuffer = INT_INPUT(vertBoundsCondition.BottomBuffer);
            }
            else if (condition is CanPathToSimCondition canPathCondition)
            {
                LABEL("Can path to - Activate if there is a path into our target location");
                canPathCondition.Exit = TOGGLE(
                    canPathCondition.Exit,
                    "Exit - does the origin allow pathing to the target direction"
                );
                canPathCondition.Entry = TOGGLE(
                    canPathCondition.Entry,
                    "Entry - does target allow pathing from the origin direction"
                );
            }
            else if (condition is HasBlockLayerSimCondition hasLayerCondition)
            {
                LABEL("Block Layer");
                hasLayerCondition.Layer = (BlockLayers)DROP_DOWN_MASK(hasLayerCondition.Layer);
            }
            else if (condition is HasInstanceSimCondition hasInstanceCondition)
            {
                if (string.IsNullOrEmpty(hasInstanceCondition.TagObjectKey))
                {
                    LABEL("EMPTY - Using origin instance object key", Color.yellow);
                }

                BEGIN_HOR();
                LABEL("Tag Object Key");
                hasInstanceCondition.TagObjectKey = TEXT_INPUT(hasInstanceCondition.TagObjectKey);
                END_HOR();

                BEGIN_HOR();
                LABEL("Sim State");
                string[] simStates = simData.GetSimStates();

                for (int i = 0; simStates != null && i < simStates.Length; i++)
                {
                    bool isMatch = simStates[i] == hasInstanceCondition.StateID;
                    Color c = isMatch ? Color.green : Color.grey;

                    BTN(
                        simStates[i],
                        c,
                        () =>
                        {
                            if (hasInstanceCondition.StateID == simStates[i])
                            {
                                hasInstanceCondition.StateID = "";
                            }
                            else
                            {
                                hasInstanceCondition.StateID = simStates[i];
                            }
                        }
                    );

                    SimOptions[] simOptions = simData.GetOptions();

                    if (simOptions != null && isMatch)
                    {
                        BEGIN_INDENT();

                        for (int n = 0; n < simOptions.Length; n++)
                        {
                            Color cc =
                                simOptions[n].OptionID == hasInstanceCondition.OptionID
                                    ? Color.green
                                    : Color.grey;

                            if (string.IsNullOrEmpty(simOptions[n].TagObjectID))
                            {
                                cc = Color.red;
                            }

                            BTN(
                                simOptions[n].OptionID + "->" + simOptions[n].TagObjectID,
                                cc,
                                () =>
                                {
                                    if (hasInstanceCondition.OptionID == simOptions[n].OptionID)
                                    {
                                        hasInstanceCondition.OptionID = "";
                                    }
                                    else
                                    {
                                        hasInstanceCondition.OptionID = simOptions[n].OptionID;
                                    }
                                }
                            );
                        }

                        END_INDENT();
                    }
                }
                END_HOR();
            }
            else if (condition is HasNeighborSimCondition hasNeighborCondition)
            {
                POSITIONS(hasNeighborCondition.NeighborPositions);

                LABEL("Neighbor Conditions");
                for (int i = 0; i < hasNeighborCondition.NeighborConditions.Length; i++)
                {
                    EntryOutput neighborOutput = RenderSimCondition(
                        index,
                        simData,
                        hasNeighborCondition,
                        hasNeighborCondition.NeighborConditions[i]
                    );

                    if (neighborOutput.Changed)
                    {
                        HandleSimulationConditionOutput(neighborOutput, hasNeighborCondition);
                        break;
                    }
                }

                BEGIN_INDENT();
                RENDER_CONDITIONS_TO_ADD(hasNeighborCondition);
                END_INDENT();
            }
            else if (condition is CanSupportWeightSimCondition supportWeightCondition)
            {
                LABEL(
                    "Can support weight - Activate if target location doesn't have a path downwards"
                );
            }
            else if (condition is Vec2RelativeSizeInGroupSimCondition vec2RelativeSizeCondition)
            {
                LABEL("Max Size");
                vec2RelativeSizeCondition.MaxRelativeSize = FLOAT_INPUT(
                    vec2RelativeSizeCondition.MaxRelativeSize
                );

                LABEL("Min Size");
                vec2RelativeSizeCondition.MinRelativeSize = FLOAT_INPUT(
                    vec2RelativeSizeCondition.MinRelativeSize
                );

                vec2RelativeSizeCondition.IncludeInstancesOfSameObjectKey = TOGGLE(
                    vec2RelativeSizeCondition.IncludeInstancesOfSameObjectKey,
                    "Include Instances Of Same Object Key"
                );
            }
            else if (condition is Vec2DistanceFromGroupCentreSimCondition vec2DistanceCondition)
            {
                LABEL("Max Distance");
                vec2DistanceCondition.MaxDistance = FLOAT_INPUT(vec2DistanceCondition.MaxDistance);

                LABEL("Min Distance");
                vec2DistanceCondition.MinDistance = FLOAT_INPUT(vec2DistanceCondition.MinDistance);
            }
            //else if (condition is CanAddTagObjectToLocationSimCondition canAddTagObjectCondition)
            //{

            //    if (string.IsNullOrEmpty(canAddTagObjectCondition.TagObjectKey))
            //    {
            //        LABEL("EMPTY - Using origin instance object key", Color.yellow);
            //    }

            //    LABEL("Tag Object Key - Activate if this tag object can be added to the target");
            //    canAddTagObjectCondition.TagObjectKey = TEXT_INPUT(canAddTagObjectCondition.TagObjectKey);
            //}
            else if (condition is RandomChanceSimCondition randomChanceCondition)
            {
                LABEL($"chances: {condition.GetDisplayName()}");
                randomChanceCondition.MaxRange = (uint)
                    EditorGUILayout.IntSlider(
                        (int)randomChanceCondition.MaxRange,
                        1,
                        (int)RandomChanceSimCondition.MAX_RANGE
                    );
            }
            //else if (condition is InstanceCountSimCondition instanceCountCondition)
            //{
            //    LABEL("Count Ceiling");
            //    instanceCountCondition.CountCeiling = INT_INPUT(instanceCountCondition.CountCeiling);
            //    LABEL("Count Floor");
            //    instanceCountCondition.CountFloor = INT_INPUT(instanceCountCondition.CountFloor);
            //}

            //SPACE(10);
            END_INDENT();
        }

        END_INDENT();

        return output;
    }

    protected void RenderSimActions(
        ISimulationActionSource sim,
        ISimulationData simData,
        bool allowNestedActions = true
    )
    {
        LABEL("Actions", ACTION_COLOR * 0.9f);

        SimulationAction[] actions = sim.GetActions();

        // Render actions
        for (int i = 0; actions != null && i < actions.Length; i++)
        {
            if (actions[i] == null)
            {
                actions[i] = new SimulationAction();
                break;
            }

            EntryOutput output = RenderSimAction(i, sim, simData, actions[i]);

            if (output.Changed)
            {
                HandleSimulationActionOutput(output, sim);
                break;
            }
        }

        BEGIN_INDENT();
        RENDER_ACTIONS_TO_ADD(sim, allowNestedActions);
        END_INDENT();
    }

    protected EntryOutput RenderSimAction(
        int index,
        ISimulationActionSource sim,
        ISimulationData simData,
        SimulationAction action
    )
    {
        BEGIN_INDENT();
        SimulationAction[] actions = sim.GetActions();
        Color actionColor = action.Enabled ? ACTION_COLOR : Color.gray;
        EntryOutput output = LIST_ENTRY_HEADER(
            index,
            actions.Length,
            action.GetDisplayName(),
            action.Expanded,
            action.Enabled,
            actionColor
        );
        action.Expanded = output.Expanded;
        action.Enabled = output.Enabled;

        if (action.Expanded && action.Enabled)
        {
            BEGIN_INDENT();

            BEGIN_HOR();
            action.DebugBreakpoint = TOGGLE(action.DebugBreakpoint, "Breakpoint");
            END_HOR();

            //LABEL("ActionID (Optional) - This can be used to filter nested actions.");
            //action.ActionID = TEXT_INPUT(action.ActionID);

            //if (action is SimulateInternalAction internalAction)
            //{
            //    GUI.color = Color.red;
            //    LABEL("EventIDMatch (Optional) - Keep empty if you want all nested actions to be checked.");
            //    internalAction.EventIDMatch = TEXT_INPUT(internalAction.EventIDMatch);
            //    BEGIN_SPRITE(simData.TooltipIconSprite);
            //    RenderSimActions(simData, simData, false);
            //    GUI.color = Color.white;
            //}
            //else
            if (action is ActivateStateSimAction activateStateSimAction)
            {
                activateStateSimAction.UseGroupUID = TOGGLE(
                    activateStateSimAction.UseGroupUID,
                    "Use Group UID",
                    Color.magenta
                );

                string[] simStates = simData.GetSimStates();

                for (int i = 0; simStates != null && i < simStates.Length; i++)
                {
                    bool isMatch = simStates[i] == activateStateSimAction.StateID;
                    Color c = isMatch ? Color.green : Color.grey;
                    BTN(
                        simStates[i],
                        c,
                        () =>
                        {
                            if (activateStateSimAction.StateID == simStates[i])
                            {
                                activateStateSimAction.StateID = "";
                            }
                            else
                            {
                                activateStateSimAction.StateID = simStates[i];
                            }
                        }
                    );

                    SimOptions[] simOptions = simData.GetOptions();

                    if (simOptions != null && isMatch)
                    {
                        BEGIN_INDENT();

                        for (int n = 0; n < simOptions.Length; n++)
                        {
                            Color cc =
                                simOptions[n].OptionID == activateStateSimAction.OptionID
                                    ? Color.green
                                    : Color.grey;

                            if (string.IsNullOrEmpty(simOptions[n].TagObjectID))
                            {
                                cc = Color.red;
                            }

                            BTN(
                                simOptions[n].OptionID + "->" + simOptions[n].TagObjectID,
                                cc,
                                () =>
                                {
                                    if (activateStateSimAction.OptionID == simOptions[n].OptionID)
                                    {
                                        activateStateSimAction.OptionID = "";
                                    }
                                    else
                                    {
                                        activateStateSimAction.OptionID = simOptions[n].OptionID;
                                    }
                                }
                            );
                        }

                        END_INDENT();
                    }
                }

                if (simStates == null || simStates.Length == 0)
                {
                    ERROR(simData + " does not have any sim states implemented!");
                }
            }
            else if (action is SpawnInstanceFromTagSimAction spawnInstanceFromTagAction)
            {
                LABEL("If the origin has a group UID, inherit it, otherwise, create a new one");
                spawnInstanceFromTagAction.UseGroupUID = TOGGLE(
                    spawnInstanceFromTagAction.UseGroupUID,
                    "Use Group UID"
                );
                LABEL("Spawn tag - Spawn a new tag object instance using a Tag");
                spawnInstanceFromTagAction.SpawnTag = GUILayout.TextField(
                    spawnInstanceFromTagAction.SpawnTag
                );
            }
            else if (action is MoveInstanceSimAction moveAction)
            {
                //LABEL("Distance");
                //moveAction.Distance = INT_INPUT(moveAction.Distance);
            }
            else if (action is SpawnInstanceFromTagObjectSimAction spawnInstanceFromTagObjectAction)
            {
                LABEL("If the origin has a group UID, inherit it, otherwise, create a new one");
                spawnInstanceFromTagObjectAction.UseGroupUID = TOGGLE(
                    spawnInstanceFromTagObjectAction.UseGroupUID,
                    "Use Group UID"
                );
                LABEL("Tag Object Key - Spawn a new tag object instance using a TagObject");
                spawnInstanceFromTagObjectAction.TagObjectKey = GUILayout.TextField(
                    spawnInstanceFromTagObjectAction.TagObjectKey
                );
            }
            else if (action is DuplicateInstanceSimAction duplicateInstanceSimAction)
            {
                LABEL("If the origin has a group UID, inherit it, otherwise, create a new one");
                duplicateInstanceSimAction.UseGroupUID = TOGGLE(
                    duplicateInstanceSimAction.UseGroupUID,
                    "Use Group UID"
                );
            }
            else if (action is ClearSimAction clearSimAction)
            {
                LABEL("Layers To Clear");
                clearSimAction.Clear.Layer = (BlockLayers)DROP_DOWN_MASK(
                    clearSimAction.Clear.Layer
                );
                clearSimAction.Clear.DisableRemoveSpawns = TOGGLE(
                    clearSimAction.Clear.DisableRemoveSpawns,
                    "Disable Remove Drops"
                );
            }

            SPACE(10);
            END_INDENT();
        }
        END_INDENT();

        return output;
    }

    protected uint TIME_INPUT(uint minutes, string label)
    {
        BEGIN_HOR();
        LABEL(label);
        minutes = (uint)INT_INPUT((int)minutes, 80);
        LABEL("(");
        LABEL(minutes / (60));
        LABEL("hrs,");
        LABEL(minutes / (60 * 24));
        LABEL("days)");
        END_HOR();
        return minutes;
    }

    protected uint CLOCK_INPUT(uint minutes, string label)
    {
        BEGIN_HOR();
        LABEL(label);
        minutes = (uint)INT_INPUT((int)minutes, 80);
        LABEL("(");

        uint h = minutes / 60;
        uint m = minutes % 60;
        string meridiem = "AM";

        if (h == 0 || h == 24)
        {
            h = 12; // Midnight
        }
        else if (h >= 12)
        {
            meridiem = "PM";
            if (h > 12)
            {
                h -= 12;
            }
        }

        LABEL(string.Format("{0:D2}:{1:D2}{2}", h, m, meridiem));
        LABEL(")");
        END_HOR();
        return minutes;
    }

    protected RandomChance RANDOM_CHANCE_INPUT(RandomChance randomChance, string label)
    {
        BEGIN_HOR();
        LABEL(label);
        randomChance.Chance = (uint)INT_INPUT((int)randomChance.Chance, 48);
        LABEL("in", 18);
        randomChance.SampleSize = (uint)INT_INPUT((int)randomChance.SampleSize, 48);
        LABEL("(");
        LABEL(randomChance.Percentage * 100);
        LABEL("%)");
        END_HOR();

        return randomChance;
    }

    protected List<T> RENDER_VALUE_LIST<T>(List<T> l, string label, Func<T, T> renderFunc)
    {
        if (l == null)
        {
            l = new List<T>();
        }

        BEGIN_HOR();
        LABEL(label);
        LABEL(l.Count);
        END_HOR();
        BEGIN_INDENT();

        for (int i = 0; i < l.Count; i++)
        {
            BEGIN_HOR();
            l = LIST_REMOVE_BTN<T>(l, i);
            END_HOR();

            if (i >= l.Count)
            {
                break;
            }

            BEGIN_INDENT();
            l[i] = renderFunc(l[i]);
            END_INDENT();
        }

        l = LIST_ADD_VALUE_BTN<T>(l);
        END_INDENT();

        return l;
    }

    protected List<T> RENDER_REF_LIST<T>(List<T> l, string label, Action<T> renderFunc)
        where T : new()
    {
        if (l == null)
        {
            l = new List<T>();
        }

        BEGIN_HOR();
        LABEL(label);
        LABEL(l.Count);
        END_HOR();
        BEGIN_INDENT();

        for (int i = 0; i < l.Count; i++)
        {
            BEGIN_HOR();
            l = LIST_REMOVE_BTN<T>(l, i);
            END_HOR();

            if (i >= l.Count)
            {
                break;
            }

            BEGIN_INDENT();
            renderFunc(l[i]);
            END_INDENT();
        }

        l = LIST_ADD_REF_BTN<T>(l);
        END_INDENT();

        return l;
    }

    protected T[] RENDER_VALUE_ARRAY<T>(T[] arr, string label, Func<T, T> renderFunc)
    {
        if (arr == null)
        {
            return arr;
        }

        BEGIN_HOR();
        BEGIN_CLR(arr.Length == 0 ? Color.grey : Color.green);
        bool showArray = EXPAND_TOGGLE(label, GUI.color, 32, "render_value_array");
        LABEL(arr.Length);
        END_CLR();
        FLEX_SPACE();
        END_HOR();

        if (showArray)
        {
            BEGIN_INDENT();

            for (int i = 0; i < arr.Length; i++)
            {
                BEGIN_HOR();
                arr = ARRAY_REMOVE_BTN<T>(arr, i);
                END_HOR();

                if (i >= arr.Length)
                {
                    break;
                }

                BEGIN_INDENT();
                arr[i] = renderFunc(arr[i]);
                END_INDENT();
            }

            arr = ARRAY_ADD_VALUE_BTN(arr);
            END_INDENT();
        }

        return arr;
    }

    protected T[] RENDER_REF_ARRAY<T>(T[] arr, string label, Action<T> renderFunc)
        where T : new()
    {
        if (arr == null)
        {
            return arr;
        }

        BEGIN_HOR();
        BEGIN_CLR(arr.Length == 0 ? Color.grey : Color.green);
        bool showArray = EXPAND_TOGGLE(label, GUI.color, 32, "render_ref_array");
        LABEL(arr.Length);
        END_CLR();
        FLEX_SPACE();
        END_HOR();

        if (showArray)
        {
            BEGIN_INDENT();

            for (int i = 0; i < arr.Length; i++)
            {
                BEGIN_HOR();
                arr = ARRAY_REMOVE_BTN<T>(arr, i);
                END_HOR();

                if (i >= arr.Length)
                {
                    break;
                }

                BEGIN_INDENT();
                renderFunc(arr[i]);
                END_INDENT();
            }

            arr = ARRAY_ADD_REF_BTN(arr);
            END_INDENT();
        }

        return arr;
    }

    protected List<T> LIST_ADD_REF_BTN<T>(List<T> l, string label = "")
        where T : new()
    {
        BEGIN_HOR();
        LABEL(l == null ? 0 : l.Count, Color.grey);
        BTN(
            string.IsNullOrEmpty(label) ? "+" : label,
            Color.green,
            () =>
            {
                T entry = new();
                l.Add(entry);
            }
        );
        END_HOR();

        return l;
    }

    protected List<T> LIST_ADD_VALUE_BTN<T>(List<T> l, string label = "")
    {
        BEGIN_HOR();
        LABEL(l == null ? 0 : l.Count, Color.grey);
        BTN(
            string.IsNullOrEmpty(label) ? "+" : label,
            Color.green,
            () =>
            {
                l.Add(default(T));
            }
        );
        END_HOR();

        return l;
    }

    protected List<T> LIST_REMOVE_BTN<T>(List<T> l, int entryIndex)
    {
        if (l == null)
        {
            return l;
        }

        LABEL(entryIndex, Color.grey);
        BTN(
            "x",
            Color.red,
            () =>
            {
                l.RemoveAt(entryIndex);
            },
            16,
            false,
            false
        );

        return l;
    }

    protected bool TRY_LIST_REMOVE_BTN<T>(List<T> l, int entryIndex, out List<T> newList)
    {
        newList = l;

        if (l == null)
        {
            return false;
        }

        bool deleted = false;

        LABEL(entryIndex, Color.grey);
        BTN(
            "x",
            Color.red,
            () =>
            {
                deleted = true;
                l.RemoveAt(entryIndex);
            },
            16,
            false,
            false
        );

        newList = l;

        return deleted;
    }

    protected T[] ARRAY_ADD_REF_BTN<T>(T[] arr, string label = "")
        where T : new()
    {
        BEGIN_HOR();
        LABEL(arr == null ? 0 : arr.Length, Color.grey);
        BTN(
            string.IsNullOrEmpty(label) ? "+" : label,
            Color.green,
            () =>
            {
                arr = AddNonNullToArray(arr, default(T));
            }
        );
        END_HOR();

        return arr;
    }

    protected T[] ARRAY_ADD_VALUE_BTN<T>(T[] arr, string label = "")
    {
        BEGIN_HOR();
        LABEL(arr == null ? 0 : arr.Length, Color.grey);
        BTN(
            string.IsNullOrEmpty(label) ? "+" : label,
            Color.green,
            () =>
            {
                arr = AddToArray(arr, default(T));
            }
        );
        END_HOR();

        return arr;
    }

    protected bool TRY_ARRAY_DUPLICATE_BTN<T>(T[] arr, int entryIndex, out T[] newArr)
    {
        newArr = arr;

        if (arr == null)
        {
            return false;
        }

        bool deleted = false;

        BTN(
            "D",
            Color.yellow,
            () =>
            {
                deleted = true;
                arr = DuplicateArrayEntry(arr, entryIndex);
            },
            18,
            false,
            false
        );

        newArr = arr;

        return deleted;
    }

    protected bool TRY_ARRAY_REMOVE_BTN<T>(T[] arr, int entryIndex, out T[] newArr)
    {
        newArr = arr;

        if (arr == null)
        {
            return false;
        }

        bool deleted = false;

        LABEL(entryIndex, Color.grey);
        BTN(
            "x",
            Color.red,
            () =>
            {
                deleted = true;
                arr = RemoveFromArray(arr, entryIndex);
            },
            16,
            false,
            false
        );

        newArr = arr;

        return deleted;
    }

    protected T[] ARRAY_UP_BTN<T>(T[] arr, int entryIndex)
    {
        if (arr == null)
        {
            return arr;
        }

        bool canMoveUp = entryIndex > 0;
        Color c = GUI.color;
        BTN(
            "^",
            canMoveUp ? Color.cyan : Color.grey,
            () =>
            {
                if (canMoveUp)
                {
                    arr = MoveUpInArray(arr, entryIndex);
                }
            },
            16,
            false,
            false
        );
        GUI.color = c;
        return arr;
    }

    protected T[] ARRAY_DOWN_BTN<T>(T[] arr, int entryIndex)
    {
        if (arr == null)
        {
            return arr;
        }

        bool canMoveDown = entryIndex < arr.Length - 1;
        Color c = GUI.color;
        BTN(
            "v",
            canMoveDown ? Color.cyan : Color.grey,
            () =>
            {
                if (canMoveDown)
                {
                    arr = MoveDownInArray(arr, entryIndex);
                }
            },
            16,
            false,
            false
        );
        GUI.color = c;
        return arr;
    }

    protected T[] ARRAY_REMOVE_BTN<T>(T[] arr, int entryIndex)
    {
        if (arr == null)
        {
            return arr;
        }

        Color c = GUI.color;
        LABEL(entryIndex, Color.grey);
        BTN(
            "x",
            Color.red,
            () =>
            {
                arr = RemoveFromArray(arr, entryIndex);
            },
            16,
            false,
            false
        );
        GUI.color = c;
        return arr;
    }

    protected T[] AddNonNullToArray<T>(T[] arr, T entry)
        where T : new()
    {
        if (entry == null)
        {
            entry = new();
        }

        if (arr == null)
            return new T[] { entry };

        T[] prevArr = arr;
        arr = new T[prevArr.Length + 1];

        for (int i = 0; i < prevArr.Length; i++)
        {
            arr[i] = prevArr[i];
        }

        arr[arr.Length - 1] = entry;

        return arr;
    }

    protected T[] AddToArray<T>(T[] arr, T entry)
    {
        if (arr == null)
            return new T[] { entry };

        T[] prevArr = arr;
        arr = new T[prevArr.Length + 1];

        for (int i = 0; i < prevArr.Length; i++)
        {
            arr[i] = prevArr[i];
        }

        arr[arr.Length - 1] = entry;

        return arr;
    }

    protected T[] DuplicateArrayEntry<T>(T[] arr, int index)
    {
        if (index < 0 || index >= arr.Length)
        {
            return arr;
        }
        List<T> tempList = new List<T>(arr);
        T entry = tempList[index];
        tempList.Insert(index, entry);
        return tempList.ToArray();
    }

    protected T[] MoveUpInArray<T>(T[] arr, int index)
    {
        if (index < 1 || index >= arr.Length)
        {
            return arr;
        }
        List<T> tempList = new List<T>(arr);
        T entry = tempList[index];
        tempList.RemoveAt(index);
        tempList.Insert(index - 1, entry);
        return tempList.ToArray();
    }

    protected T[] MoveDownInArray<T>(T[] arr, int index)
    {
        if (index < 0 || index >= arr.Length - 1)
        {
            return arr;
        }
        List<T> tempList = new List<T>(arr);
        T entry = tempList[index];
        tempList.RemoveAt(index);
        tempList.Insert(index + 1, entry);
        return tempList.ToArray();
    }

    protected T[] RemoveFromArray<T>(T[] arr, int index)
    {
        List<T> tempList = new List<T>(arr);
        tempList.RemoveAt(index);
        return tempList.ToArray();
    }

    protected void SAVE(string key, List<string> stringValues)
    {
        SAVE(key, stringValues.Count);

        for (int i = 0; i < stringValues.Count; i++)
        {
            SAVE(key + i, stringValues[i]);
        }
    }

    protected void SAVE(string key, string stringValue)
    {
        PlayerPrefs.SetString(key, stringValue);
        PlayerPrefs.Save();
    }

    protected void SAVE(string key, float floatValue)
    {
        PlayerPrefs.SetFloat(key, floatValue);
        PlayerPrefs.Save();
    }

    protected void SAVE(string key, int intValue)
    {
        PlayerPrefs.SetInt(key, intValue);
        PlayerPrefs.Save();
    }

    protected List<string> LOAD_STRINGS(string key)
    {
        List<string> l = new List<string>();
        int count = LOAD_INT(key);

        for (int i = 0; i < count; i++)
        {
            l.Add(LOAD_STRING(key + i));
        }

        return l;
    }

    protected string LOAD_STRING(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    protected float LOAD_FLOAT(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    protected int LOAD_INT(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    protected KeywordHelperMatchStates GetKeywordHelperStateInMinMaxBounds(
        TuningKeyWordHelper helper,
        int amount
    )
    {
        if (helper == null)
        {
            return KeywordHelperMatchStates.NO_KEYWORD_FOUND;
        }

        if (amount < helper.Min)
        {
            return KeywordHelperMatchStates.OUT_OF_BOUNDS;
        }
        if (amount > helper.Max)
        {
            return KeywordHelperMatchStates.OUT_OF_BOUNDS;
        }

        return KeywordHelperMatchStates.IN_BOUNDS;
    }

    protected KeywordHelperMatchStates GetKeywordHelperStateMin(string name, int amount)
    {
        return GetKeywordHelperStateMin(GetHelperForName(name), amount);
    }

    protected KeywordHelperMatchStates GetKeywordHelperStateMin(
        TuningKeyWordHelper helper,
        int amount
    )
    {
        if (helper == null)
        {
            return KeywordHelperMatchStates.NO_KEYWORD_FOUND;
        }

        if (amount != helper.Min)
        {
            return KeywordHelperMatchStates.OUT_OF_BOUNDS;
        }

        return KeywordHelperMatchStates.IN_BOUNDS;
    }

    protected KeywordHelperMatchStates GetKeywordHelperStateMax(string name, int amount)
    {
        return GetKeywordHelperStateMax(GetHelperForName(name), amount);
    }

    protected KeywordHelperMatchStates GetKeywordHelperStateMax(
        TuningKeyWordHelper helper,
        int amount
    )
    {
        if (helper == null)
        {
            return KeywordHelperMatchStates.NO_KEYWORD_FOUND;
        }

        if (amount != helper.Max)
        {
            return KeywordHelperMatchStates.OUT_OF_BOUNDS;
        }

        return KeywordHelperMatchStates.IN_BOUNDS;
    }

    protected TuningKeyWordHelper GetHelperForName(string name)
    {
        for (int i = 0; i < _helpers.Count; i++)
        {
            if (_helpers[i].Keywords.Count == 0)
            {
                continue;
            }
            if (!_helpers[i].Enabled)
            {
                continue;
            }
            bool matchFound = true;

            for (int n = 0; n < _helpers[i].Keywords.Count; n++)
            {
                if (name.Contains(_helpers[i].Keywords[n]))
                {
                    continue;
                }

                matchFound = false;
                break;
            }

            if (matchFound)
            {
                return _helpers[i];
            }
        }

        return null;
    }

    protected enum KeywordHelperMatchStates
    {
        NO_KEYWORD_FOUND,
        OUT_OF_BOUNDS,
        IN_BOUNDS,
    }

    [System.Serializable]
    public class TuningKeyWordHelper
    {
        public TuningKeyWordHelper() { }

        public TuningKeyWordHelper(TuningKeyWordHelper other)
        {
            this.Clr = other.Clr;
            this.Keywords.AddRange(other.Keywords);
            this.Enabled = other.Enabled;
            this.Min = other.Min;
            this.Max = other.Max;
        }

        public bool Enabled = true;
        public Color Clr = Color.white;
        public List<string> Keywords = new List<string>();
        public int Min = 0;
        public int Max = 5;
    }

    protected const int DEFAULT_BTN_SIZE = 32;
    protected const int ADD_BTN_SIZE = 32;
    protected const int REMOVE_BTN_SIZE = 32;
    protected const int TOGGLE_BTN_SIZE = 32;

    protected static string[] _categories = new string[]
    {
        "B_jobs",
        "C_blocks",
        "D_doors",
        "E_props",
        "F_containers",
        "G_vert_access",
        "H_lights",
        "I_floors",
        "J_roofs",
        "K_plants",
        "L_trees",
        "M_summons",
        "N_weeds",
        "O_meals",
        "P_liquids",
        "Q_tools",
        "R_materials",
        "S_gear",
        "T_rooms",
    };

    protected static string[] _resources = new string[]
    {
        "alcohol",
        "bedrock",
        "blood_sand",
        "bone",
        "bronze",
        "butcher",
        "clay",
        "cloth",
        "coal",
        "dirt",
        "egg",
        "ember_rock",
        "fire",
        "fish",
        "fruit",
        "fungus",
        "glass",
        "ice",
        "iron",
        "kalite",
        "leather",
        "light_leather",
        "linen",
        "meat",
        "medium_leather",
        "natural",
        "paper",
        "plant_fiber",
        "ridgestone",
        "rope",
        "rutile",
        "sable",
        "sand",
        "sandstone",
        "seed",
        "snow",
        "steel",
        "stone",
        "stoneleaf",
        "thatch",
        "veg",
        "void",
        "water",
        "wood",
        "wool",
    };

    protected static string[] _groups = new string[]
    {
        "aeternum",
        "ale",
        "altar",
        "anvil",
        "atlatl",
        "axe",
        "backpack",
        "bag",
        "barrel",
        "basket",
        "bed",
        "bfs",
        "bin",
        "block",
        "body",
        "bolas",
        "bookcase",
        "boots",
        "bow",
        "bowl",
        "brazier",
        "bread",
        "brew",
        "brick",
        "buckler",
        "burger",
        "cabinet",
        "cage",
        "carving_knife",
        "catalyst",
        "cauldron",
        "cell",
        "chest",
        "chisel",
        "chunk",
        "cider",
        "clod",
        "cloth",
        "club",
        "coat",
        "coffin",
        "crate",
        "crossbow",
        "crystal",
        "cudgel",
        "dagger",
        "dais",
        "dart",
        "displacer",
        "door",
        "drink",
        "dummy",
        "dust",
        "fence",
        "fire_pit",
        "fishing_pole",
        "flail",
        "floor",
        "flour",
        "fodder",
        "forge",
        "fungus",
        "furnace",
        "gate",
        "gladius",
        "grave",
        "hallberd",
        "hammer",
        "hat",
        "hatch",
        "hatchet",
        "hedge",
        "helmet",
        "icebox",
        "ingot",
        "ink",
        "khopesh",
        "kukri",
        "ladder",
        "lantern",
        "leather",
        "lectern",
        "leggings",
        "loom",
        "mace",
        "mat",
        "mine",
        "oven",
        "pane",
        "pants",
        "paper",
        "pickaxe",
        "pie",
        "plank",
        "plant",
        "plate",
        "pot",
        "rack",
        "remains",
        "roast",
        "roof",
        "rutile_body",
        "rutile_boots",
        "sagaris",
        "sarcophagus",
        "sausage",
        "seat",
        "sewing_needle",
        "shard",
        "shield",
        "sickle",
        "sling",
        "spear",
        "spellbook",
        "spikes",
        "spinning_wheel",
        "spoon",
        "stabilizer",
        "staff",
        "stairs",
        "stew",
        "stick",
        "still",
        "stool",
        "stove",
        "summon",
        "sword",
        "table",
        "throne",
        "tomb",
        "tome",
        "tongs",
        "torch",
        "tree",
        "trough",
        "waffles",
        "water",
        "weed",
        "well",
        "wine",
        "yeast",
    };

    protected static string[] _uniqueKeys = new string[]
    {
        "adept",
        "agave",
        "agriculture",
        "ancient",
        "apple",
        "arcana",
        "architect",
        "ardyn",
        "armor",
        "bastard",
        "beam",
        "beetroot",
        "berara",
        "blackberry",
        "blood_crown",
        "blood_crown_mushroom",
        "boiled",
        "boletus",
        "boletus_mushroom",
        "brick",
        "broccoli",
        "bronze_cap",
        "bronze_cap_mushroom",
        "buckhorn",
        "burclover",
        "cabbage",
        "caelum_oscula",
        "carpenters",
        "carrot",
        "carved",
        "cat",
        "cat_grass",
        "cauliflower",
        "celery",
        "chaga_fruit",
        "cherry_corn",
        "chi_chi",
        "chicken",
        "chickweed",
        "chirpen",
        "cholla",
        "chongo",
        "cleaving",
        "cobble",
        "coconut",
        "common",
        "cooking",
        "corn",
        "cow",
        "crab",
        "cresis",
        "dax_web",
        "death",
        "deep_moss",
        "deer",
        "destruction",
        "dire",
        "dirt_clod",
        "dog",
        "dolma",
        "dominators",
        "dragon_fern",
        "elderberry",
        "faceless",
        "fall",
        "felix",
        "fledgling",
        "focus",
        "food",
        "force",
        "gharox",
        "glow_light",
        "gnarl_tooth",
        "goose_grass",
        "gooseberry",
        "green_thumb",
        "gully",
        "gwdir",
        "healing",
        "heater",
        "hook_thorn",
        "human",
        "ice_root",
        "imp",
        "industry",
        "invokers",
        "iro",
        "kelp_witch",
        "kite",
        "knowledge",
        "kotha",
        "lady_harp",
        "lantern",
        "large",
        "large_crabgrass",
        "lirium",
        "log",
        "long",
        "lopen",
        "makeshift",
        "medium",
        "melon",
        "mirmit",
        "mirtle_grass",
        "mud_cat",
        "nequhtli",
        "night_ghost",
        "night_sleep",
        "nightwing",
        "nila",
        "oak",
        "omelette",
        "onion",
        "ornate",
        "owl",
        "owl_meat",
        "pala",
        "palm",
        "pineapple",
        "plank",
        "plant_fiber",
        "pointy",
        "potato",
        "practice",
        "prickly_bear",
        "prim_grass",
        "primarchs",
        "pumpkin",
        "purified",
        "rabbit",
        "rat",
        "rhodo",
        "rice",
        "round",
        "sacaton_grass",
        "sake",
        "sand_shrimp",
        "sandbur",
        "sendalo",
        "shadow_fern",
        "sheep",
        "sikari",
        "skyberry",
        "sleeping",
        "sleipnir",
        "small",
        "snoot",
        "spring",
        "squeaken",
        "staghorn",
        "stoneleaf",
        "stradken",
        "summer",
        "sweet_potato",
        "sweet_root",
        "tallow_grass",
        "tamtallen",
        "tanning",
        "temba",
        "tomato",
        "tomek",
        "tomek_fries",
        "traelik",
        "training",
        "trap",
        "tree_apple_sapling",
        "tree_berara_sapling",
        "tree_caelum_oscula_sapling",
        "tree_iro_sapling",
        "tree_nila_sapling",
        "tree_oak_sapling",
        "tree_pala_sapling",
        "tree_palm_sapling",
        "twilight",
        "viola",
        "vodka",
        "vytrax",
        "war",
        "water",
        "weapon",
        "weep_wisp",
        "well",
        "wheat",
        "whiskey",
        "winter",
        "woken",
        "xan",
        "yellow_gale",
        "zucchini",
    };

    private static AudioClip[] _loadedClips;

    public static AudioClip[] LoadAllClips()
    {
        if (_loadedClips != null && _loadedClips.Length > 0)
        {
            return _loadedClips;
        }
        string path = $"/Resources/Audio/";

        //_loadedClips = AssetDatabase.LoadAllAssetsAtPath(path);

        //return _loadedClips;

        string dir = Application.dataPath + path;

        if (!Directory.Exists(dir))
        {
            return null;
        }

        string[] files = Directory.GetFiles(dir);
        List<AudioClip> clips = new List<AudioClip>();

        for (int i = 0; files != null && i < files.Length; i++)
        {
            string filePath = "Assets" + files[i].Replace(Application.dataPath, "");
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<AudioClip>(filePath);

            if (asset == null)
            {
                continue;
            }

            AudioClip clip = asset as AudioClip;

            if (clip == null)
            {
                continue;
            }
            clips.Add(clip);
        }

        _loadedClips = clips.ToArray();

        return _loadedClips;
    }

    public static AudioClip LoadClip(string clipID)
    {
        string path = $"Assets/Resources/Audio/{clipID}.wav";

        return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    }

    public static bool ClipExists(string clipID)
    {
        return LoadClip(clipID) != null;
    }

    public static void PlayClip(string clipID)
    {
        PlayClip(LoadClip(clipID));
    }

    public static void PlayClip(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;

        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");

        MethodInfo[] m = audioUtilClass.GetMethods();

        MethodInfo method = audioUtilClass.GetMethod(
            "PlayPreviewClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] { typeof(AudioClip), typeof(Int32), typeof(Boolean) },
            null
        );

        Debug.Log(method);
        method.Invoke(null, new object[] { clip, 0, false });

        //AudioUtil.StopAllPreviewClips();
        //AudioUtil.PlayPreviewClip(clip, startSample, loop);
    }
}
