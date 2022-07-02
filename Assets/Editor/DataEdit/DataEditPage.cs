
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class DataEditPage
{
    protected bool _dataNeedsReload;
    protected string _searchInput = "";
    protected string _searchReplace = "";
    protected string _listFilter = "";
    protected Vector2 _scroll = new Vector2();
    protected List<AsyncOperationHandle<IList<Object>>> _activeLoads = new List<AsyncOperationHandle<IList<Object>>>();
    protected List<Object> _searchedObjects = new List<Object>();
    protected List<GDEInterfaceInfoData> _interfaces = new List<GDEInterfaceInfoData>();
    protected List<GDEItemsData> _items = new List<GDEItemsData>();
    protected List<GDEBlockModelData> _models = new List<GDEBlockModelData>();
    protected List<GDEBlocksData> _blocks = new List<GDEBlocksData>();
    protected List<GDEBlueprintsData> _blueprints = new List<GDEBlueprintsData>();
    protected static readonly TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;


    public abstract string PageName { get; }

    public virtual void OnEnable()
    {
        LoadAllData();
        ReOrderAll();
    }

    public virtual void Update()
    {
        if (_dataNeedsReload)
        {
            _dataNeedsReload = false;
            LoadAllData();
            ReOrderAll();
        }
    }

    public virtual void OnSelectionChange()
    {

    }

    private List<string> ids = new List<string>();
    private int _unusedTransitionKey = 1;

    public virtual void RenderGUI()
    {
        if (Selection.objects.Length > 0)
        {
            if (GUILayout.Button("FOO"))
            {
                //string[] dirs = Directory.GetDirectories(Application.dataPath + "/Resources_moved/Data/", "*", SearchOption.AllDirectories);

                //if(dirs != null)
                //{
                //    for (int i = 0; i < dirs.Length; i++)
                //    {
                //        string[] files = Directory.GetFiles(dirs[i]);

                //        if (files == null) { continue; }

                //        EditorUtility.DisplayProgressBar("Keying Data", dirs[i] + "...", (float)i / (float)dirs.Length);

                //        for (int n = 0; n < files.Length; n++)
                //        {
                //            if (files[n].Contains(".meta")) { continue; }

                //            string f = files[n].Replace(Application.dataPath, "Assets");
                //            Object o = AssetDatabase.LoadAssetAtPath(f, typeof(Object));

                //            if (o == null) { continue; }

                //            FieldInfo fi = o.GetType().GetField("Key");

                //            if (fi == null || fi.IsPrivate) { Debug.LogError("No key found for: " + f); continue; }

                //            fi.SetValue(o, o.name);
                //            EditorUtility.SetDirty(o);
                //        }
                //    }

                //    EditorUtility.ClearProgressBar();
                //}

                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    GDETooltipsData app = Selection.objects[i] as GDETooltipsData;

                    if (app == null) { continue; }

                    if (string.IsNullOrEmpty(app.InlineIcon))
                    {
                        app.InlineAndName = app.Name;
                    }
                    else if (string.IsNullOrEmpty(app.Name))
                    {
                        app.InlineAndName = app.InlineIcon;
                    }
                    else
                    {
                        app.InlineAndName = app.InlineIcon + " " + app.Name;
                    }

                    EditorUtility.SetDirty(app);
                }
            }
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Store Unused Transition Key ("+_unusedTransitionKey+")"))
        {
            _unusedTransitionKey = 1;


            for (int i = 0; i < Selection.objects.Length; i++)
            {
                GDEBlockVisualsData app = Selection.objects[i] as GDEBlockVisualsData;

                if (app == null) { continue; }
                if (app.TransitionKey < _unusedTransitionKey) { continue; }

                _unusedTransitionKey = app.TransitionKey + 1;
            }

            for (int i = 0; i < Selection.objects.Length; i++)
            {
                GDEBlockVisualsData app = Selection.objects[i] as GDEBlockVisualsData;

                if (app == null) { continue; }
                if (app.TransitionKey == _unusedTransitionKey) 
                {
                    _unusedTransitionKey = 0;
                }
            }
        }

        if (GUILayout.Button("Store IDs"))
        {
            ids.Clear();

            for (int i = 0; i < Selection.objects.Length; i++)
            {
                ids.Add(Selection.objects[i].name);
            }
        }

        if (GUILayout.Button("Clear IDs"))
        {
            ids.Clear();
        }
        GUILayout.EndHorizontal();

        for (int i = 0; i < ids.Count; i++)
        {
            GUILayout.Label(ids[i]);
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("List Filter");
        _listFilter = GUILayout.TextField(_listFilter);
        GUILayout.EndHorizontal();
    }

    private static void RenameAsset(Object obj, string newName)
    {
        var path = AssetDatabase.GetAssetPath(obj);
        AssetDatabase.RenameAsset(path, newName);
    }

    protected void GUI_SelectButton(Object objToSelect)
    {
        if (GUILayout.Button("Select", GUILayout.Width(128)))
        {
            Selection.objects = new Object[] { objToSelect };
            GUI.FocusControl(null);
        }
    }

    protected Color GUI_GetSelectedColor(Object objToCompare)
    {
        return Selection.objects.Length > 0 && Selection.objects[0] == objToCompare ? Color.green : Color.white;
    }

    // TOOLS

    protected GDEInterfaceInfoData CreateNewInterface(string newName)
    {
        string interfaceName = "interface_info_" + newName;
        GDEInterfaceInfoData newInterfaceData = CreateScriptableObject<GDEInterfaceInfoData>(interfaceName);
        newInterfaceData.Category = "A";

        SetDataDirty();

        EditorUtility.SetDirty(newInterfaceData);

        return newInterfaceData;
    }

    protected GDEBlueprintsData CreateNewBlueprint(string newName, GDEBlueprintsData blueprintToClone, GDEBlockVisualsData prevVisuals, GDEBlockModelData prevModel = null, GDEBlocksData blockData = null)
    {
        if (blueprintToClone == null) { return null; }
        if (prevVisuals == null) { return null; }

        if (string.IsNullOrEmpty(newName)) { return null; }

        string friendlyName = GetFriendlyName(newName);
        string blueprintName = "blueprint_" + newName;

        if (NameExistsInList<GDEBlocksData>(blueprintName, _blocks))
        {
            return null;
        }

        GDEBlueprintsData newBlueprintData = CreateScriptableObject<GDEBlueprintsData>(blueprintName);

        if (newBlueprintData == null) { return null; }

        string tooltipName = "tooltip_" + blueprintName;
        string requirementsName = "requirement_" + newName;
        string interfaceInfoName = "interface_info_" + newName;

        GDEBlueprintRequirementsData newRequirementsData = CreateScriptableObject<GDEBlueprintRequirementsData>(requirementsName);
        GDEInterfaceInfoData newInterfaceData = CreateScriptableObject<GDEInterfaceInfoData>(interfaceInfoName);

        // blueprint
        newBlueprintData.Clone(blueprintToClone);
        newBlueprintData.Requirements = newRequirementsData.name;
        newBlueprintData.TooltipID = tooltipName;
        newBlueprintData.InterfaceInfo = interfaceInfoName;
        newBlueprintData.Visuals.Clear();
        newBlueprintData.Visuals.Add(prevVisuals.name);
        
        if (blockData != null)
        {
            newBlueprintData.TooltipID = blockData.TooltipID;
        }

        if (prevModel != null)
        {
            newBlueprintData.ModelIndex = prevModel.ModelIndex;
        }

        // requirements
        newRequirementsData.Clone(LoadScriptableObject<GDEBlueprintRequirementsData>(blueprintToClone.Requirements));
        
        SetDataDirty();

        EditorUtility.SetDirty(newInterfaceData);
        EditorUtility.SetDirty(newBlueprintData);
        EditorUtility.SetDirty(newRequirementsData);

        return newBlueprintData;
    }

    protected GDEBlockModelData CreateNewModel(string newName, 
        GDEBlocksData block = null, 
        List<GDEItemsData> items = null, 
        List<GDEBlockPlantsData> plants = null, 
        GDEBlockPlatformsData platform = null)
    {
        if (string.IsNullOrEmpty(newName)) { return null; }
        string modelName = "model_" + newName;
        GDEBlockModelData newModelData = CreateScriptableObject<GDEBlockModelData>(modelName);

        int modelIndex = GetNextModelIndex();
        newModelData.ModelIndex = modelIndex;

        if (block != null)
        {
            newModelData.BlockIndex = block.Index;
        }

        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                newModelData.Items.Add(items[i].Key);
            }
        }

        if (plants != null)
        {
            for (int i = 0; i < plants.Count; i++)
            {
                newModelData.Plants.Add(plants[i].Key);
            }
        }

        if (platform != null)
        {
            newModelData.Platform = platform.Key;
        }

        EditorUtility.SetDirty(newModelData);

        return newModelData;
    }

    protected GDEBlocksData CreateNewBlock(string newName, GDEBlocksData blockToClone)
    {
        if (blockToClone == null) { return null; }

        if (string.IsNullOrEmpty(newName)) { return null; }

        string friendlyName = GetFriendlyName(newName);
        string blockName = "block_" + newName;

        if (NameExistsInList<GDEBlocksData>(blockName, _blocks))
        {
            return null;
        }

        GDEBlocksData newBlockData = CreateScriptableObject<GDEBlocksData>(blockName);

        if (newBlockData == null) { return null; }

        string visualsName = "visuals_" + blockName;
        string modelName = "model_" + blockName;
        string tooltipName = "tooltip_" + blockName;
        string spawnGroupName = "isg_" + blockName;

        GDEBlockVisualsData newVisualsData = CreateScriptableObject<GDEBlockVisualsData>(visualsName);
        GDEBlockModelData newModelData = CreateScriptableObject<GDEBlockModelData>(modelName);
        GDETooltipsData newTooltipData = CreateScriptableObject<GDETooltipsData>(tooltipName);
        GDEItemSpawnGroupsData newSpawnGroupData = CreateScriptableObject<GDEItemSpawnGroupsData>(spawnGroupName);

        int nextBlockIndex = GetNextBlockIndex();
        int nextModelIndex = GetNextModelIndex();

        // block
        newBlockData.Clone(blockToClone);
        newBlockData.Visuals.Clear();
        newBlockData.Visuals.Add(visualsName);
        newBlockData.TooltipID = tooltipName;
        newBlockData.Index = nextBlockIndex;
        newBlockData.RemoveItemDrops = spawnGroupName;

        // visuals
        GDEBlockVisualsData otherVisuals = null;
        if (blockToClone != null && blockToClone.Visuals.Count > 0) otherVisuals = LoadScriptableObject<GDEBlockVisualsData>(blockToClone.Visuals[0]);
        if (otherVisuals != null) newVisualsData.Clone(otherVisuals);

        // model
        newModelData.ModelIndex = nextModelIndex;
        newModelData.BlockIndex = nextBlockIndex;

        // tooltip
        newTooltipData.Name = friendlyName;
        newTooltipData.Icon = "sp_" + newName + "_icon";
        newTooltipData.InlineIcon = "<sprite=0>";
        _blocks.Add(newBlockData);
        
        SetDataDirty();

        EditorUtility.SetDirty(newBlockData);
        EditorUtility.SetDirty(newVisualsData);
        EditorUtility.SetDirty(newModelData);
        EditorUtility.SetDirty(newTooltipData);
        EditorUtility.SetDirty(newSpawnGroupData);

        return newBlockData;
    }

    protected GDEItemsData CreateNewItem(string newName, GDEItemsData itemToClone)
    {
        if (itemToClone == null) { return null; }

        if (string.IsNullOrEmpty(newName)) { return null; }

        string friendlyName = GetFriendlyName(newName);
        string itemName = "item_" + newName;

        if (NameExistsInList<GDEItemsData>(itemName, _items))
        {
            return null;
        }

        GDEItemsData newItemData = CreateScriptableObject<GDEItemsData>(itemName);

        if (newItemData == null) { return null; }

        string visualsName = "visuals_" + itemName;
        string modelName = "model_" + itemName;
        string tooltipName = "tooltip_" + itemName;

        GDEBlockVisualsData newVisualsData = CreateScriptableObject<GDEBlockVisualsData>(visualsName);
        GDEBlockModelData newModelData = CreateScriptableObject<GDEBlockModelData>(modelName);
        GDETooltipsData newTooltipData = CreateScriptableObject<GDETooltipsData>(tooltipName);


        int nextItemIndex = GetNextItemIndex();
        int nextModelIndex = GetNextModelIndex();

        // item
        newItemData.Clone(itemToClone);
        newItemData.Index = nextItemIndex;
        newItemData.TooltipID = tooltipName;
        newItemData.Visuals = visualsName;

        // visuals
        GDEBlockVisualsData otherVisuals = null;
        if (itemToClone != null) otherVisuals = LoadScriptableObject<GDEBlockVisualsData>(itemToClone.Visuals);
        newVisualsData.Clone(otherVisuals);

        // model
        newModelData.ModelIndex = nextModelIndex;
        newModelData.Items.Add(newItemData.name);

        // tooltip
        newTooltipData.Name = friendlyName;
        newTooltipData.InlineIcon = "<sprite=0>";
        newTooltipData.Icon = "sp_" + itemName + "_icon";
        SetDataDirty();

        EditorUtility.SetDirty(newItemData);
        EditorUtility.SetDirty(newVisualsData);
        EditorUtility.SetDirty(newModelData);
        EditorUtility.SetDirty(newTooltipData);

        return newItemData;
    }

    protected static bool SearchFilter(Object o, string searchInput)
    {
        if (o == null) { return false; }
        if (o.name.Contains(searchInput)) { return true; }

        FieldInfo[] fields = o.GetType().GetFields();

        for (int j = 0; j < fields.Length; j++)
        {
            FieldInfo f = fields[j];
            object fVal = f.GetValue(o);

            if (SearchFieldFilter(fVal, searchInput)) { return true; }
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

    protected bool NameExistsInList<T>(string name, List<T> l) where T : ScriptableObject
    {
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].name == name) { return true; }
        }

        return false;
    }

    // Data loading

    protected void SetDataDirty()
    {
        _dataNeedsReload = true;
    }

    private void ImportData<T>(List<T> l, string name) where T : ScriptableObject
    {
        l.Clear();
        Debug.Log("Loading " + name + "...");

        string[] files = Directory.GetFiles(Application.dataPath + "/Resources_moved/Data/" + name);

        for (int i = 0; files != null && i < files.Length; i++)
        {
            string path = "Assets" + files[i].Replace(Application.dataPath, "");
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset == null) { continue; }

            l.Add(asset);
        }

        Debug.Log(name + " loaded.");
    }

    protected bool _loadingAllData;

    protected void LoadAllData()
    {
        _loadingAllData = true;

        // Blocks.
        ImportData<GDEBlocksData>(_blocks, "blocks");

        // Blueprints.
        ImportData<GDEBlueprintsData>(_blueprints, "blueprints");

        // Block models.
        ImportData<GDEBlockModelData>(_models, "blockmodel");

        // Interface Info.
        ImportData<GDEInterfaceInfoData>(_interfaces, "interfaceinfo");

        // Items.
        ImportData<GDEItemsData>(_items, "items");

        _loadingAllData = false;
    }

    public static IEnumerator LoadAddressableData<T>(List<T> l, string group, System.Action onDone) where T : ScriptableObject
    {
        AsyncOperationHandle<IList<T>> dataHandler = Addressables.LoadAssetsAsync<T>(group, (T data) => {

            if (data == null) { return; }

            string key = data.name;

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Invalid dictionary key: " + key);
                return;
            }

            l.Add(data);
        });

        while (!dataHandler.IsDone) { yield return null; }

        onDone?.Invoke();
    }

    // Sorting

    protected void ReOrderAll()
    {
        ReOrderBlocks();
        ReOrderBlueprints();
        ReOrderModels();
        ReOrderInterfaces();
        ReOrderItems();
    }

    protected void ReOrderBlocks()
    {
        _blocks = _blocks.OrderBy((GDEBlocksData d) => { return d.Index; }).ToList();
    }

    protected void ReOrderBlueprints()
    {
        _blueprints = _blueprints.OrderBy((GDEBlueprintsData d) => { return d.name; }).ToList();
    }

    protected void ReOrderModels()
    {
        _models = _models.OrderBy((GDEBlockModelData d) => { return d.ModelIndex; }).ToList();
    }

    protected void ReOrderItems()
    {
        _items = _items.OrderBy((GDEItemsData d) => { return d.Index; }).ToList();
    }

    protected void ReOrderInterfaces()
    {
        _interfaces = _interfaces.OrderBy((GDEInterfaceInfoData interfaceData) => {
            return interfaceData.OrderKey;
        }).ToList();
    }

    protected void ReOrderAndSetInterfaceIndices()
    {
        ReOrderInterfaces();

        for (int i = 0; i < _interfaces.Count; i++)
        {
            if (_interfaces[i].Index == i) { continue; }
            _interfaces[i].Index = i;
            EditorUtility.SetDirty(_interfaces[i]);
        }
    }

    // Indexing

    private HashSet<int> _checkedIndices = new HashSet<int>();

    protected int GetNextModelIndex()
    {
        int highest = -1;
        _checkedIndices.Clear();
        _checkedIndices.Add(0);

        for (int i = 0; i < _models.Count; i++)
        {
            _checkedIndices.Add(_models[i].ModelIndex);

            if (_models[i].ModelIndex <= highest) { continue; }

            highest = _models[i].ModelIndex;
        }

        for (int i = 0; i < highest+1; i++)
        {
            if (_checkedIndices.Contains(i)) { continue; }

            return i;
        }

        return highest + 1;
    }

    protected int GetNextBlockIndex()
    {
        int highest = -1;
        _checkedIndices.Clear();
        _checkedIndices.Add(0);

        for (int i = 0; i < _blocks.Count; i++)
        {
            _checkedIndices.Add(_blocks[i].Index);

            if (_blocks[i].Index <= highest) { continue; }

            highest = _blocks[i].Index;
        }

        for (int i = 0; i < highest + 1; i++)
        {
            if (_checkedIndices.Contains(i)) { continue; }

            return i;
        }

        return highest + 1;
    }

    protected int GetNextItemIndex()
    {
        int highest = -1;
        _checkedIndices.Clear();
        _checkedIndices.Add(0);

        for (int i = 0; i < _items.Count; i++)
        {
            _checkedIndices.Add(_items[i].Index);

            if (_items[i].Index <= highest) { continue; }

            highest = _items[i].Index;
        }

        for (int i = 0; i < highest + 1; i++)
        {
            if (_checkedIndices.Contains(i)) { continue; }

            return i;
        }

        return highest + 1;
    }

    protected static void ReplaceStringInField(Object o, string searchInput, string replaceInput)
    {
        if (string.IsNullOrEmpty(searchInput) || string.IsNullOrEmpty(replaceInput)) { return; }

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

    protected void MoveInterfaceDown(GDEInterfaceInfoData interfaceData)
    {
        int v = -1;

        System.Int32.TryParse(interfaceData.SubCategory, out v);

        if (v == -1)
        {
            interfaceData.SubCategory = "99";
        }
        else
        {
            v++;
        }

        v = Mathf.Clamp(v, 1, 99);

        interfaceData.SubCategory = v < 10 ? "0" + v.ToString() : v.ToString();
    }

    protected void MoveInterfaceUp(GDEInterfaceInfoData interfaceData)
    {
        int v = -1;

        System.Int32.TryParse(interfaceData.SubCategory, out v);

        if (v == -1)
        {
            interfaceData.SubCategory = "99";
        }
        else
        {
            v--;
        }

        v = Mathf.Clamp(v, 1, 99);

        interfaceData.SubCategory = v < 10 ? "0" + v.ToString() : v.ToString();
    }

    protected static List<Object> LoadObjects(List<string> filePaths, string searchInput, System.Func<string, Object> load, System.Func<Object, string, bool> objectFilter)
    {
        List<Object> objs = new List<Object>();

        for (int i = 0; i < filePaths.Count; i++)
        {
            Object o = load(filePaths[i]);

            if (o == null) { continue; }
            if (!objectFilter(o, searchInput)) { continue; }

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

            if (dir == null) { continue; }

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

            if (dir == null) { continue; }
            if (dir.Name == "Animations") { continue; }

            FileInfo[] files = dir.GetFiles();

            for (int n = 0; files != null && n < files.Length; n++)
            {
                FileInfo file = files[n];

                if (file.Extension != ".asset") { continue; }

                string path = "Data/" + dir.Name + "/" + file.Name.Replace(".asset", "");
                filePaths.Add(path);
            }
        }

        return filePaths;
    }

    protected static T LoadScriptableObject<T>(string name) where T : ScriptableObject
    {
        T prev = Resources.Load<T>("Data/" + typeof(T).Name.Replace("GDE", "").Replace("Data", "") + "/" + name);

        return prev;
    }

    protected static T CreateScriptableObject<T>(string name) where T : ScriptableObject
    {
        T prev = LoadScriptableObject<T>(name);

        if (prev != null) { return prev; }

        T newSO = ScriptableObject.CreateInstance(typeof(T).Name) as T;

        if (newSO == null)
        {
            Debug.LogError("Could not create " + typeof(T).Name);
            return null;
        }

        AssetDatabase.CreateAsset(newSO, string.Format("Assets/Resources/Data/{0}/{1}.asset", typeof(T).Name.Replace("GDE", "").Replace("Data", ""), name));
        AssetDatabase.SaveAssets();

        return newSO;
    }

    protected static T GetScriptableObjectFromSelection<T>(object[] selections) where T : ScriptableObject
    {
        for (int i = 0; i < selections.Length; i++)
        {
            T s = selections[i] as T;

            if (s == null) { continue; }

            return s;
        }

        return null;
    }

    protected static List<T> GetScriptableObjectsFromSelection<T>(object[] selections) where T : ScriptableObject
    {
        List<T> l = null;

        for (int i = 0; i < selections.Length; i++)
        {
            T s = selections[i] as T;

            if (s == null) { continue; }

            if (l == null) l = new List<T>();
            l.Add(s);
        }

        return l;
    }

    protected string GetFriendlyName(string name)
    {
        return _textInfo.ToTitleCase(name.Replace("_", " "));
    }

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
        "T_rooms"
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
        "wool"
    };

    protected static string[] _groups = new string[] {
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
        "yeast"
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
        "zucchini"
    };
}
