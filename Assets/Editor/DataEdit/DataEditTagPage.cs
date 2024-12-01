using Assets.GameData;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using static GDEBiomesData;

public class DataEditTagPage : DataEditPage
{
    public struct UIState
    {
        public bool Expanded;
    }

    public override string PageName { get { return "Tags"; } }

#if ODD_REALM_APP
    public GameSession Session { get { return GameManager.Instance != null && GameManager.Instance.Session != null ? GameManager.Instance.Session : null; } }
    public WorldManager World { get { return Session != null ? Session.World : null; } }
    public OverworldTile ActiveTile { get { return World != null ? Session.World.ActiveTile : null; } }
#endif

    private Object[] _lockedSelections;
    private int _dataPageIndex;
    private int _dataPerPage = 30;
    private bool _hideScriptsWithNoChildren;
    private bool _hideScriptsWithNoTag;
    private bool _hideScriptsWithNoIssue;
    private bool _hideUnselected;
    private bool _invertTagFilterResults;
    private string _scriptKeyFilter = "";
    private string[] _scriptKeyFilterWords;
    private string _tagKeyFilter = "";
    private string[] _tagKeyFilterWords;
    private string _scriptTypeFilter = "";
    private List<Scriptable> _visibleScripts = new List<Scriptable>();
    //private string _selectedScript;
    //private List<Scriptable> _scriptCache = new List<Scriptable>();
    private bool _visibleDirty;
    private bool _editingTags;
    private bool _editingScriptTypes;
    private string _newTagName;
    private string _newTagObjSpawnName;
    private string _simToAddToSelections;
    private string _newSimDataID;
    private bool _chooseSimFromList;

    private HashSet<string> _visibleScriptTypes = new HashSet<string>()
    {

    };

    private const string SAVE_KEY_VISIBLE_SCRIPT_TYPES = "save_key_visible_script_types";
    private const string SAVE_KEY_DATA_PER_PAGE = "save_key_data_per_page";
    private const string SAVE_KEY_HIDE_SCRIPT_NO_CHILD = "save_key_hide_script_no_child";
    private const string SAVE_KEY_HIDE_SCRIPT_NO_TAG = "save_key_hide_script_no_tag";
    private const string SAVE_KEY_HIDE_SCRIPT_NO_ISSUE = "save_key_hide_script_no_issue";
    private const string SAVE_KEY_HIDE_SCRIPT_UNSELECTED = "save_key_hide_script_unselected";
    private const string SAVE_KEY_SCRIPT_KEY_FILTER = "save_key_script_key_filter";
    private const string SAVE_KEY_TAG_KEY_FILTER = "save_key_tag_key_filter";
    private const string SAVE_KEY_INVERT_TAG_FILTER_RESULTS = "save_key_invert_tag_filter_results";

    public override void OnEnable(DataEdit dataEdit)
    {
        base.OnEnable(dataEdit);
        _visibleScriptTypes = new HashSet<string>(LOAD_STRINGS(SAVE_KEY_VISIBLE_SCRIPT_TYPES));
        _dataPerPage = LOAD_INT(SAVE_KEY_DATA_PER_PAGE);
        _hideScriptsWithNoChildren = LOAD_INT(SAVE_KEY_HIDE_SCRIPT_NO_CHILD) == 1;
        _hideScriptsWithNoTag = LOAD_INT(SAVE_KEY_HIDE_SCRIPT_NO_TAG) == 1;
        _hideScriptsWithNoIssue = LOAD_INT(SAVE_KEY_HIDE_SCRIPT_NO_ISSUE) == 1;
        _hideUnselected = LOAD_INT(SAVE_KEY_HIDE_SCRIPT_UNSELECTED) == 1;
        _scriptKeyFilter = LOAD_STRING(SAVE_KEY_SCRIPT_KEY_FILTER);
        _tagKeyFilter = LOAD_STRING(SAVE_KEY_TAG_KEY_FILTER);
        _invertTagFilterResults = LOAD_INT(SAVE_KEY_INVERT_TAG_FILTER_RESULTS) == 1;
    }

    protected override void SAVE()
    {
        SAVE(SAVE_KEY_VISIBLE_SCRIPT_TYPES, new List<string>(_visibleScriptTypes));
        SAVE(SAVE_KEY_HIDE_SCRIPT_NO_ISSUE, _hideScriptsWithNoIssue ? 1 : 0);
        SAVE(SAVE_KEY_HIDE_SCRIPT_UNSELECTED, _hideUnselected ? 1 : 0);
        SAVE(SAVE_KEY_HIDE_SCRIPT_NO_TAG, _hideScriptsWithNoTag ? 1 : 0);
        SAVE(SAVE_KEY_HIDE_SCRIPT_NO_CHILD, _hideScriptsWithNoChildren ? 1 : 0);
        SAVE(SAVE_KEY_DATA_PER_PAGE, _dataPerPage);
        SAVE(SAVE_KEY_SCRIPT_KEY_FILTER, _scriptKeyFilter);
        SAVE(SAVE_KEY_TAG_KEY_FILTER, _tagKeyFilter);
        SAVE(SAVE_KEY_INVERT_TAG_FILTER_RESULTS, _invertTagFilterResults ? 1 : 0);
        base.SAVE();
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private bool IsScriptVisible(Scriptable script)
    {
        if (IS_SELECTED(script) || script.Key == _scriptKeyFilter) { return true; }

        if (SelectionCount > 0 && _hideUnselected) { return false; }

        if (_hideScriptsWithNoChildren && GetScriptsByTagCount(script) == 0) { return false; }
        if (_hideScriptsWithNoTag && script.TagCount == 0) { return false; }
        if (_hideScriptsWithNoIssue && !HasIssue(script)) { return false; }

        if (_visibleScriptTypes.Count > 0 && !_visibleScriptTypes.Contains(script.ObjectType)) { return false; }

        if (!HasFilter(script.Key, _scriptKeyFilterWords)) { return false; }

        if (!string.IsNullOrEmpty(_tagKeyFilter))
        {
            bool hasTag = HasTag(script, _tagKeyFilterWords);

            if (_invertTagFilterResults && hasTag) { return false; }
            if (!_invertTagFilterResults && !hasTag) { return false; }
        }

        return FOO_FILTER(script);
    }

    private bool HasFilter(string name, string[] filterWords)
    {
        if (filterWords != null && filterWords.Length > 0)
        {
            for (int i = 0; i < filterWords.Length; i++)
            {
                if (!name.Contains(filterWords[i])) { return false; }
            }
        }

        return true;
    }

    private bool HasTag(Scriptable script, string[] filterWords)
    {
        if (filterWords != null && filterWords.Length > 0)
        {
            for (int n = 0; n < script.TagCount; n++)
            {
                string tagID = script.GetTagID(n);

                for (int i = 0; i < filterWords.Length; i++)
                {
                    if (tagID == filterWords[i]) { return true; }
                }
            }
        }

        return false;
    }

    private bool HasIssue(Scriptable script)
    {
        if (string.IsNullOrEmpty(script.TooltipID))
        {
            return true;
        }

        if (script.TooltipIcon == "tooltip_missing")
        {
            return true;
        }

        if (script is GDEAttacksData attackData)
        {
            if (string.IsNullOrEmpty(attackData.TargetAttributeID) &&
                string.IsNullOrEmpty(attackData.SourceAttributeID))
            {
                return true;
            }

            if (string.IsNullOrEmpty(attackData.SkillID))
            {
                return true;
            }
        }

        return false;
    }

    public override void OnSelectionChange()
    {
        base.OnSelectionChange();
        MarkVisibleDirty();
    }

    protected override void AddTagToScript(Scriptable script, Scriptable tag)
    {
        base.AddTagToScript(script, tag);
        MarkVisibleDirty();
    }

    protected override void RemoveTagFromScript(Scriptable script, Scriptable tag)
    {
        base.RemoveTagFromScript(script, tag);
        MarkVisibleDirty();
    }

    protected override void MarkDataNeedsReload()
    {
        _dataNeedsReload = true;
    }

    protected override void MarkVisibleDirty()
    {
        _visibleDirty = true;
    }

    private void ClearScriptSearch()
    {
        _scriptKeyFilter = "";
        _scriptKeyFilterWords = null;
        MarkVisibleDirty();
    }

    protected override void LoadAllData()
    {
        base.LoadAllData();
        MarkVisibleDirty();
    }

    protected bool FOO_FILTER(Scriptable script)
    {
        //if (script is GDEItemsData itemData)
        //{
        //    if (itemData.RarityType != RarityTypes.LEGENDARY) { return false; }
        //}
        //else
        //{
        //    return false;
        //}

        return true;
    }

    protected override void FOO(string fooTxt, int fooInt)
    {
        for (int t = 0; t < _visibleScripts.Count; t++)
        {
            Scriptable script = _visibleScripts[t];


            if (script == null) { continue; }
            if (!FOO_FILTER(script)) { continue; }

            EditorUtility.DisplayProgressBar("FOO", "", t / (float)_visibleScripts.Count);

            MARK_DIRTY(script);

            if (script is GDEBlocksData data)
            {
                if (data.Key.Contains("_ladder") ||
                    data.Key.Contains("_stairs"))
                {
                    data.SetSimulationID("");
                    Debug.Log(data.Key);
                }
            }
        }

        EditorUtility.ClearProgressBar();
    }

    private void AddActionBuff(GDEItemsData itemData, string targetID, int amount)
    {
        bool hasAttribute = false;

        for (int n = 0; n < itemData.Actions.Length; n++)
        {
            if (itemData.Actions[n].Buff.TargetID == targetID)
            {
                hasAttribute = true;

                if (itemData.Actions[n].Buff.Amount != amount)
                {
                    int prevAmount = itemData.Actions[n].Buff.Amount;
                    itemData.Actions[n].Buff.Amount = amount;
                    Debug.LogError($"Updated action: {itemData.Key} from {prevAmount} to {amount}");
                }

                break;
            }
        }

        if (!hasAttribute)
        {
            List<ItemAction> actions = new List<ItemAction>(itemData.Actions);

            actions.Add(new ItemAction()
            {
                ActionID = "tag_can_equip",
                Buff = new BuffData()
                {
                    TargetID = targetID,
                    Amount = amount
                }
            });

            itemData.Actions = actions.ToArray();
            Debug.LogError($"Added action to: {itemData.Key}");
        }
    }

    public override void RenderGUI()
    {
        base.RenderGUI();

        _scriptsRendering = 0;

        if (_allData == null || _allData.Count == 0)
        {
            BTN("LOAD TAG DATA", LoadAllData);
        }

        BEGIN_HOR();

        string scriptKeyFilter = _scriptKeyFilter;
        LABEL("Search");
        _scriptKeyFilter = TEXT_INPUT(_scriptKeyFilter);

        if (scriptKeyFilter != _scriptKeyFilter || _scriptKeyFilterWords == null)
        {
            _scriptKeyFilterWords = _scriptKeyFilter.Split(' ');
            MARK_NEEDS_SAVE();
            MarkVisibleDirty();
        }

        //END_HOR();


        //BEGIN_HOR();

        string tagFilter = _tagKeyFilter;
        BEGIN_CLR(Color.cyan);
        LABEL("Tag");
        _tagKeyFilter = TEXT_INPUT(_tagKeyFilter);

        if (!string.IsNullOrEmpty(_tagKeyFilter) && !ScriptExists(_tagKeyFilter))
        {
            CREATE_SCRIPT_BTN<GDETagsData>(_tagKeyFilter);
            WARNING("Tag does not exist!");
        }

        END_CLR();

        if (tagFilter != _tagKeyFilter || _tagKeyFilterWords == null)
        {
            _tagKeyFilterWords = _tagKeyFilter.Split(' ');
            MARK_NEEDS_SAVE();
            MarkVisibleDirty();
        }

        END_HOR();

        // Script type filter.
        RenderScriptTypesFilter();


        // Tag list.
        RenderTagList();

        //if (!string.IsNullOrEmpty(_copiedText))
        {
            BEGIN_HOR();
            DELETE_BTN(CLEAR_COPY);
            BEGIN_CLR(ColorUtility.purple);
            _copiedText = TEXT_INPUT(_copiedText);
            END_CLR();
            END_HOR();
        }

        BEGIN_HOR();

        bool invertTagFilterResults = _invertTagFilterResults;
        _invertTagFilterResults = TOGGLE(_invertTagFilterResults, "Show Results Without Tag Filter");

        if (invertTagFilterResults != _invertTagFilterResults)
        {
            MARK_NEEDS_SAVE();
            MarkVisibleDirty();
        }

        bool hideScriptsWithNoChildren = _hideScriptsWithNoChildren;
        _hideScriptsWithNoChildren = TOGGLE(_hideScriptsWithNoChildren, "Hide Without Children");

        if (hideScriptsWithNoChildren != _hideScriptsWithNoChildren)
        {
            MARK_NEEDS_SAVE();
            MarkVisibleDirty();
        }

        bool hideScriptsWithNoTag = _hideScriptsWithNoTag;
        _hideScriptsWithNoTag = TOGGLE(_hideScriptsWithNoTag, "Hide Without Tag");

        if (hideScriptsWithNoTag != _hideScriptsWithNoTag)
        {
            MARK_NEEDS_SAVE();
            MarkVisibleDirty();
        }

        bool hideScriptsWithoutIssue = _hideScriptsWithNoIssue;
        _hideScriptsWithNoIssue = TOGGLE(_hideScriptsWithNoIssue, "Hide Without Issue");

        if (hideScriptsWithoutIssue != _hideScriptsWithNoIssue)
        {
            MARK_NEEDS_SAVE();
            MarkVisibleDirty();
        }

        bool hideUnselected = _hideUnselected;
        _hideUnselected = TOGGLE(_hideUnselected, "Hide Unselected");

        if (hideUnselected != _hideUnselected)
        {
            MARK_NEEDS_SAVE();
            MarkVisibleDirty();
        }
        END_HOR();

        BEGIN_HOR();
        LABEL("Scripts Per Page:");
        int prevDataPerPage = _dataPerPage;
        _dataPerPage = INT_INPUT(_dataPerPage);
        if (prevDataPerPage != _dataPerPage)
        {
            MARK_NEEDS_SAVE();
            MarkVisibleDirty();
        }
        END_HOR();

        if (_visibleDirty)
        {
            _visibleDirty = false;
            int prevSize = _visibleScripts.Count;
            _visibleScripts.Clear();

            for (int i = 0; i < _allData.Count; i++)
            {
                Scriptable script = _allData[i];

                if (script == null) { continue; }
                if (!IsScriptVisible(script)) { continue; }

                _visibleScripts.Add(script);
            }

            if (prevSize != _visibleScripts.Count)
            {
                _dataPageIndex = 0;
            }

            for (int i = 0; Selection.objects.Length > 0 && i < _visibleScripts.Count; i++)
            {
                if (_visibleScripts[i] == Selection.objects[0])
                {
                    _dataPageIndex = Mathf.FloorToInt((float)i / (float)_dataPerPage);
                    break;
                }
            }
        }

        _dataPerPage = Mathf.Max(1, _dataPerPage);
        int totalPages = Mathf.CeilToInt((float)_visibleScripts.Count / (float)_dataPerPage);

        if (totalPages > 0)
        {
            BEGIN_HOR(32);
            System.Action movePageLeft = () =>
            {
                _dataPageIndex = Mathf.Clamp(_dataPageIndex - 1, 0, totalPages - 1);
            };
            System.Action movePageRight = () =>
            {
                _dataPageIndex = Mathf.Clamp(_dataPageIndex + 1, 0, totalPages - 1);
            };
            BTN("<", movePageLeft, -1, true, true);
            READ_KEY_UP(KeyCode.PageUp, movePageLeft);
            LABEL(_dataPageIndex);
            LABEL("/");
            LABEL(totalPages - 1);
            LABEL("(");
            LABEL(_visibleScripts.Count);
            LABEL(")");
            BTN(">", movePageRight, -1, true, true);
            READ_KEY_UP(KeyCode.PageDown, movePageRight);
            END_HOR();

            System.Action moveUp = () =>
            {
                for (int i = 0; Selection.objects.Length > 0 && i < _visibleScripts.Count; i++)
                {
                    if (_visibleScripts[i] == Selection.objects[0])
                    {
                        SELECT(_visibleScripts[Mathf.Max(0, i - 1)]);
                        break;
                    }
                }
            };

            System.Action moveDown = () =>
            {
                for (int i = 0; Selection.objects.Length > 0 && i < _visibleScripts.Count; i++)
                {
                    if (_visibleScripts[i] == Selection.objects[0])
                    {
                        SELECT(_visibleScripts[Mathf.Min(_visibleScripts.Count - 1, i + 1)]);
                        break;
                    }
                }
            };

            READ_KEY_UP(KeyCode.UpArrow, moveUp);
            READ_KEY_UP(KeyCode.DownArrow, moveDown);

        }

        //_dataPerPage = Mathf.Min(_dataPerPage, _visibleScripts.Count);
        int dataStart = _dataPageIndex * _dataPerPage;
        int dataEnd = Mathf.Min(dataStart + _dataPerPage, _visibleScripts.Count);
        BEGIN_SCROLL("script_scroll");

        if (dataStart != 0)
        {
            LABEL("<< More", Color.yellow);
        }

        // Scripts.
        for (int i = dataStart; i >= 0 && i < dataEnd; i++)
        {
            if (i >= _visibleScripts.Count) { break; }
            Scriptable script = _visibleScripts[i];

            if (script == null) { continue; }

            BEGIN_HOR();

            int tagObjCount = GetScriptsByTagCount(script);
            bool isSelected = IS_SELECTED(script);

            LABEL(i, isSelected ? ColorUtility.voidPurple : Color.gray);

            //if (isSelected)
            {
                COPY_BTN(script.Key);
            }

            if (!string.IsNullOrEmpty(_copiedText) && !ScriptExists(_copiedText))
            {
                BTN($"D:", ColorUtility.purple, () =>
                {
                    CreateScriptableObject(_copiedText, script.ObjectType);
                    MarkDataNeedsReload();
                }, 32);
            }

            Color selectedColor = isSelected ? ColorUtility.voidPurple : Color.white;
            BTN(script.Key, selectedColor, () =>
            {
                if (isSelected) DESELECT(script);
                else SELECT(script);
            });

            LABEL("(");
            LABEL(script.ObjectType, selectedColor);
            LABEL(")");
            if (script.TagCount > 0)
            {
                LABEL(script.TagCount, Color.cyan);
                LABEL("Tags", Color.cyan);
            }
            if (tagObjCount > 0)
            {
                LABEL(tagObjCount, Color.yellow);
                LABEL("Children", Color.yellow);
            }

            ASSERT_WARNING_ICON(!HasIssue(script));
            FLEX_SPACE();
            END_HOR();

            // If selected.
            if (isSelected)
            {
                //BEGIN_HOR();
                //TAB(1);
                RenderScript(script);
                //END_HOR();
            }
        }

        if (dataEnd != _visibleScripts.Count)
        {
            LABEL("More >>", Color.yellow);
        }

        END_SCROLL();

        //FLEX_SPACE();
    }

    private void RenderTagList()
    {
        // Hide / Show Tags.
        BTN(_editingTags ? "Hide Tags" : "Show Tags", _editingTags ? Color.magenta : Color.gray, () =>
        {
            _editingTags = !_editingTags;
        });

        // Tags.
        if (_editingTags)
        {
            // Create new tag?
            BEGIN_HOR();

            BEGIN_DISABLED(string.IsNullOrEmpty(_newTagName) || ScriptExists(_newTagName));
            BTN($"Create Tag: {_newTagName}", Color.green, () =>
            {
                if (!_newTagName.Contains("tag_")) { return; }
                SELECT(CreateScriptableObject<GDETagsData>(_newTagName));
                MarkDataNeedsReload();
            });
            END_DISABLED();
            _newTagName = TEXT_INPUT(_newTagName);
            END_HOR();

            BEGIN_HOR();
            string tagKeyFilter = _tagKeyFilter;
            LABEL("Search");
            _tagKeyFilter = TEXT_INPUT(_tagKeyFilter);
            END_HOR();

            if (tagKeyFilter != _tagKeyFilter)
            {
                _tagKeyFilterWords = _tagKeyFilter.Split(' ');
                MarkVisibleDirty();
            }

            List<Scriptable> tags = GetDataByType<GDETagsData>();

            BTN("Clear Unused Tags", Color.red, () =>
            {
                List<Scriptable> unusedTags = new List<Scriptable>();

                for (int i = 0; i < tags.Count; i++)
                {
                    if (GetScriptsByTagCount(tags[i]) > 0 ||
                        tags[i].Key == "tag_missing")
                    {
                        continue;
                    }

                    unusedTags.Add(tags[i]);
                }

                for (int i = 0; i < unusedTags.Count; i++)
                {
                    string path = AssetDatabase.GetAssetPath(unusedTags[i]);
                    AssetDatabase.DeleteAsset(path);
                    Debug.LogError($"Deleted: {path}");
                }

                MarkDataNeedsReload();
            });

            BEGIN_SCROLL("tag_scroll");

            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i] == null) { continue; }

                int addCount = 0;

                bool isSelected = IS_SELECTED(tags[i]);

                for (int n = 0; n < Selection.objects.Length; n++)
                {
                    if (Selection.objects[n] is Scriptable scriptObj)
                    {
                        if (scriptObj.TagIDs.Contains(tags[i].Key)) { continue; }

                        addCount++;
                    }
                }

                if (!HasFilter(tags[i].Key, _tagKeyFilterWords)) { continue; }

                BEGIN_HOR();
                TAB(2);
                LABEL("Tag: ");
                BEGIN_DISABLED(addCount == 0);
                BTN($"Add Tag To: {addCount}", Color.green, () =>
                {
                    AddTagToScripts(Selection.objects, tags[i]);
                }, 128);
                END_DISABLED();
                COPY_BTN(tags[i].Key);
                BTN(tags[i].Key, isSelected ? ColorUtility.selectedGold : Color.white, () =>
                {
                    SELECT(tags[i]);
                    ClearScriptSearch();
                });
                LABEL(GetScriptsByTagCount(tags[i]), Color.yellow);
                LABEL("Children", Color.yellow);
                END_HOR();
            }

            END_SCROLL();
        }

    }

    private void RenderScriptTypesFilter()
    {
        BEGIN_HOR();
        if (_visibleScriptTypes.Count > 0)
        {
            BTN($"Clear Script Type Filters ({_visibleScriptTypes.Count})", Color.red, () =>
            {
                _visibleScriptTypes.Clear();
                MarkVisibleDirty();
            }, 160);
        }
        BTN(_editingScriptTypes ? "Hide Script Types" : "Show Script Types", _editingScriptTypes ? Color.magenta : Color.gray, () =>
        {
            _editingScriptTypes = !_editingScriptTypes;
        });
        END_HOR();

        // Script types
        if (_editingScriptTypes)
        {
            BEGIN_HOR();
            LABEL($"Visible Scripts: {(_visibleScriptTypes.Count == 0 ? _scriptTypes.Count : _visibleScriptTypes.Count)}");
            _scriptTypeFilter = TEXT_INPUT(_scriptTypeFilter);
            END_HOR();

            BEGIN_SCROLL("script_type_scroll");

            for (int i = 0; i < _scriptTypes.Count; i++)
            {
                if (!string.IsNullOrEmpty(_scriptTypeFilter) && !_scriptTypes[i].ToLower().Contains(_scriptTypeFilter)) { continue; }

                bool isVisible = _visibleScriptTypes.Contains(_scriptTypes[i]);
                Color btnColor = isVisible ? Color.green : Color.gray;

                BTN(_scriptTypes[i], btnColor, () =>
                {
                    if (isVisible)
                        _visibleScriptTypes.Remove(_scriptTypes[i]);
                    else
                        _visibleScriptTypes.Add(_scriptTypes[i]);

                    MARK_NEEDS_SAVE();
                    MarkVisibleDirty();
                });
            }

            END_SCROLL();
        }
    }

    private int _scriptsRendering;

    private void RenderTooltip(Scriptable script)
    {
        ITooltipContent tooltip = script;
        GDETooltipsData tooltipData = script as GDETooltipsData;
        if (tooltipData != null) tooltip = tooltipData;

        BEGIN_HOR();
        BEGIN_SPRITE(tooltip.TooltipIconSprite);
        BEGIN_VERT();

        if (tooltipData == null)
        {
            script.TooltipID = DATA_ID_INPUT<GDETooltipsData>(script.TooltipID, "Tooltip ID", out tooltipData);
        }

        if (tooltipData != null && tooltipData.Key != "tooltip_missing")
        {
            RenderTooltip(tooltipData);
        }
        END_VERT();
        END_HOR();
    }

    private void RenderTooltip(GDETooltipsData tooltip)
    {
        MARK_DIRTY(tooltip);
        LABEL(tooltip.Key, tooltip.TooltipTextColor);
        LABEL(tooltip.TooltipName, tooltip.TooltipTextColor);
        tooltip.TextColor = COLOR_INPUT(tooltip.TooltipTextColor);

        BEGIN_HOR();
        tooltip.TooltipData.Icon = TEXT_INPUT(tooltip.TooltipData.Icon, "Icon");
        END_HOR();

        BEGIN_HOR();
        tooltip.TooltipData.InlineIcon = TEXT_INPUT(tooltip.TooltipData.InlineIcon, "Inline Icon");
        END_HOR();

        BEGIN_HOR();
        tooltip.TooltipData.Name = TEXT_INPUT(tooltip.TooltipData.Name, "Name");
        END_HOR();

        BEGIN_HOR();
        tooltip.TooltipData.Action = TEXT_INPUT(tooltip.TooltipData.Action, "Action");
        END_HOR();

        BEGIN_HOR();
        tooltip.TooltipData.Type = TEXT_INPUT(tooltip.TooltipData.Type, "Type");
        END_HOR();

        BEGIN_HOR();
        LABEL("Description");
        tooltip.TooltipData.Description = GUILayout.TextField(tooltip.TooltipData.Description, GUILayout.MaxWidth(WINDOW_WIDTH() * 0.75f));
        END_HOR();
    }

    private void RenderScript(Scriptable script)
    {
        if (script == null) { return; }
        if (_scriptsRendering > 1) { return; }

        MARK_DIRTY(script);
        MARK_DIRTY(script.TooltipData);

        _scriptsRendering++;

        LABEL(script.ObjectType, ColorUtility.voidPurple);
        BEGIN_INDENT();
        LABEL("Core Script Data:", Color.grey);
        RenderTooltip(script);
        // List/Edit tag IDs for script.
        LABEL("Tag IDs:", Color.yellow);
        BEGIN_INDENT();

        for (int n = 0; n < script.TagCount; n++)
        {
            BEGIN_HOR();
            script.TagIDs = LIST_REMOVE_BTN<string>(script.TagIDs, n);
            if (n < script.TagCount)
            {
                GDETagsData t = null;
                script.SetTagID(n, DATA_ID_INPUT<GDETagsData>(script.TagIDs[n], "Tag ID", out t));
            }
            END_HOR();
        }

        script.TagIDs = LIST_ADD_BTN<string>(script.TagIDs, "+Add Tag");
        END_INDENT();

        // List/Edit tag IDs for script.
        LABEL("Discovery Children:", Color.cyan);
        BEGIN_INDENT();

        for (int n = 0; n < script.DiscoveryDependencies.Count; n++)
        {
            BEGIN_HOR();
            script.DiscoveryDependencies = LIST_REMOVE_BTN<string>(script.DiscoveryDependencies, n);
            if (n < script.DiscoveryDependencies.Count)
            {
                script.DiscoveryDependencies[n] = DATA_ID_INPUT<Scriptable>(script.DiscoveryDependencies[n], "Tag Object ID", out var t);
            }
            END_HOR();
        }

        script.DiscoveryDependencies = LIST_ADD_BTN<string>(script.DiscoveryDependencies, "+Add Discovery Child");
        END_INDENT();
        END_INDENT();

        // Tag.
        if (script is GDETagsData tagData)
        {
            LABEL("Tag Data:", Color.grey);
            RenderTag(tagData);
        }
        // Tag Obj Spawn.
        else if (script is GDETagObjectSpawnData tagObjSpawnData)
        {
            LABEL("Tag Obj Spawn Data:", Color.grey);
            RenderTagObjectSpawn(tagObjSpawnData);
        }
        // Biome.
        else if (script is GDEBiomesData biomeData)
        {
            LABEL("Biome Data:", Color.grey);
            RenderBiome(biomeData);
        }
        // Cave.
        else if (script is GDECaveData caveData)
        {
            LABEL("Cave Data:", Color.grey);
            RenderCave(caveData);
        }
        // Scenario.
        else if (script is GDEScenariosData scenarioData)
        {
            LABEL("Scenario Data:", Color.grey);
            RenderScenario(scenarioData);
        }
        // Item.
        else if (script is GDEItemsData itemData)
        {
            RenderItem(script, itemData);
        }
        // Fish.
        else if (script is GDEFishData fishData)
        {
            RenderFish(script, fishData);
        }
        // Entity status.
        else if (script is GDEEntityStatusData entityStatusData)
        {
            RenderEntityStatusData(script, entityStatusData);
        }
        // Entity.
        else if (script is GDEEntitiesData entityData)
        {
            RenderEntityData(script, entityData);
        }
        // Leader.
        else if (script is GDELeaderData leaderData)
        {
            RenderLeaderData(script, leaderData);
        }
        // Blueprint.
        else if (script is GDEBlueprintsData blueprintData)
        {
            RenderBlueprintData(script, blueprintData);
        }
        // Room.
        else if (script is GDERoomTemplatesData roomData)
        {
            RenderRoomData(script, roomData);
        }
        // Occupant Group.
        else if (script is GDEOccupantGroupData occupantGroupData)
        {
            RenderOccupantGrouptData(script, occupantGroupData);
        }
        // Plant.
        else if (script is GDEBlockPlantsData plantData)
        {
            RenderPlantData(script, plantData);
        }
        // Block.
        else if (script is GDEBlocksData blockData)
        {
            RenderBlock(script, blockData);
        }
        // Visuals.
        else if (script is GDEBlockVisualsData visualsData)
        {
            RenderBlockVisualsData(script, visualsData);
        }
        // Sim.
        else if (script is GDESimulationData baseSimData)
        {
            RenderSimData(script, baseSimData);
        }
        // Attack Group.
        else if (script is GDEAttackGroupsData attackGroup)
        {
            RenderAttackGroup(attackGroup);
        }
        // Attack.
        else if (script is GDEAttacksData attackData)
        {
            RenderAttack(script, attackData);
        }
        // SFX Group.
        else if (script is GDEAudioGroupsData sfxGroup)
        {
            RenderSFXGroup(sfxGroup);
        }
        // Overworld party.
        else if (script is GDEPartyData partyData)
        {
            RenderPartyData(script, partyData);
        }
        // Tuning data.
        else if (script is GDEEntityTuningData tuningData)
        {
            RenderEntityTuningData(script, tuningData);
        }
        // Landmark data.
        else if (script is GDELandmarkData landmarkData)
        {
            RenderLandmarkData(script, landmarkData);
        }
        // Prefab data.
        else if (script is GDEPrefabData prefabData)
        {
            RenderPrefabData(script, prefabData);
        }
        else if (script is GDEInputCommandData inputData)
        {
            RenderInputData(script, inputData);
        }
        else if (script is GDETutorialSegmentData tutorialData)
        {
            RenderTutorialData(script, tutorialData);
        }
        else
        {

        }

        if (script is ISimulationData simData)
        {
            LABEL("Sim Data:", Color.grey);
            BEGIN_HOR();
            TAB(1);
            BEGIN_VERT();
            RenderSimData(script, simData);
            END_VERT();
            END_HOR();
        }

        //END_VERT();
        //END_HOR();
        _scriptsRendering--;

        DRAW_BACKGROUND(ColorUtility.voidPurple);
    }

    private void RenderTagObj(ITagObject tagObj)
    {
        if (tagObj == null) { return; }


    }

    private void RenderTag(GDETagsData tagData)
    {
        Scriptable remove = null;
        List<Scriptable> scriptsByTag = GetScriptsByTag(tagData);

        for (int n = 0; n < scriptsByTag.Count; n++)
        {
            Scriptable tagChild = scriptsByTag[n];

            BEGIN_HOR();
            DELETE_BTN(() =>
            {
                remove = tagChild;
            });
            BTN(tagChild.Key, Color.yellow, () =>
            {
                SELECT(tagChild);
                ClearScriptSearch();
            });
            END_HOR();
        }

        // Remove selected tag from script.
        if (remove != null)
        {
            RemoveTagFromScript(remove, tagData);
        }
    }

    private void RenderTagObjectSpawn(GDETagObjectSpawnData spawnData)
    {
        if (spawnData == null) { return; }

        BEGIN_HOR();
        BEGIN_DISABLED(string.IsNullOrEmpty(_newTagObjSpawnName) || ScriptExists(_newTagObjSpawnName));
        BTN("Create New Tag Obj Spawn", Color.green, () =>
        {
            if (!_newTagObjSpawnName.Contains("spawns")) { return; }
            GDETagObjectSpawnData newTagObjSpawn = CreateScriptableObject<GDETagObjectSpawnData>(_newTagObjSpawnName);
            GDETagsData newTagObjSpawnTag = CreateScriptableObject<GDETagsData>("tag_" + _newTagObjSpawnName);
            newTagObjSpawn.AddTag(newTagObjSpawnTag.Key);
            MarkDataNeedsReload();
        });
        END_DISABLED();
        _newTagObjSpawnName = TEXT_INPUT(_newTagObjSpawnName);
        END_HOR();

        int removeIndex = -1;
        float totalWeight = 0f;

        if (spawnData.Spawns == null)
        {
            spawnData.Spawns = new TagObjectSpawn[] { };
        }

        BEGIN_INDENT();

        BEGIN_HOR();
        LABEL("Min Iterations");
        spawnData.MinIterations = INT_INPUT(spawnData.MinIterations);
        LABEL("Max Iterations");
        spawnData.MaxIterations = INT_INPUT(spawnData.MaxIterations);
        END_HOR();

        int rolledSpawns = 0;

        for (int i = 0; i < spawnData.Spawns.Length; i++)
        {
            TagObjectSpawn spawn = spawnData.Spawns[i];
            BEGIN_HOR();
            LABEL(i, Color.gray);
            DELETE_BTN(() =>
            {
                removeIndex = i;
            });
            LABEL("Spawn");
            END_HOR();

            BEGIN_INDENT();
            spawn.Comment = COMMENT(spawn.Comment);
            BEGIN_HOR();
            spawn.LimitMaxCountByLocationItemMax = TOGGLE(spawn.LimitMaxCountByLocationItemMax, "Limit Max Count By Location Item Max");

            spawn.DisableRoll = TOGGLE(spawn.DisableRoll, "Is Guaranteed");
            if (!spawn.DisableRoll) rolledSpawns++;

            FLEX_SPACE();
            END_HOR();

            BEGIN_HOR();
            LABEL("Max Count");
            spawn.MaxCount = Mathf.Max(1, INT_INPUT(spawn.MaxCount));
            END_HOR();

            BEGIN_HOR();
            LABEL("Min Count");
            spawn.MinCount = Mathf.Max(1, INT_INPUT(spawn.MinCount));
            END_HOR();

            if (!spawn.DisableRoll)
            {
                BEGIN_HOR();
                spawn.Weight = FLOAT_INPUT(Mathf.Max(spawn.Weight, 0.001f), "Weighting", Color.white);
                totalWeight += spawn.Weight;

                float remaingingWeight = 1f;

                for (int n = 0; n < spawnData.Spawns.Length; n++)
                {
                    remaingingWeight -= spawnData.Spawns[n].Weight;
                }

                remaingingWeight = Mathf.Clamp01(remaingingWeight);

                BEGIN_DISABLED(remaingingWeight <= float.Epsilon);
                BTN("^", Color.yellow, () =>
                {
                    spawn.Weight = Mathf.Clamp01(spawn.Weight + remaingingWeight);
                }, 24);
                END_DISABLED();
                END_HOR();
            }

            // Tag.
            GDETagsData tagData = GetDataByID<GDETagsData>(spawn.TagID);
            BEGIN_HOR();
            LABEL("Tag", Color.cyan);
            bool spawnTagExists = ScriptExists(spawn.TagID);
            BEGIN_CLR(Color.cyan, Color.red, () =>
            {
                return spawnTagExists;
            });
            spawn.TagID = TEXT_INPUT(spawn.TagID);
            if (tagData != null) LABEL(GetScriptsByTagCount(tagData), Color.yellow);
            if (!spawnTagExists && !string.IsNullOrEmpty(spawn.TagID)) CREATE_SCRIPT_BTN<GDETagsData>(spawn.TagID);
            END_HOR();

            // Tag Obj.
            BEGIN_HOR();
            LABEL("Tag Object ID", Color.cyan);
            bool spawnTagObjExists = ScriptExists(spawn.TagObjectID);
            BEGIN_CLR(Color.cyan, Color.red, () =>
            {
                return spawnTagObjExists;
            });
            spawn.TagObjectID = TEXT_INPUT(spawn.TagObjectID);

            if (!string.IsNullOrEmpty(spawn.TagObjectID))
            {
                if (!spawnTagObjExists)
                {
                    Scriptable matchingTagObj = GetClosestScript(spawn.TagObjectID);
                    if (matchingTagObj != null) CREATE_SCRIPT_BTN(spawn.TagObjectID, matchingTagObj.ObjectType, $"New ({matchingTagObj.ObjectType})");
                }

            }
            END_CLR();
            END_HOR();

            spawnData.Spawns[i] = spawn;
            END_INDENT();
        }


        BEGIN_HOR();
        LABEL(spawnData.Spawns.Length, Color.gray);
        BTN("Add Spawn", Color.green, () =>
        {
            if (spawnData.Spawns == null) spawnData.Spawns = new TagObjectSpawn[0];
            List<TagObjectSpawn> spawns = new List<TagObjectSpawn>(spawnData.Spawns);

            spawns.Add(new TagObjectSpawn()
            {
                Comment = "New Spawn",
                Weight = Mathf.Clamp01(1f - totalWeight)
            });

            spawnData.Spawns = spawns.ToArray();
        });
        END_HOR();

        BEGIN_HOR();
        BEGIN_CLR(Color.green, Color.red, () => { return Mathf.Approximately(1f, totalWeight); });
        LABELS("Total Weight:", totalWeight);
        END_CLR();

        if (!Mathf.Approximately(1f, totalWeight))
        {
            BEGIN_CLR(Color.yellow);
            LABELS("Remaining Weight:", 1f - totalWeight);
            END_CLR();
        }
        END_HOR();

        if (removeIndex != -1)
        {
            List<TagObjectSpawn> spawns = new List<TagObjectSpawn>(spawnData.Spawns);
            spawns.RemoveAt(removeIndex);
            spawnData.Spawns = spawns.ToArray();
        }
        END_INDENT();
    }

    private void RenderBiome(GDEBiomesData data)
    {
        if (data == null) { return; }

        MARK_DIRTY(data);

        bool showBiomeLayers = EXPAND_TOGGLE("Terrain Gen:", ColorUtility.earthBrown, -1, data.Key);

        GDEBiomesData.TerrainGen terrainGen = data.TerrainGenSettings;

        if (showBiomeLayers)
        {
            terrainGen.Layers = RenderTerrainLayers(terrainGen.Layers);
        }

        bool showBiomeCaves = EXPAND_TOGGLE("Cave Gen:", ColorUtility.caveBlue, -1, data.Key);

        // Caves.
        if (showBiomeCaves)
        {
            BEGIN_INDENT();
            for (int i = 0; i < terrainGen.Caves.Length; i++)
            {
                GDEBiomesData.CaveLayer cave = terrainGen.Caves[i];

                if (cave == null)
                {
                    cave = new CaveLayer();
                    terrainGen.Caves[i] = cave;
                }

                GDEBiomesData.CaveLayer[] arr = terrainGen.Caves;
                int minZ = (int)(cave.Min * 255);
                int maxZ = (int)(cave.Max * 255);

                BEGIN_HOR();
                BEGIN_CLR(ColorUtility.caveBlue);
                arr = ARRAY_REMOVE_BTN(arr, i);

                LABEL("Cave Strata");

#if ODD_REALM_APP
                if (Application.isPlaying && ActiveTile != null && ActiveTile.BiomeData == data)
                {
                    BTN("Find Next", () =>
                    {
                        bool found = false;
                        for (int z = maxZ; !found && z >= minZ; z--)
                        {
                            for (int x = 0; !found && x < Session.WorldDimensions.Width; x++)
                            {
                                for (int y = 0; !found && y < Session.WorldDimensions.Height; y++)
                                {
                                    Location l = Session.World.Map.GetLocation(x, y, z);
                                    if (l.IsVisible) { continue; }
                                    TerrainBuildOutput terrainData = TerrainBuilder.GetTerrainAtPoint(l.x, l.y, l.z, Session.Realm.Seed, ActiveTile.TerrainGen);
                                    TagUID prototype = (uint)(terrainData.TagUID > 0 ? terrainData.TagUID : 1);
                                    if (prototype != TerrainBuilder.CAVE_TERRAIN) { continue; }
                                    Session.World.MarkLocationVisible(l.locationUID);
                                    found = true;
                                    l.Point.RenderPlus(Color.magenta, 1f, 1f, true);
                                    Session.Selections.CurrentSelection.FocusPoint(l.Point);
                                }
                            }
                        }
                    });
                }
#endif

                END_CLR();
                END_HOR();

                BEGIN_INDENT();
                cave.Comment = COMMENT(cave.Comment);

                // Distribution curve.
                BEGIN_HOR();
                cave.StrengthDistribution = ANIM_CURVE(cave.StrengthDistribution, "Size Distribution (Over Z)");
                END_HOR();

                BEGIN_HOR();
                LABEL("Max Z:");
                LABEL(maxZ);
                cave.Max = FLOAT_INPUT01(cave.Max);
                END_HOR();


                BEGIN_HOR();
                LABEL("Min Z:");
                LABEL(minZ);
                cave.Min = FLOAT_INPUT01(cave.Min);
                END_HOR();

                bool showThemes = EXPAND_TOGGLE("Caves:", Color.cyan, -1, cave.Comment);

                if (showThemes)
                {
                    BEGIN_INDENT();

                    for (int n = 0; n < cave.InteriorSpawnTags.Length; n++)
                    {
                        string[] caveTags = cave.InteriorSpawnTags;

                        BEGIN_HOR();
                        caveTags = ARRAY_REMOVE_BTN<string>(caveTags, n);
                        GDECaveData caveData = null;

                        if (n < caveTags.Length)
                        {
                            caveTags[n] = DATA_ID_INPUT<GDECaveData>(caveTags[n], "Cave ID", out caveData);
                        }

                        END_HOR();
                        BEGIN_INDENT();

                        if (caveData != null)
                        {
                            bool showCaveLayers = EXPAND_TOGGLE("Cave Layers (TagID):", ColorUtility.caveBlue, -1, caveData.Key);
                            BEGIN_INDENT();

                            if (showCaveLayers)
                            {
                                RenderCave(caveData);
                            }
                            END_INDENT(ColorUtility.caveBlue);
                        }

                        cave.InteriorSpawnTags = caveTags;
                        END_INDENT();
                    }

                    cave.InteriorSpawnTags = ARRAY_ADD_BTN<string>(cave.InteriorSpawnTags, "+Spawn");

                    terrainGen.Caves = arr;
                    END_INDENT(ColorUtility.caveBlue);
                }

                END_INDENT();
            }

            terrainGen.Caves = ARRAY_ADD_BTN<GDEBiomesData.CaveLayer>(terrainGen.Caves, "+Cave");
            END_INDENT();
        }

        bool showBiomePlants = EXPAND_TOGGLE("Props/Plants Gen:", ColorUtility.plantGreen, -1, data.Key);

        // Plants and Props.
        if (showBiomePlants)
        {
            for (int i = 0; terrainGen.PlantsToGenerate != null && i < terrainGen.PlantsToGenerate.Length; i++)
            {
                GDEBiomesData.PlantGen plantGen = terrainGen.PlantsToGenerate[i];

                if (plantGen == null)
                {
                    plantGen = new PlantGen();
                    terrainGen.PlantsToGenerate[i] = plantGen;
                }

                GDETagsData tagData = GetDataByID<GDETagsData>(plantGen.TagID);

                BEGIN_HOR();
                GDEBiomesData.PlantGen[] arr = ARRAY_REMOVE_BTN<GDEBiomesData.PlantGen>(terrainGen.PlantsToGenerate, i);

                LABEL("Prop/Plant", ColorUtility.plantGreen);
                BEGIN_CLR(Color.cyan, Color.red, () => { return tagData != null; });
                plantGen.TagID = TEXT_INPUT(plantGen.TagID);
                END_CLR();
                ASSERT_WARNING(tagData != null, "Missing tag!");
                END_HOR();

                BEGIN_HOR();
                TAB(1);
                int maxZ = (int)(plantGen.PlantCeiling * 255);
                LABEL("Max Z:");
                LABEL(maxZ);
                plantGen.PlantCeiling = FLOAT_INPUT01(plantGen.PlantCeiling);
                END_HOR();

                BEGIN_HOR();
                TAB(1);
                int minZ = (int)(plantGen.PlantFloor * 255);
                LABEL("Min Z:");
                LABEL(minZ);
                plantGen.PlantFloor = FLOAT_INPUT01(plantGen.PlantFloor);
                END_HOR();

                if (tagData != null)
                {
                    List<Scriptable> tagObjs = GetScriptsByTag(tagData);

                    BEGIN_HOR();
                    TAB(1);
                    LABEL(tagObjs == null ? 0 : tagObjs.Count, Color.yellow);
                    LABEL("Children:", Color.yellow);
                    END_HOR();

                    for (int n = 0; n < tagObjs.Count; n++)
                    {
                        BEGIN_HOR();
                        TAB(1);
                        RenderScript(tagObjs[n]);
                        END_HOR();
                    }
                }

                terrainGen.PlantsToGenerate = arr;
            }

            terrainGen.PlantsToGenerate = ARRAY_ADD_BTN<GDEBiomesData.PlantGen>(terrainGen.PlantsToGenerate, "+Plant Gen");
        }

        bool showBiomeFloraPopulations = EXPAND_TOGGLE("Plant Population Limits:", ColorUtility.purple, -1, data.Key);

        // Population.
        if (showBiomeFloraPopulations)
        {
            if (_lastBiomeSelection != data.Key)
            {
                _lastBiomeSelection = data.Key;
                _rebuildPlantsInBiome = true;
            }

            int popCount = data.DefaultFloraPopulationModifiers.Length;

            for (int i = 0; i < data.DefaultFloraPopulationModifiers.Length; i++)
            {
                GDEBiomesData.DefaultPopulationRating pop = data.DefaultFloraPopulationModifiers[i];

                BEGIN_HOR();
                GDEBiomesData.DefaultPopulationRating[] arr = ARRAY_REMOVE_BTN<GDEBiomesData.DefaultPopulationRating>(data.DefaultFloraPopulationModifiers, i);
                pop.Rating = (PopulationRatings)DROP_DOWN(pop.Rating, 100);
                pop.TagObjID = TEXT_INPUT(pop.TagObjID);
                //FLEX_SPACE();
                END_HOR();
                data.DefaultFloraPopulationModifiers[i] = pop;
                data.DefaultFloraPopulationModifiers = arr;
            }

            data.DefaultFloraPopulationModifiers = ARRAY_ADD_BTN<GDEBiomesData.DefaultPopulationRating>(data.DefaultFloraPopulationModifiers, "+Pop. Modifier");
            _rebuildPlantsInBiome |= popCount != data.DefaultFloraPopulationModifiers.Length;
            BEGIN_CLR(Color.yellow);
            BTN("Rebuild Plant Default Pop List", () => { _rebuildPlantsInBiome = true; });
            END_CLR();

            if (_rebuildPlantsInBiome)
            {
                _rebuildPlantsInBiome = false;
                _plantsInDefaultPop.Clear();
                _plantsNotInDefaultPop.Clear();

                for (int i = 0; i < data.DefaultFloraPopulationModifiers.Length; i++)
                {
                    _plantsInDefaultPop.Add(data.DefaultFloraPopulationModifiers[i].TagObjID);
                }

                for (int i = 0; i < data.TerrainGenSettings.PlantsToGenerate.Length; i++)
                {
                    Scriptable plantSpawnTag = GetDataByID(data.TerrainGenSettings.PlantsToGenerate[i].TagID) as Scriptable;

                    if (plantSpawnTag == null) { continue; }

                    List<Scriptable> tagObjsByTag = GetScriptsByTag(plantSpawnTag);

                    for (int n = 0; n < tagObjsByTag.Count; n++)
                    {
                        GDETagObjectSpawnData spawn = GetDataByID<GDETagObjectSpawnData>(tagObjsByTag[n].Key);

                        if (spawn != null)
                        {
                            for (int l = 0; l < spawn.Spawns.Length; l++)
                            {
                                Scriptable spawnTag = GetDataByID(spawn.Spawns[l].TagID);

                                if (spawnTag == null) { continue; }

                                List<Scriptable> spawnTagObjs = GetScriptsByTag(spawnTag);

                                for (int s = 0; s < spawnTagObjs.Count; s++)
                                {
                                    Scriptable spawnTagObj = spawnTagObjs[s];

                                    // Flora.
                                    if (spawnTagObj is GDEBlockPlantsData plantData && !_plantsInDefaultPop.Contains(plantData.Key))
                                    {
                                        _plantsNotInDefaultPop.Add(plantData.Key);
                                    }
                                }
                            }
                        }
                    }
                }
            }



            LABEL("Plants With Pop Rating:");
            BEGIN_INDENT();

            foreach (string plant in _plantsInDefaultPop)
            {
                LABEL(plant);
            }

            END_INDENT();

            LABEL("Plants Without Pop Rating:");
            BEGIN_INDENT();
            BEGIN_CLR(Color.yellow);

            foreach (string plant in _plantsNotInDefaultPop)
            {
                BEGIN_HOR();
                LABEL(plant, 180);

                foreach (PopulationRatings rating in System.Enum.GetValues(typeof(PopulationRatings)))
                {
                    if (rating == PopulationRatings.COUNT) { continue; }

                    BTN(rating.ToString(), () =>
                    {
                        DefaultPopulationRating r = new DefaultPopulationRating()
                        {
                            TagObjID = plant,
                            Rating = rating,
                        };

                        data.DefaultFloraPopulationModifiers = AddToArray(data.DefaultFloraPopulationModifiers, r);
                        _rebuildPlantsInBiome = true;
                    });
                }

                END_HOR();
            }

            END_CLR();
            END_INDENT();
        }
    }

    private string _lastBiomeSelection;
    private bool _rebuildPlantsInBiome;
    private HashSet<string> _plantsInDefaultPop = new HashSet<string>();
    private HashSet<string> _plantsNotInDefaultPop = new HashSet<string>();

    private void RenderScenario(GDEScenariosData scenariosData)
    {
        if (scenariosData == null) { return; }

        MARK_DIRTY(scenariosData);
    }

    private void RenderCave(GDECaveData caveData)
    {
        if (caveData == null) { return; }

        MARK_DIRTY(caveData);

        caveData.Layers = RenderTerrainLayers(caveData.Layers);
    }

    private GDEBiomesData.TerrainLayer[] RenderTerrainLayers(GDEBiomesData.TerrainLayer[] layers)
    {
        for (int i = 0; layers != null && i < layers.Length; i++)
        {
            GDEBiomesData.TerrainLayer terrainLayer = layers[i];

            if (terrainLayer == null)
            {
                terrainLayer = new GDEBiomesData.TerrainLayer();
                layers[i] = terrainLayer;
            }

            GDETagsData tagData = GetDataByID<GDETagsData>(terrainLayer.TagID);

            BEGIN_HOR();
            layers = ARRAY_REMOVE_BTN<GDEBiomesData.TerrainLayer>(layers, i);
            LABEL("Layer", ColorUtility.earthBrown);
            END_HOR();

            BEGIN_INDENT();
            terrainLayer.Comment = COMMENT(terrainLayer.Comment);

            BEGIN_HOR();
            int maxZ = (int)(terrainLayer.Max * 10);
            LABEL("Max Z (in cave of height 10):");
            LABEL(maxZ);
            terrainLayer.Max = FLOAT_INPUT01(terrainLayer.Max);
            END_HOR();

            BEGIN_HOR();
            BEGIN_CLR(Color.yellow, Color.red, () => { return tagData != null; });
            terrainLayer.TagID = TEXT_INPUT(terrainLayer.TagID);
            END_CLR();
            if (tagData != null) LABEL(GetScriptsByTagCount(tagData), Color.yellow);
            if (tagData == null && !string.IsNullOrEmpty(terrainLayer.TagID)) CREATE_SCRIPT_BTN<GDETagsData>(terrainLayer.TagID);
            ASSERT_WARNING(tagData != null, "Missing tag!");
            END_HOR();


            if (tagData != null)
            {
                List<Scriptable> tagObjs = GetScriptsByTag(tagData);

                BEGIN_HOR();
                LABEL($"Tag Children ({(tagObjs == null ? 0 : tagObjs.Count)}):", Color.yellow);
                END_HOR();

                for (int n = 0; n < tagObjs.Count; n++)
                {
                    BEGIN_INDENT();
                    BTN($"{tagObjs[n].TooltipName} ({tagObjs[n].Key})", Color.yellow, () => { SELECT(tagObjs[n]); });

                    if (tagObjs[n] is GDETagObjectSpawnData spawn)
                    {
                        RenderTagObjectSpawn(spawn);
                    }
                    END_INDENT(Color.yellow);
                }
            }
            END_INDENT(ColorUtility.earthBrown);
        }

        layers = ARRAY_ADD_BTN<GDEBiomesData.TerrainLayer>(layers, "+Terrain Layer");

        return layers;
    }

    protected void RenderSimData(Scriptable script, ISimulationData simData)
    {
        if (!string.IsNullOrEmpty(_simToAddToSelections))
        {
            if (_simToAddToSelections == simData.SimulationID)
            {
                LABEL("Has Sim", Color.green);
            }
            else
            {
                if (!string.IsNullOrEmpty(simData.SimulationID))
                {
                    LABEL("Overwrite " + _simToAddToSelections + "->" + simData.SimulationID, Color.yellow);
                }
                else
                {
                    LABEL("Add " + _simToAddToSelections, Color.green);
                }
            }
        }

        simData.SetSimulationID(DATA_ID_INPUT<GDESimulationData>(simData.SimulationID, "Simulation ID (GDESimulationData)", out var simScriptObj));

        if (simScriptObj != null)
        {

            int removeIndex = -1;
            int shiftUpIndex = -1;
            int shiftDownIndex = -1;

            MARK_DIRTY(simScriptObj);

            for (int n = 0; n < simScriptObj.Simulations.Length; n++)
            {
                InstanceSimulation sim = simScriptObj.Simulations[n];

                if (sim == null)
                {
                    sim = new InstanceSimulation();
                    simScriptObj.Simulations[n] = sim;
                }

                if (n > 0)
                {
                    //SPACE(12);
                }

                EntryOutput output = RenderSim(n, simData, simScriptObj, sim);

                if (sim.DebugMaximized != output.Expanded)
                {
                    sim.DebugMaximized = output.Expanded;
                    GUI.FocusControl(null);
                }

                if (output.RemoveIndex != -1)
                {
                    removeIndex = n;
                }

                if (output.ShiftUpIndex != -1)
                {
                    shiftUpIndex = n;
                }

                if (output.ShiftDownIndex != -1)
                {
                    shiftDownIndex = n;
                }

            }

            simScriptObj.Simulations = ARRAY_ADD_BTN<InstanceSimulation>(simScriptObj.Simulations, "+Simulation");
            //if (_selectedSim != null)
            {
            }

            if (shiftUpIndex != -1)
            {
                SwapSimulations(shiftUpIndex, shiftUpIndex - 1, simScriptObj);
            }

            if (shiftDownIndex != -1)
            {
                SwapSimulations(shiftDownIndex, shiftDownIndex + 1, simScriptObj);
            }

            // Remove sim
            if (removeIndex > -1)
            {
                RemoveSimulation(removeIndex, simScriptObj);
            }
        }
    }

    private void RenderSimData(Scriptable script, GDESimulationData simData)
    {
        if (simData == null) { return; }

    }

    private void RenderItem(Scriptable script, GDEItemsData itemData)
    {
        if (itemData == null) { return; }

        MARK_DIRTY(itemData);

        itemData.ItemType = DATA_ID_INPUT<GDETagsData>(itemData.ItemType, "Item Type", out var itemType);
        itemData.ItemRarity = DATA_ID_INPUT<GDEItemRarityData>(itemData.ItemRarity, "Rarity ID", out var rarityData);

        if (rarityData != null)
        {
            //RenderScript(rarityData);
        }

        bool showAttacks = EXPAND_TOGGLE("Attacks:", ColorUtility.warningYellow, -1, itemData.Key);

        if (showAttacks)
        {
            BEGIN_INDENT();
            itemData.AttackGroup = DATA_ID_INPUT<GDEAttackGroupsData>(itemData.AttackGroup, "Attack Group", out var attackGroupData);

            RenderAttackGroup(attackGroupData);
            END_INDENT();
        }

        bool showActions = EXPAND_TOGGLE("Item Actions:", ColorUtility.plantGreen, -1, itemData.Key);

        if (showActions)
        {
            BEGIN_INDENT();

            LABEL("Timed Activators:");

            // Timed actions.
            for (int i = 0; i < itemData.TimedActions.Length; i++)
            {
                BEGIN_HOR();
                if (TRY_ARRAY_REMOVE_BTN<AutomatedItemActionActivation>(itemData.TimedActions, i, out var newArr))
                {
                    itemData.TimedActions = newArr;
                    END_HOR();
                    continue;
                }

                LABEL("Timed Action Activator");
                string prevActionID = itemData.TimedActions[i].ActionID;
                END_HOR();

                BEGIN_INDENT();

                itemData.TimedActions[i].ActionID = DATA_ID_INPUT<GDETagsData>(itemData.TimedActions[i].ActionID, "Action ID", out var actionTagData);

                if (actionTagData == null)
                {
                    WARNING("Action ID must be a valid tag!");
                }

                BEGIN_INDENT();

                itemData.TimedActions[i].ActivationTime = (uint)INT_INPUT((int)itemData.TimedActions[i].ActivationTime, "Activation Time");
                itemData.TimedActions[i].LocationRequirementTagObjectID = DATA_ID_INPUT<Scriptable>(itemData.TimedActions[i].LocationRequirementTagObjectID, "Location Requirement", out var locationReqData);
                itemData.TimedActions[i].DisposeItem = TOGGLE(itemData.TimedActions[i].DisposeItem, "Dispose Item On Activation");
                END_INDENT();
                END_INDENT();
            }

            itemData.TimedActions = ARRAY_ADD_BTN<AutomatedItemActionActivation>(itemData.TimedActions, "+Timed Item Action Activators");

            LABEL("Actions:");

            // Actions.
            for (int i = 0; i < itemData.Actions.Length; i++)
            {
                BEGIN_HOR();
                if (TRY_ARRAY_REMOVE_BTN<ItemAction>(itemData.Actions, i, out var newArr))
                {
                    itemData.Actions = newArr;
                    END_HOR();
                    continue;
                }

                LABEL("Action");
                string prevActionID = itemData.Actions[i].ActionID;
                //itemData.Actions[i].ActionID = TEXT_INPUT(itemData.Actions[i].ActionID);
                END_HOR();

                BEGIN_INDENT();

                itemData.Actions[i].ActionID = DATA_ID_INPUT<GDETagsData>(itemData.Actions[i].ActionID, "Action ID", out var actionTagData);

                if (actionTagData == null)
                {
                    WARNING("Action ID must be a valid tag!");
                }

                BEGIN_INDENT();

                // Item Buff.
                itemData.Actions[i].Buff = RenderBuffData(script, itemData.Actions[i].Buff);

                // Item Status.
                itemData.Actions[i].Status = DATA_ID_INPUT<GDEEntityStatusData>(itemData.Actions[i].Status, "Status", out var statusData);
                BEGIN_INDENT();
                RenderEntityStatusData(script, statusData);
                END_INDENT();

                // Item Spawn.
                itemData.Actions[i].SpawnID = DATA_ID_INPUT<Scriptable>(itemData.Actions[i].SpawnID, "Spawn", out var spawnData);

                END_INDENT();
                END_INDENT();
            }

            itemData.Actions = ARRAY_ADD_BTN<ItemAction>(itemData.Actions, "+Item Action");
            END_INDENT();
        }
    }

    // Fish.
    private void RenderFish(Scriptable script, GDEFishData fishData)
    {
        if (fishData == null) { return; }

        MARK_DIRTY(fishData);

        LABEL("Biomes:", Color.green);
        BEGIN_INDENT();

        for (int n = 0; n < fishData.Biomes.Count; n++)
        {
            BEGIN_HOR();

            if (TRY_LIST_REMOVE_BTN<string>(fishData.Biomes, n, out var newList))
            {
                fishData.Biomes = newList;
                END_HOR();
                continue;
            }

            fishData.Biomes[n] = DATA_ID_INPUT<GDEBiomesData>(fishData.Biomes[n], "Biome", out var biomeData);

            END_HOR();
        }

        fishData.Biomes = LIST_ADD_BTN<string>(fishData.Biomes, "+Biome");
        END_INDENT();
    }

    // Buffs.
    private BuffData[] RenderBuffs(Scriptable script, BuffData[] buffs)
    {
        if (buffs == null)
        {
            buffs = new BuffData[] { };
        }

        LABEL("Buffs:", Color.grey);

        BEGIN_INDENT();
        //BEGIN_VERT();

        for (int n = 0; n < buffs.Length; n++)
        {
            BEGIN_HOR();
            buffs = ARRAY_REMOVE_BTN<BuffData>(buffs, n);

            if (n < buffs.Length)
            {
                buffs[n] = RenderBuffData(script, buffs[n]);
            }

            END_HOR();
        }

        buffs = ARRAY_ADD_BTN<BuffData>(buffs, "+Buff");
        //buffs[buffs.Length - 1].Permanent = true;
        //END_VERT();
        END_INDENT();

        return buffs;
    }

    // Buff Data.
    private BuffData RenderBuffData(Scriptable script, BuffData buffData)
    {
        buffData.TargetID = DATA_ID_INPUT<Scriptable>(buffData.TargetID, "Buff", out var targetData);

        if (targetData != null)
        {
            BEGIN_INDENT();
            buffData.Amount = INT_INPUT(buffData.Amount, "Amount");
            buffData.Max = INT_INPUT(buffData.Max, "Max");
            buffData.Permanent = TOGGLE(buffData.Permanent, "Permanent");
            END_INDENT();
        }

        return buffData;
    }

    // Attack Group.
    private void RenderAttackGroup(GDEAttackGroupsData attackGroupData)
    {
        if (attackGroupData == null) { return; }

        MARK_DIRTY(attackGroupData);
        BEGIN_INDENT();

        for (int i = 0; i < attackGroupData.Attacks.Count; i++)
        {
            BEGIN_HOR();

            if (TRY_LIST_REMOVE_BTN<string>(attackGroupData.Attacks, i, out var newList))
            {
                attackGroupData.Attacks = newList;
                END_HOR();
                continue;
            }

            attackGroupData.Attacks[i] = DATA_ID_INPUT<GDEAttacksData>(attackGroupData.Attacks[i], "Attack", out var attackData);
            END_HOR();

            BEGIN_INDENT();
            RenderAttack(attackData, attackData);
            END_INDENT();
        }

        attackGroupData.Attacks = LIST_ADD_BTN<string>(attackGroupData.Attacks, "+Attack");
        END_INDENT();
    }

    private void RenderLandmarkData(Scriptable script, GDELandmarkData landmarkData)
    {
        if (landmarkData == null) { return; }

        MARK_DIRTY(landmarkData);
        BEGIN_INDENT();

        for (int i = 0; i < landmarkData.Prefabs.Length; i++)
        {
            BEGIN_HOR();

            if (TRY_ARRAY_REMOVE_BTN<GDELandmarkData.PrefabSettings>(landmarkData.Prefabs, i, out var newArr))
            {
                landmarkData.Prefabs = newArr;
                END_HOR();
                continue;
            }

            END_HOR();


            BEGIN_INDENT();

            landmarkData.Prefabs[i].PrefabID = DATA_ID_INPUT<GDEPrefabData>(landmarkData.Prefabs[i].PrefabID, "Prefab", out var prefabData);

            END_INDENT();
        }

        landmarkData.Prefabs = ARRAY_ADD_BTN<GDELandmarkData.PrefabSettings>(landmarkData.Prefabs, "+Prefab");

        END_INDENT();
    }

    //private string _prefabPaintID = "";
    private bool _paintingPrefab;
    private int _paintLayer = 0;
    private int _paintMaxX = 4;
    private int _paintMaxY = 4;
    private int _editZ;

    private Dictionary<int, List<int>> _prefabLocationIndices = new Dictionary<int, List<int>>();

    private void RenderPrefabData(Scriptable script, GDEPrefabData prefabData)
    {
        if (prefabData == null) { return; }

        MARK_DIRTY(prefabData);
        BEGIN_INDENT();

        //_prefabPaintID = DATA_ID_INPUT<Scriptable>(_prefabPaintID, "Paint ID", out var data);
        _paintingPrefab = Event.current.modifiers != EventModifiers.Control;
        TOGGLE(_paintingPrefab, "Ctrl to Paint");
        LABEL("Painting: " + _copiedText, Color.yellow);
        if (!string.IsNullOrEmpty(_copiedText))
        {
            if (TryGetDataByID<Scriptable>(_copiedText, out var paintData) && paintData.TooltipIconSprite != null)
            {
                Sprite s = paintData.TooltipIconSprite;
                float w = s.rect.width * 2;
                float h = s.rect.height * 2;
                GUILayout.Label("", GUILayout.Width(w + 16), GUILayout.Height(h + 16));
                Rect pos = GUILayoutUtility.GetLastRect();
                Texture2D texture = s.texture;

                Rect rect = new Rect(pos.x + 8, pos.y + 8, w, h);
                EditorGUI.DrawRect(new Rect(pos.x + 8, pos.y + 8, rect.width, rect.height), new Color(0.1f, 0.1f, 0.1f, 1f));
                GUI.DrawTexture(rect, texture);
            }
            else
            {
                GUILayout.Label("", GUILayout.Width(48), GUILayout.Height(48));

            }
        }

        _prefabLocationIndices.Clear();

        int maxDim = 8;
        int maxDimBits = 3;

        for (int i = 0; i < prefabData.Locations.Length; i++)
        {
            GDEPrefabData.LocationData locationData = prefabData.Locations[i];

            int index = locationData.Point.x | (locationData.Point.y << maxDimBits) | (locationData.Point.z << maxDimBits << maxDimBits);

            if (!_prefabLocationIndices.TryGetValue(index, out var locations))
            {
                locations = new List<int>();
                _prefabLocationIndices[index] = locations;
            }

            locations.Add(i);
        }

        BEGIN_HOR();
        _paintMaxX = INT_INPUT(_paintMaxX, "Width");
        _paintMaxY = INT_INPUT(_paintMaxY, "Height");
        END_HOR();

        BEGIN_HOR();
        LABEL(_editZ);
        LABEL("z");

        BEGIN_VERT();
        BTN("^", () => { if (_editZ < maxDim - 1) _editZ++; });
        BTN("v", () => { if (_editZ > 0) _editZ--; });
        END_VERT();

        END_HOR();

        int width = Mathf.Min(_paintMaxX, maxDim);
        int height = Mathf.Min(_paintMaxY, maxDim);

        BlockPoint newLocation = BlockPoint.NULL;
        string newTagID = "";
        int removeIndex = -1;

        BEGIN_HOR();
        for (int x = 0; x < width; x++)
        {
            BEGIN_VERT();
            for (int y = 0; y < height; y++)
            {
                int index = x | (y << maxDimBits) | (_editZ << maxDimBits << maxDimBits);
                int btnCount = 1;

                if (_prefabLocationIndices.TryGetValue(index, out var locations))
                {
                    btnCount += locations.Count;

                    for (int i = 0; i < locations.Count; i++)
                    {
                        int locationIndex = locations[i];
                        //SPACE(3);
                        GDEPrefabData.LocationData locationData = prefabData.Locations[locationIndex];
                        BEGIN_HOR();
                        //SPACE(6);

                        RenderPrefabLocationLayerBtn(locationData.TagObjectID, btnCount, (string output) =>
                        {

                            if (_paintingPrefab)
                            {
                                locationData.TagObjectID = _copiedText;

                                if (string.IsNullOrEmpty(locationData.TagObjectID))
                                {
                                    removeIndex = locationIndex;
                                }
                            }
                            else
                            {
                                _copiedText = locationData.TagObjectID;
                            }
                        });
                        END_HOR();
                        prefabData.Locations[locationIndex] = locationData;
                    }
                }

                RenderPrefabLocationLayerBtn((_paintingPrefab && !string.IsNullOrEmpty(_copiedText)) ? "+" : "", btnCount, (string output) =>
                {
                    if (_paintingPrefab)
                    {
                        newLocation = new BlockPoint(x, y, _editZ);
                        newTagID = _copiedText;
                    }
                    else
                    {
                        _copiedText = "";
                    }
                });
            }
            END_VERT();

            if (x == width - 1) FLEX_SPACE();
        }
        END_HOR();

        if (!newLocation.IsNULL)
        {
            prefabData.Locations = AddToArray<GDEPrefabData.LocationData>(prefabData.Locations, new GDEPrefabData.LocationData()
            {
                TagObjectID = newTagID,
                X = newLocation.x,
                Y = newLocation.y,
                Z = newLocation.z,
            });
        }

        if (removeIndex != -1)
        {
            prefabData.Locations = RemoveFromArray<GDEPrefabData.LocationData>(prefabData.Locations, removeIndex);
        }

        //BEGIN_HOR();
        //LABEL("Paint Layer");
        //BTN("0", () => { _paintLayer = 0; });
        //BTN("1", () => { _paintLayer = 1; });
        //BTN("2", () => { _paintLayer = 2; });
        //END_HOR();



        //for (int i = 0; i < prefabData.Layers.Length; i++)
        //{
        //    GDEPrefabData.PrefabLayer layer = prefabData.Layers[i];

        //    if (layer == null)
        //    {
        //        layer = new GDEPrefabData.PrefabLayer();
        //        prefabData.Layers[i] = layer;
        //    }

        //    BEGIN_HOR();

        //    if (TRY_ARRAY_REMOVE_BTN<GDEPrefabData.PrefabLayer>(prefabData.Layers, i, out var newArr))
        //    {
        //        prefabData.Layers = newArr;
        //        END_HOR();
        //        continue;
        //    }

        //    END_HOR();

        //    BEGIN_INDENT();

        //    if (layer.Columns == null || layer.Columns.Length < 1)
        //    {
        //        layer.Columns = new GDEPrefabData.Column[1];
        //        layer.Columns[0] = new GDEPrefabData.Column();
        //        layer.Columns[0].Locations = new GDEPrefabData.LocationData[1];
        //    }

        //    BEGIN_HOR();
        //    int width = layer.Columns.Length;
        //    width = Mathf.Max(1, INT_INPUT(width, "width"));

        //    int height = layer.Columns.Length > 0 ? layer.Columns[0].Locations.Length : 0;
        //    height = Mathf.Max(1, INT_INPUT(height, "height"));
        //    END_HOR();


        //    if (width != layer.Columns.Length || height != layer.Columns[0].Locations.Length)
        //    {
        //        // Create a new array with the updated dimensions
        //        var newColumns = new GDEPrefabData.Column[width];
        //        for (int n = 0; n < width; n++)
        //        {
        //            newColumns[n] = new GDEPrefabData.Column
        //            {
        //                Locations = new GDEPrefabData.LocationData[height]
        //            };
        //        }

        //        // Copy the data from the old array to the new array
        //        for (int n = 0; n < Mathf.Min(width, layer.Columns.Length); n++)
        //        {
        //            for (int j = 0; j < Mathf.Min(height, layer.Columns[n].Locations.Length); j++)
        //            {
        //                newColumns[n].Locations[j] = layer.Columns[n].Locations[j];
        //            }
        //        }

        //        // Replace the old array with the new array
        //        layer.Columns = newColumns;
        //    }

        //    BEGIN_HOR();
        //    for (int bx = 0; bx < width; bx++)
        //    {
        //        BEGIN_VERT();
        //        for (int by = 0; by < height; by++)
        //        {
        //            BOX(150, 152, Color.black*0.5f);
        //            SPACE(3);
        //            GDEPrefabData.LocationData locationData = layer.Columns[bx].Locations[by];
        //            BEGIN_HOR();
        //            SPACE(6);
        //            locationData.TagObjectID0 = RenderPrefabLocationLayerBtn(locationData.TagObjectID0);
        //            END_HOR();
        //            BEGIN_HOR();
        //            SPACE(6);
        //            locationData.TagObjectID1 = RenderPrefabLocationLayerBtn(locationData.TagObjectID1);
        //            END_HOR();
        //            BEGIN_HOR();
        //            SPACE(6);
        //            locationData.TagObjectID2 = RenderPrefabLocationLayerBtn(locationData.TagObjectID2);
        //            END_HOR();
        //            layer.Columns[bx].Locations[by] = locationData;

        //            SPACE(8);
        //        }
        //        END_VERT();
        //        //EXPAND_WIDTH();
        //        SPACE(8);

        //        if (bx == width-1) FLEX_SPACE();
        //    }

        //    END_HOR();

        //    END_INDENT();
        //}

        //prefabData.Layers = ARRAY_ADD_BTN<GDEPrefabData.PrefabLayer>(prefabData.Layers, "+Layer");

        END_INDENT();
    }

    private void RenderInputData(Scriptable script, GDEInputCommandData inputData)
    {
        BEGIN_INDENT();

        inputData.Category = (GDEInputCommandData.InputCategories)DROP_DOWN(inputData.Category, "Input Category");

        END_INDENT();
    }

    private void RenderTutorialData(Scriptable script, GDETutorialSegmentData tutorialData)
    {
        if (tutorialData == null) { return; }

        BEGIN_INDENT();
        BEGIN_HOR();

        if (!string.IsNullOrEmpty(tutorialData.Icon) && TryGetSprite(tutorialData.Icon, out var sprite))
        {
            BEGIN_SPRITE(sprite);
        }

        tutorialData.Icon = TEXT_INPUT(tutorialData.Icon, "Icon ID");

        END_HOR();

        tutorialData.Comment = COMMENT(tutorialData.Comment);
        //BEGIN_HOR();
        tutorialData.Trigger = (TutorialActivationTriggers)DROP_DOWN(tutorialData.Trigger, "Trigger");

        if (tutorialData.Trigger == TutorialActivationTriggers.SCALED_ELAPSED_PLAY_TIME)
        {
            tutorialData.MinScaledPlayedTime = INT_INPUT((int)tutorialData.MinScaledPlayedTime, "Min Scaled Time");
        }
        else if (tutorialData.Trigger == TutorialActivationTriggers.UNSCALED_ELAPSED_PLAY_TIME)
        {
            tutorialData.MinUnscaledPlayedTime = INT_INPUT((int)tutorialData.MinUnscaledPlayedTime, "Min Unscaled Time");
        }
        else if (tutorialData.Trigger == TutorialActivationTriggers.SELECTION_INPUT_ACTIVATED)
        {
            tutorialData.SelectionInputID = DATA_ID_INPUT<GDEInputCommandData>(tutorialData.SelectionInputID, "Selection Input ID", out var inputData);
        }

        //END_HOR();

        tutorialData.SelectionType = (SelectionTypes)DROP_DOWN(tutorialData.SelectionType, "Selection Type");

        BEGIN_HOR();
        LABEL("REQ SEGMENT:", ColorUtility.warningYellow);
        tutorialData.PreviousSegment = DATA_ID_INPUT<GDETutorialSegmentData>(tutorialData.PreviousSegment, "Segment ID", out var prevSegmentData);
        END_HOR();

        BEGIN_HOR();
        LABEL("NEXT SEGMENT:", ColorUtility.blueprintBlue);
        tutorialData.NextSegment = DATA_ID_INPUT<GDETutorialSegmentData>(tutorialData.NextSegment, "Segment ID", out var nextSegmentData);
        END_HOR();


        LABEL("Messages:", ColorUtility.voidPurple);
        BEGIN_INDENT();

        for (int i = 0; tutorialData.Message != null && i < tutorialData.Message.Length; i++)
        {
            BEGIN_HOR();

            TutorialMessage[] arr = ARRAY_UP_BTN<TutorialMessage>(tutorialData.Message, i);
            arr = ARRAY_DOWN_BTN<TutorialMessage>(arr, i);
            if (TRY_ARRAY_REMOVE_BTN<TutorialMessage>(arr, i, out var newArr))
            {
                tutorialData.Message = newArr;
                END_HOR();
                continue;
            }

            END_HOR();

            BEGIN_INDENT();

            TutorialMessage msg = arr[i];

            msg.OnlyShowInputHotkey = TOGGLE(msg.OnlyShowInputHotkey, "Only Show Input Hotkey");
            msg.InputID = DATA_ID_INPUT<GDEInputCommandData>(msg.InputID, "Input ID", out var inputCommandData);
            msg.TagObjectID = DATA_ID_INPUT<Scriptable>(msg.TagObjectID, "Tag Object ID", out var tagObjectData);
            msg.Body = TEXT_INPUT(msg.Body, "Body");

            BEGIN_HOR();
            msg.UseOverrideColor = TOGGLE(msg.UseOverrideColor, "Use Override Color");
            if (msg.UseOverrideColor)
            {
                msg.OverrideColor = COLOR_INPUT(msg.OverrideColor);
            }
            END_HOR();
            msg.NewLine = TOGGLE(msg.NewLine, "New Line");
            END_INDENT();

            arr[i] = msg;
            tutorialData.Message = arr;
        }

        tutorialData.Message = ARRAY_ADD_BTN<TutorialMessage>(tutorialData.Message, "+Add Tutorial Message");

        END_INDENT();
        END_INDENT();
    }

    private void RenderPrefabLocationLayerBtn(string tagObjID, int count, System.Action<string> onClick)
    {
        count = Mathf.Max(1, count);
        Color c = Color.gray;
        Sprite s = null;

        if (!string.IsNullOrEmpty(tagObjID))
        {
            Scriptable tagObj = GetDataByID(tagObjID);

            if (tagObj != null)
            {
                c = tagObj.TooltipTextColor;
                s = tagObj.TooltipIconSprite;
            }

            if (!string.IsNullOrEmpty(_copiedText) && _copiedText != tagObjID)
            {
                if (string.IsNullOrEmpty(tagObjID) || tagObjID == "+")
                {
                    c = Color.green;
                }
                else
                {
                    c = Color.red;
                }
            }
        }

        if (!string.IsNullOrEmpty(_copiedText) && !ScriptExists(_copiedText))
        {
            c = Color.yellow;
        }

        BEGIN_CLR(c);

        // Create a new GUIStyle for the button
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

        // Adjust the padding to offset the text inside the button
        if (s != null)
        {
            buttonStyle.padding = new RectOffset(44, 2, 2, 2); // Adjust these values as needed
        }

        buttonStyle.wordWrap = true;
        string label = tagObjID;

        if (tagObjID != "+" && !string.IsNullOrEmpty(tagObjID) && _paintingPrefab && tagObjID != _copiedText)
        {
            label = "X " + tagObjID;
        }

        if (GUILayout.Button(label, buttonStyle, GUILayout.Width(60 * 3), GUILayout.Height(((60 * 3) - ((count * 2))) / count), GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false)))
        {
            //string output = tagObjID;

            //if (_paintingPrefab)
            //{
            //    tagObjID = _prefabPaintID;
            //}
            //else
            //{
            //    _prefabPaintID = tagObjID;
            //}

            onClick?.Invoke("");
        }

        END_CLR();

        if (s != null)
        {
            Rect pos = GUILayoutUtility.GetLastRect();
            Texture2D texture = s.texture;

            Rect rect = new Rect(pos.x + 8, pos.y + 8, s.rect.width * 2, s.rect.height * 2);
            EditorGUI.DrawRect(new Rect(pos.x + 8, pos.y + 8, rect.width, rect.height), new Color(0.1f, 0.1f, 0.1f, 1f));
            GUI.DrawTexture(rect, texture);
        }
    }

    // Party.
    private void RenderPartyData(Scriptable script, GDEPartyData partyData)
    {
        if (partyData == null) { return; }

        MARK_DIRTY(partyData);
        BEGIN_INDENT();

        LABEL("Member Tuning");

        partyData.RandomizeMembers = TOGGLE(partyData.RandomizeMembers, "Randomize Members");

        BEGIN_INDENT();

        for (int i = 0; i < partyData.MemberTunings.Count; i++)
        {
            BEGIN_HOR();

            if (TRY_LIST_REMOVE_BTN<GDEPartyData.MemberTuning>(partyData.MemberTunings, i, out var newList))
            {
                partyData.MemberTunings = newList;
                END_HOR();
                continue;
            }

            END_HOR();
            partyData.MemberTunings[i] = RenderMemberData(script, partyData.MemberTunings[i]);

        }

        partyData.MemberTunings = LIST_ADD_BTN<GDEPartyData.MemberTuning>(partyData.MemberTunings, "+Additional Member Tuning");
        END_INDENT();
        END_INDENT();
    }

    private GDEPartyData.MemberTuning RenderMemberData(Scriptable script, GDEPartyData.MemberTuning memberData)
    {
        memberData.EntityID = DATA_ID_INPUT<GDEEntitiesData>(memberData.EntityID, "Entity ID", out var entityData);
        memberData.TuningID = DATA_ID_INPUT<GDEEntityTuningData>(memberData.TuningID, "Entity Tuning", out var additionalMemberTuning);

        if (additionalMemberTuning != null)
        {
            RenderEntityTuningData(script, additionalMemberTuning);
        }

        return memberData;
    }

    // Tuning data.
    private void RenderEntityTuningData(Scriptable script, GDEEntityTuningData tuningData)
    {
        if (tuningData == null) { return; }

        MARK_DIRTY(tuningData);
        BEGIN_INDENT();

        tuningData.AutoGenerateInventory = TOGGLE(tuningData.AutoGenerateInventory, "Auto Generate Inventory");
        tuningData.InventoryGroupID = DATA_ID_INPUT<GDETagObjectSpawnData>(tuningData.InventoryGroupID, "Inventory Group ID", out var inventorySpawnGroup);

        if (inventorySpawnGroup != null)
        {
            BEGIN_INDENT();
            RenderTagObjectSpawn(inventorySpawnGroup);
            END_INDENT();
        }

        tuningData.AutoGenerateEquipment = TOGGLE(tuningData.AutoGenerateEquipment, "Auto Generate Equipment (Fills in empty slots)");
        tuningData.EquipmentGroupID = DATA_ID_INPUT<GDETagObjectSpawnData>(tuningData.EquipmentGroupID, "Equipment Group ID", out var equipmentSpawnGroup);

        if (equipmentSpawnGroup != null)
        {
            BEGIN_INDENT();
            RenderTagObjectSpawn(equipmentSpawnGroup);
            END_INDENT();
        }

        END_INDENT();
    }

    // Attack.
    private void RenderAttack(Scriptable script, GDEAttacksData attackData)
    {
        if (attackData == null) { return; }

        MARK_DIRTY(attackData);

        attackData.SkillID = DATA_ID_INPUT<GDESkillsData>(attackData.SkillID, "Skill ID", out var skillData);
        attackData.TargetAttributeID = DATA_ID_INPUT<GDEAttributesData>(attackData.TargetAttributeID, "Target Attribute ID", out var targetAttributeData);
        attackData.SourceAttributeID = DATA_ID_INPUT<GDEAttributesData>(attackData.SourceAttributeID, "Source Attribute ID", out var sourceAttributeData);

        BEGIN_HOR();
        LABEL("Min/Max Amount");
        attackData.MinAmount = INT_INPUT(attackData.MinAmount, "");
        LABEL("-");
        attackData.MaxAmount = INT_INPUT(attackData.MaxAmount, "");
        END_HOR();
    }

    private string _audioSearch = "";

    // SFX Group.
    private void RenderSFXGroup(GDEAudioGroupsData sfxGroup)
    {
        if (sfxGroup == null) { return; }

        MARK_DIRTY(sfxGroup);
        BEGIN_INDENT();

        for (int i = 0; i < sfxGroup.SFX.Count; i++)
        {
            BEGIN_HOR();

            if (TRY_LIST_REMOVE_BTN<string>(sfxGroup.SFX, i, out var newList))
            {
                sfxGroup.SFX = newList;
                END_HOR();
                continue;
            }

            sfxGroup.SFX[i] = TEXT_INPUT(sfxGroup.SFX[i], "SFX");

            if (ClipExists(sfxGroup.SFX[i]))
            {
                BTN("Play", Color.green, () =>
                {
                    PlayClip(sfxGroup.SFX[i]);
                });
            }
            else
            {
                WARNING("Clip doesn't exists!");
            }

            END_HOR();

        }

        sfxGroup.SFX = LIST_ADD_BTN<string>(sfxGroup.SFX, "+SFX");

        BEGIN_INDENT();
        bool showSFXList = EXPAND_TOGGLE("All SFX List", ColorUtility.plantGreen, -1, sfxGroup.Key);

        if (showSFXList)
        {
            _audioSearch = TEXT_INPUT(_audioSearch, "SFX Search");

            UnityEngine.Object[] clips = LoadAllClips();

            for (int i = 0; clips != null && i < clips.Length; i++)
            {
                if (!clips[i].name.Contains("sfx")) { continue; }
                if (!string.IsNullOrEmpty(_audioSearch) && !clips[i].name.Contains(_audioSearch)) { continue; }

                BEGIN_HOR();
                BTN("+", Color.green, () =>
                {
                    if (!sfxGroup.SFX.Contains(clips[i].name))
                    {
                        sfxGroup.SFX.Add(clips[i].name);
                    }
                }, 32);
                BTN("Play", Color.green, () =>
                {
                    PlayClip(clips[i].name);
                }, 48);
                LABEL(clips[i].name, true);
                END_HOR();
            }
        }
        END_INDENT();
        END_INDENT();
    }

    // Attribute Data.
    private void RenderAttributeData(Scriptable script, GDEAttributesData attributeData)
    {
        if (attributeData == null) { return; }

        MARK_DIRTY(attributeData);

        LABEL("Attribute:", Color.grey);
        BEGIN_HOR();
        TAB(1);

        BEGIN_VERT();

        attributeData.StartMax = INT_INPUT(attributeData.StartMax, "Start Max");
        attributeData.StartMin = INT_INPUT(attributeData.StartMin, "Start Min");

        END_VERT();
        END_HOR();
    }

    // Statuses.
    private string[] RenderEntityStatuses(Scriptable script, string[] statuses)
    {
        if (statuses == null)
        {
            statuses = new string[] { };
        }

        LABEL("Statuses:", Color.grey);

        BEGIN_HOR();
        TAB(1);

        BEGIN_VERT();

        for (int n = 0; n < statuses.Length; n++)
        {
            BEGIN_HOR();
            statuses = ARRAY_REMOVE_BTN<string>(statuses, n);

            BEGIN_VERT();
            if (n < statuses.Length)
            {
                statuses[n] = DATA_ID_INPUT<GDEEntityStatusData>(statuses[n], "StatusID", out var statusData);
                //RenderEntityStatusData(script, statusData);
            }
            END_VERT();

            END_HOR();
        }

        statuses = ARRAY_ADD_BTN<string>(statuses, "+Status");
        END_VERT();
        END_HOR();

        DRAW_BACKGROUND();

        return statuses;
    }

    // Entity Status Data.
    private void RenderEntityStatusData(Scriptable script, GDEEntityStatusData statusData)
    {
        if (statusData == null) { return; }

        MARK_DIRTY(statusData);

        statusData.ExpireTimeMinutesMax = INT_INPUT(statusData.ExpireTimeMinutesMax, "Expire Time Max (Mins)");
        statusData.ExpireTimeMinutesMin = INT_INPUT(statusData.ExpireTimeMinutesMin, "Expire Time Min (Mins)");

        statusData.VisibleToPlayer = TOGGLE(statusData.VisibleToPlayer, "Show Icon On Toolbar");

        statusData.Notification = DATA_ID_INPUT<GDENotificationsData>(statusData.Notification, "Notification", out var notificationData);

        BEGIN_HOR();
        bool showActions = EXPAND_TOGGLE("Actions:", ColorUtility.plantGreen, -1, statusData.Key);
        LABEL($"({statusData.Actions.Length})");
        END_HOR();

        if (showActions)
        {
            BEGIN_VERT();

            for (int i = 0; i < statusData.Actions.Length; i++)
            {
                BEGIN_HOR();

                StatusAction[] arr = ARRAY_UP_BTN<StatusAction>(statusData.Actions, i);
                arr = ARRAY_DOWN_BTN<StatusAction>(arr, i);
                arr = ARRAY_REMOVE_BTN<StatusAction>(arr, i);

                if (i >= arr.Length)
                {
                    statusData.Actions = arr;
                    END_HOR();
                    continue;
                };

                StatusAction statusAction = arr[i];
                statusAction.ActivationID = DATA_ID_INPUT<GDETagsData>(statusAction.ActivationID, "Activation ID (Optional)", out var actionData);
                END_HOR();

                BEGIN_INDENT();
                statusAction.Comment = COMMENT(statusAction.Comment);
                BEGIN_HOR();
                BEGIN_VERT();

                statusAction.HideInTooltips = TOGGLE(statusAction.HideInTooltips, "Hide In Tooltips");

                BEGIN_HOR();
                BEGIN_CLR(Color.white, Color.grey, () => { return statusAction.TimeThresholdMinutes > 0; });
                statusAction.TimeThresholdMinutes = (uint)INT_INPUT((int)statusAction.TimeThresholdMinutes, "Time Activation Threshold (In Minutes). i.e., Every 60 minutes.");
                if (statusAction.TimeThresholdMinutes == 0)
                {
                    WARNING("Time Threshold is 0. This will only activate once on status activation.");
                }
                END_CLR();
                LABEL(((SimTime)statusAction.TimeThresholdMinutes).ToString(), ColorUtility.caveBlue);
                END_HOR();

                if (statusAction.TimeThresholdMinutes > 0)
                {
                    statusAction.RandomChance = (uint)INT_INPUT((int)statusAction.RandomChance, "Random Chance On Time Activate (1/100,000)");
                }

                // Condition Target.
                BEGIN_HOR();
                Scriptable target = GetDataByID(statusAction.ConditionTarget);
                BEGIN_CLR(Color.white, Color.grey, () => { return !string.IsNullOrEmpty(statusAction.ConditionTarget); });
                BEGIN_CLR(GUI.color, Color.red, () => { return string.IsNullOrEmpty(statusAction.ConditionTarget) || target != null; });
                statusAction.ConditionTarget = TEXT_INPUT(statusAction.ConditionTarget, "Condition Target");
                END_CLR();
                ASSERT_WARNING(string.IsNullOrEmpty(statusAction.ConditionTarget) || target != null, "Does not exists!");
                END_HOR();

                if (target != null)
                {
                    BEGIN_INDENT();
                    BEGIN_VERT();
                    BEGIN_HOR();
                    LABEL("Conditions:", ColorUtility.purple);
                    END_HOR();

                    for (int j = 0; statusAction.Conditions != null && j < statusAction.Conditions.Length; j++)
                    {
                        ICondition condition = statusAction.Conditions[j];
                        Scriptable conditionTarget = GetDataByID(statusAction.ConditionTarget);
                        BEGIN_HOR();
                        statusAction.Conditions = ARRAY_REMOVE_BTN<ICondition>(statusAction.Conditions, j);
                        condition = RenderCondition(condition, conditionTarget);
                        if (statusAction.Conditions.Length > j)
                        {
                            statusAction.Conditions[j] = condition;
                        }
                        END_HOR();
                    }

                    END_CLR();
                    BEGIN_HOR();
                    BEGIN_DISABLED(string.IsNullOrEmpty(statusAction.ConditionTarget));
                    BTN("+ [a <= b]", ColorUtility.purple, () =>
                    {
                        statusAction.Conditions = AddToArray<ICondition>(statusAction.Conditions, new LessThanEqualCondition());
                    });
                    BTN("+ [a < b]", ColorUtility.purple, () =>
                    {
                        statusAction.Conditions = AddToArray<ICondition>(statusAction.Conditions, new LessThanCondition());
                    });
                    BTN("+ [a >= b]", ColorUtility.purple, () =>
                    {
                        statusAction.Conditions = AddToArray<ICondition>(statusAction.Conditions, new GreaterThanEqualCondition());
                    });
                    BTN("+ [a > b]", ColorUtility.purple, () =>
                    {
                        statusAction.Conditions = AddToArray<ICondition>(statusAction.Conditions, new GreaterThanCondition());
                    });
                    BTN("+ [a == b]", ColorUtility.purple, () =>
                    {
                        statusAction.Conditions = AddToArray<ICondition>(statusAction.Conditions, new EqualCondition());
                    });
                    BTN("+ [a != b]", ColorUtility.purple, () =>
                    {
                        statusAction.Conditions = AddToArray<ICondition>(statusAction.Conditions, new NotEqualCondition());
                    });
                    END_DISABLED();
                    END_HOR();
                    END_VERT();
                    END_INDENT();
                }

                statusAction.ActivationAction = (StatusActionTypes)DROP_DOWN(statusAction.ActivationAction, "Activation Type");
                statusAction.DeactivationAction = (StatusActionTypes)DROP_DOWN(statusAction.DeactivationAction, "Deactivation Type");

                // Spawns.
                statusAction.SpawnID = DATA_ID_INPUT<Scriptable>(statusAction.SpawnID, "Spawn ID", out var spawnData);
                statusAction.SpawnMax = INT_INPUT(statusAction.SpawnMax, "Spawn Max");
                statusAction.SpawnMin = INT_INPUT(statusAction.SpawnMin, "Spawn Min");

                // Status Buffs.
                statusAction.Buffs = RenderBuffs(script, statusAction.Buffs);

                // Status Statuses.
                statusAction.Statuses = RenderEntityStatuses(script, statusAction.Statuses);

                END_VERT();
                END_HOR();

                arr[i] = statusAction;

                statusData.Actions = arr;

                END_INDENT();
            }

            statusData.Actions = ARRAY_ADD_BTN<StatusAction>(statusData.Actions, "+Status Action");
            END_VERT();
        }

        // Auto Jobs.
        BEGIN_HOR();
        bool showAutoJobs = EXPAND_TOGGLE($"Auto Jobs:", ColorUtility.plantGreen, -1, statusData.Key);
        LABEL($"({statusData.AutoJobs.Count})");
        END_HOR();

        if (showAutoJobs)
        {
            for (int i = 0; i < statusData.AutoJobs.Count; i++)
            {
                BEGIN_HOR();

                if (TRY_LIST_REMOVE_BTN<GDEEntityStatusData.StatusAutoJob>(statusData.AutoJobs, i, out var newList))
                {
                    statusData.AutoJobs = newList;
                    END_HOR();
                    continue;
                }

                BEGIN_INDENT();
                BEGIN_VERT();
                GDEBlueprintsData blueprintData = null;

                GDEEntityStatusData.StatusAutoJob autoJob = statusData.AutoJobs[i];

                autoJob.Settings = RenderAutoJobSettings(script, autoJob.Settings);
                autoJob.BlueprintID = DATA_ID_INPUT<GDEBlueprintsData>(autoJob.BlueprintID, "Blueprint ID", out blueprintData);

                statusData.AutoJobs[i] = autoJob;

                END_VERT();
                END_INDENT();
                END_HOR();
            }

            LIST_ADD_BTN<GDEEntityStatusData.StatusAutoJob>(statusData.AutoJobs, "+Auto Job");
        }
    }

    private void RenderLeaderData(Scriptable script, GDELeaderData leaderData)
    {
        if (leaderData == null) { return; }

        MARK_DIRTY(leaderData);

        leaderData.Statuses = RenderEntityStatuses(script, leaderData.Statuses);
    }

    private void RenderEntityData(Scriptable script, GDEEntitiesData entityData)
    {
        if (entityData == null) { return; }

        MARK_DIRTY(entityData);

        LABEL("Entity:", Color.grey);
        BEGIN_HOR();
        TAB(1);

        BEGIN_VERT();

        entityData.Race = DATA_ID_INPUT<GDERacesData>(entityData.Race, "Race", out var raceData);

        // Entity diet.
        bool showDiets = EXPAND_TOGGLE("Diets:", ColorUtility.purple, -1, entityData.Key);

        if (showDiets)
        {
            BEGIN_INDENT();
            for (int i = 0; i < entityData.Diets.Count; i++)
            {
                BEGIN_HOR();

                if (TRY_LIST_REMOVE_BTN<GDEEntitiesData.DietSettings>(entityData.Diets, i, out var newList))
                {
                    END_HOR();

                    entityData.Diets = newList;
                    continue;
                }
                BEGIN_INDENT();

                entityData.Diets[i] = RenderEntityDiet(script, entityData.Diets[i]);

                END_INDENT();
                END_HOR();
            }

            entityData.Diets = LIST_ADD_BTN<GDEEntitiesData.DietSettings>(entityData.Diets, "+Add Diet");

            END_INDENT();
        }

        // Entity biomes.
        bool showBiomes = EXPAND_TOGGLE("Biomes:", ColorUtility.plantGreen, -1, entityData.Key);

        if (showBiomes)
        {
            BEGIN_INDENT();
            List<Scriptable> biomes = GetDataByType<GDEBiomesData>();
            entityData.Biomes = TOGGLE_LIST<string>("", entityData.Biomes, biomes.Count,
                (int i) => { return biomes[i].Key; },
                (int i) => { return biomes[i].Key; });

            END_INDENT();
        }

        // Entity default statuses.
        bool showStatuses = EXPAND_TOGGLE("Default Statuses:", ColorUtility.plantGreen, -1, entityData.Key);

        if (showStatuses)
        {
            BEGIN_INDENT();
            for (int i = 0; i < entityData.Statuses.Count; i++)
            {
                BEGIN_HOR();
                List<string> l = LIST_REMOVE_BTN<string>(entityData.Statuses, i);
                GDEEntityStatusData statusData = null;

                if (i < l.Count)
                {
                    entityData.Statuses[i] = DATA_ID_INPUT<GDEEntityStatusData>(entityData.Statuses[i], "Status ID", out statusData);
                }

                END_HOR();
                entityData.Statuses = l;

                BEGIN_INDENT();
                RenderEntityStatusData(script, statusData);
                END_INDENT();
            }

            entityData.Statuses = LIST_ADD_BTN<string>(entityData.Statuses, "+Add Default Status");
            END_INDENT();
        }

        // Skill Permissions.
        bool showProhibitedSkills = EXPAND_TOGGLE("Skill Permissions:", Color.yellow, -1, entityData.Key);

        if (showProhibitedSkills)
        {
            BEGIN_INDENT();
            List<Scriptable> skills = GetDataByType<GDESkillsData>();

            BTN("Enable All", Color.green, () =>
            {
                for (int i = 0; i < skills.Count; i++)
                {

                    GDEEntitiesData.SkillPermission newPermission = new GDEEntitiesData.SkillPermission()
                    {
                        SkillID = skills[i].Key,
                        EnabledByDefault = true,
                        PlayerCanEdit = true
                    };

                    entityData.SetSkillPermission(newPermission);
                }
            });
            BTN("Disable All", Color.red, () =>
            {
                for (int i = 0; i < skills.Count; i++)
                {

                    GDEEntitiesData.SkillPermission newPermission = new GDEEntitiesData.SkillPermission()
                    {
                        SkillID = skills[i].Key,
                        EnabledByDefault = false,
                        PlayerCanEdit = false
                    };

                    entityData.SetSkillPermission(newPermission);
                }
            });


            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i] is GDESkillsData skillData && !skillData.VisibleToPlayer)
                {
                    continue;
                }

                GDEEntitiesData.SkillPermission prevPermission = entityData.GetSkillPermissionByID(skills[i].Key);

                BEGIN_HOR();
                bool enabledByDefault = TOGGLE(prevPermission.EnabledByDefault, "Enabled By Default", "Disabled By Default", 128);
                bool playerCanEdit = TOGGLE(prevPermission.PlayerCanEdit, "Player Can Edit", "Player Cannot Edit", 128);

                GDEEntitiesData.SkillPermission newPermission = new GDEEntitiesData.SkillPermission()
                {
                    SkillID = skills[i].Key,
                    EnabledByDefault = enabledByDefault,
                    PlayerCanEdit = playerCanEdit
                };

                LABEL(skills[i].TooltipName, enabledByDefault ? Color.green : Color.grey);
                END_HOR();

                if (prevPermission.PlayerCanEdit != newPermission.PlayerCanEdit ||
                    prevPermission.EnabledByDefault != newPermission.EnabledByDefault)
                {
                    entityData.SetSkillPermission(newPermission);
                }
            }

            END_INDENT();
        }


        // Profession Permissions.
        bool showProhibitedProfessions = EXPAND_TOGGLE("Profession Permissions:", Color.yellow, -1, entityData.Key);

        if (showProhibitedProfessions)
        {
            BEGIN_INDENT();
            List<Scriptable> professions = GetDataByType<GDEProfessionData>();

            BTN("Enable All", Color.green, () =>
            {
                for (int i = 0; i < professions.Count; i++)
                {

                    GDEEntitiesData.ProfessionPermission newPermission = new GDEEntitiesData.ProfessionPermission()
                    {
                        ProfessionID = professions[i].Key,
                        CanAssign = true,
                        CanSpawnWith = true
                    };

                    entityData.SetProfessionPermission(newPermission);
                }
            });
            BTN("Disable All", Color.red, () =>
            {
                for (int i = 0; i < professions.Count; i++)
                {

                    GDEEntitiesData.ProfessionPermission newPermission = new GDEEntitiesData.ProfessionPermission()
                    {
                        ProfessionID = professions[i].Key,
                        CanAssign = false,
                        CanSpawnWith = false
                    };

                    entityData.SetProfessionPermission(newPermission);
                }
            });


            for (int i = 0; i < professions.Count; i++)
            {
                if (professions[i] is GDEProfessionData professionData && !professionData.VisibleToPlayer)
                {
                    continue;
                }

                GDEEntitiesData.ProfessionPermission prevPermission = entityData.GetProfessionPermissionByID(professions[i].Key);

                BEGIN_HOR();
                bool enabledByDefault = TOGGLE(prevPermission.CanAssign, "Can Assign", "Cannot Assign", 128);
                bool playerCanEdit = TOGGLE(prevPermission.CanSpawnWith, "Can Spawn With", "Cannot Spawn With", 128);

                GDEEntitiesData.ProfessionPermission newPermission = new GDEEntitiesData.ProfessionPermission()
                {
                    ProfessionID = professions[i].Key,
                    CanAssign = enabledByDefault,
                    CanSpawnWith = playerCanEdit
                };

                LABEL(professions[i].TooltipName, enabledByDefault ? Color.green : Color.grey);
                END_HOR();

                if (prevPermission.CanAssign != newPermission.CanAssign ||
                    prevPermission.CanSpawnWith != newPermission.CanSpawnWith)
                {
                    entityData.SetProfessionPermission(newPermission);
                }
            }

            END_INDENT();
        }

        END_VERT();
        END_HOR();
    }

    private GDEEntitiesData.DietSettings RenderEntityDiet(Scriptable script, GDEEntitiesData.DietSettings diet)
    {
        diet.TagObjectID = DATA_ID_INPUT<Scriptable>(diet.TagObjectID, "Diet ID", out var dietData);
        diet.Priority = INT_INPUT(diet.Priority, "Priority");
        diet.Buffs = RenderBuffs(script, diet.Buffs);
        diet.Statuses = RenderEntityStatuses(script, diet.Statuses);
        return diet;
    }

    private void RenderBlueprintData(Scriptable script, GDEBlueprintsData blueprintData)
    {
        if (blueprintData == null) { return; }

        MARK_DIRTY(blueprintData);

        blueprintData.ProgressMax = INT_INPUT(blueprintData.ProgressMax, "Progress Max", ColorUtility.blueprintBlue);
        blueprintData.SkillLevel = INT_INPUT(blueprintData.SkillLevel, "Skill Level", ColorUtility.blueprintBlue);
        blueprintData.AddBlockSkillToJobSkill = TOGGLE(blueprintData.AddBlockSkillToJobSkill, "Add Block Skill To Job Skill", ColorUtility.blueprintBlue);
        blueprintData.ResearchKey = DATA_ID_INPUT<GDEResearchData>(blueprintData.ResearchKey, "Research ID", out var researchData);
        blueprintData.CategoryID = DATA_ID_INPUT<GDEBlueprintCategoryData>(blueprintData.CategoryID, "Category ID", out var categoryData);

        LABEL("Audio", ColorUtility.purple);
        BEGIN_INDENT();
        blueprintData.WorkSFX = DATA_ID_INPUT<GDEAudioGroupsData>(blueprintData.WorkSFX, "Work SFX", out var workSFXData);
        blueprintData.OnFinishedSFX = DATA_ID_INPUT<GDEAudioGroupsData>(blueprintData.OnFinishedSFX, "On Finished SFX", out var finishedSFXData);
        END_INDENT();

        LABEL("Visuals", ColorUtility.purple);
        BEGIN_INDENT();

        blueprintData.WorkFX = DATA_ID_INPUT<GDEFXGroupsData>(blueprintData.WorkFX, "Work FX", out var workFXData);
        blueprintData.OnFinishedFX = DATA_ID_INPUT<GDEFXGroupsData>(blueprintData.OnFinishedFX, "On Finished FX", out var onFinishedFXData);

        for (int i = 0; i < blueprintData.Visuals.Count; i++)
        {
            BEGIN_HOR();

            if (TRY_LIST_REMOVE_BTN<string>(blueprintData.Visuals, i, out var newList))
            {
                END_HOR();

                blueprintData.Visuals = newList;
                continue;
            }
            BEGIN_INDENT();

            blueprintData.Visuals[i] = DATA_ID_INPUT<GDEBlockVisualsData>(blueprintData.Visuals[i], "Visual ID", out var visualsData);

            END_INDENT();
            END_HOR();
        }

        blueprintData.Visuals = LIST_ADD_BTN<string>(blueprintData.Visuals, "+Job Visuals");
        END_INDENT();

        LABEL("Location Requirements:", ColorUtility.blueprintBlue);
        BEGIN_INDENT();
        blueprintData.LocationID = DATA_ID_INPUT<Scriptable>(blueprintData.LocationID, "Location ID", out var locationData);
        blueprintData.PermissionType = (BlockPermissionTypes)DROP_DOWN(blueprintData.PermissionType, "Location Permissions");
        END_INDENT();

        LABEL("Resource Restrictions:", ColorUtility.blueprintBlue);
        BEGIN_INDENT();
        blueprintData.ShowItemTypeResourcePermissions = TOGGLE(blueprintData.ShowItemTypeResourcePermissions, "Show Item Type Resource Restrictions");
        END_INDENT();


        LABEL("Create:", ColorUtility.green);
        BEGIN_INDENT();
        blueprintData.SpawnCount = INT_INPUT(blueprintData.SpawnCount, "Spawn Count");
        blueprintData.SpawnTagID = DATA_ID_INPUT<Scriptable>(blueprintData.SpawnTagID, "Spawn Tag ID", out var spawnTagData);
        blueprintData.SpawnTagObjectID = DATA_ID_INPUT<Scriptable>(blueprintData.SpawnTagObjectID, "Spawn Tag Object ID", out var spawnTagObjData);
        END_INDENT();

        LABEL("Trigger:", ColorUtility.green);
        BEGIN_INDENT();
        blueprintData.TriggerActivationID = TEXT_INPUT(blueprintData.TriggerActivationID, "Trigger Activation ID");
        END_INDENT();

        // On Finish resource action.
        LABEL("On Finish Resource:", ColorUtility.green);
        BEGIN_INDENT();
        blueprintData.OnFinishedResourcesActionID = DATA_ID_INPUT<Scriptable>(blueprintData.OnFinishedResourcesActionID, "On Finished Resources Action ID", out var onFinishResourceActionTagObjData);
        END_INDENT();

        // Worker Buffs.
        LABEL("On Finish Worker Buffs:", ColorUtility.green);
        BEGIN_INDENT();
        blueprintData.OnFinishWorkerBuffs = RenderBuffs(script, blueprintData.OnFinishWorkerBuffs);
        END_INDENT();

        // Worker Status Add.
        LABEL("On Finish Worker Statuses Added:", ColorUtility.green);
        BEGIN_INDENT();
        blueprintData.OnFinishWorkerStatusAdds = RenderEntityStatuses(script, blueprintData.OnFinishWorkerStatusAdds);
        END_INDENT();

        // Worker Status Remove.
        LABEL("On Finish Worker Statuses Removed:", ColorUtility.green);
        BEGIN_INDENT();
        blueprintData.OnFinishWorkerStatusRemoves = RenderEntityStatuses(script, blueprintData.OnFinishWorkerStatusRemoves);
        END_INDENT();

        // Target Buffs.
        LABEL("On Finish Target Buffs:", ColorUtility.green);
        BEGIN_INDENT();
        blueprintData.OnFinishTargetBuffs = RenderBuffs(script, blueprintData.OnFinishTargetBuffs);
        END_INDENT();

        // Target Status Add.
        LABEL("On Finish Target Statuses Added:", ColorUtility.green);
        BEGIN_INDENT();
        blueprintData.OnFinishTargetStatusAdds = RenderEntityStatuses(script, blueprintData.OnFinishTargetStatusAdds);
        END_INDENT();

        // Target Status Remove.
        LABEL("On Finish Target Statuses Removed:", ColorUtility.green);
        BEGIN_INDENT();
        blueprintData.OnFinishTargetStatusRemoves = RenderEntityStatuses(script, blueprintData.OnFinishTargetStatusRemoves);
        END_INDENT();

        BEGIN_HOR();
        LABEL("Actions", ColorUtility.selectedGold);
        LABEL(blueprintData.JobActions != null ? blueprintData.JobActions.Length : 0);
        END_HOR();
        BEGIN_INDENT();

        for (int i = 0; i < blueprintData.JobActions.Length; i++)
        {
            BEGIN_HOR();

            if (TRY_ARRAY_REMOVE_BTN<JobActionRequirement>(blueprintData.JobActions, i, out var newArr))
            {
                END_HOR();

                blueprintData.JobActions = newArr;
                continue;
            }

            if (TRY_ARRAY_DUPLICATE_BTN<JobActionRequirement>(blueprintData.JobActions, i, out newArr))
            {
                END_HOR();

                blueprintData.JobActions = newArr;
                continue;
            }

            LABEL("Action", ColorUtility.selectedGold);
            END_HOR();

            JobActionRequirement req = blueprintData.JobActions[i];

            BEGIN_INDENT();

            //Action Type.
            req.ActionType = (JobActionTypes)DROP_DOWN(req.ActionType, "Action Type");

            // Worker.
            req.WorkerID = DATA_ID_INPUT<Scriptable>(req.WorkerID, "Worker ID (i.e., \"entity_status_hungry\")", out var workerData);

            // Target.
            req.TargetID = DATA_ID_INPUT<Scriptable>(req.TargetID, "Target ID (i.e., \"entity_rat\")", out var targetData);

            // Resource.
            req.ResourceID = DATA_ID_INPUT<Scriptable>(req.ResourceID, "Resource ID (i.e., \"item_wood_log\" or \"tag_item_type_wood\")", out var resourceData);

            //// On Start action.
            //req.OnJobStartActionID = DATA_ID_INPUT<GDETagsData>(req.OnJobStartActionID, "On Start Action (i.e., \"tag_can_eat\")", out var onStartData);

            //// On Quit action.
            //req.OnJobQuitActionID = DATA_ID_INPUT<GDETagsData>(req.OnJobQuitActionID, "On Quit Action (i.e., \"tag_can_eat\")", out var onQuitData);

            // On Finish action.
            req.OnJobFinishActionID = DATA_ID_INPUT<GDETagsData>(req.OnJobFinishActionID, "On Finish Action (i.e., \"tag_can_eat\")", out var onFinishData);

            // Animations.
            req.AnimationTrigger = (EntityAnimationTriggers)DROP_DOWN(req.AnimationTrigger, "Anim Trigger");
            req.AnimationHitEvent = (EntityAnimationEvents)DROP_DOWN(req.AnimationHitEvent, "Anim Hit Event");
            req.AnimStartFX = DATA_ID_INPUT<GDEFXGroupsData>(req.AnimStartFX, "Anim Start FX", out var animStartFXData);
            req.AnimStartSFX = DATA_ID_INPUT<GDEAudioGroupsData>(req.AnimStartSFX, "Anim Start SFX", out var animStartSFXData);

            // Position.
            req.WorkPosition = (WorkPositionTypes)DROP_DOWN(req.WorkPosition, "Work Position");

            END_INDENT();

            blueprintData.JobActions[i] = req;
        }

        blueprintData.JobActions = ARRAY_ADD_BTN<JobActionRequirement>(blueprintData.JobActions, "+Job Action");
        END_INDENT(ColorUtility.selectedGold);
    }

    private void RenderRoomData(Scriptable script, GDERoomTemplatesData roomData)
    {
        if (roomData == null) { return; }

        MARK_DIRTY(roomData);

        roomData.CategoryID = DATA_ID_INPUT<GDERoomCategoryData>(roomData.CategoryID, "Category ID", out var categoryData);
        LABEL("Occupant Groups:", ColorUtility.blueprintBlue);
        BEGIN_INDENT();

        for (int i = 0; i < roomData.OccupantGroups.Count; i++)
        {
            BEGIN_HOR();
            if (TRY_LIST_REMOVE_BTN<string>(roomData.OccupantGroups, i, out var newList))
            {
                roomData.OccupantGroups = newList;
                END_HOR();
                continue;
            }

            roomData.OccupantGroups[i] = DATA_ID_INPUT<GDEOccupantGroupData>(roomData.OccupantGroups[i], "Occupant Group ID", out var occupantGroupData);

            END_HOR();

            BEGIN_INDENT();
            RenderOccupantGrouptData(script, occupantGroupData);
            END_INDENT();
        }

        roomData.OccupantGroups = LIST_ADD_BTN<string>(roomData.OccupantGroups, "+Occupant Group");

        END_INDENT();

        LABEL("Default Auto Jobs:", ColorUtility.blueprintBlue);
        BEGIN_INDENT();

        for (int i = 0; i < roomData.DefaultAutoJobs.Count; i++)
        {
            BEGIN_HOR();
            if (TRY_LIST_REMOVE_BTN<GDERoomTemplatesData.RoomAutoJob>(roomData.DefaultAutoJobs, i, out var newList))
            {
                roomData.DefaultAutoJobs = newList;
                END_HOR();
                continue;
            }

            LABEL("Auto Job");
            END_HOR();

            GDERoomTemplatesData.RoomAutoJob roomAutoJob = roomData.DefaultAutoJobs[i];

            BEGIN_INDENT();
            roomAutoJob.BlueprintID = DATA_ID_INPUT<GDEBlueprintsData>(roomAutoJob.BlueprintID, "Blueprint ID", out var blueprintData);

            BEGIN_HOR();
            BEGIN_SPRITE(blueprintData == null ? null : blueprintData.TooltipIconSprite);
            SPACE(4);
            BEGIN_VERT();
            roomAutoJob.Settings = RenderAutoJobSettings(script, roomAutoJob.Settings);
            END_VERT();
            END_HOR();
            roomData.DefaultAutoJobs[i] = roomAutoJob;
            END_INDENT();
        }

        roomData.DefaultAutoJobs = LIST_ADD_BTN<GDERoomTemplatesData.RoomAutoJob>(roomData.DefaultAutoJobs, "+Default Auto Jobs");

        bool showBlueprints = EXPAND_TOGGLE("Blueprints", ColorUtility.blueprintBlue, -1, roomData.Key);

        if (showBlueprints)
        {
            BEGIN_INDENT();

            List<Scriptable> blueprints = GetDataByType<GDEBlueprintsData>();

            for (int i = 0; i < blueprints.Count; i++)
            {
                if (blueprints[i] is GDEBlueprintsData blueprint && blueprint.CategoryID == "blueprint_category_none")
                {
                    BEGIN_HOR();
                    BEGIN_SPRITE(blueprint.TooltipIconSprite);
                    BTN(blueprint.TooltipName, Color.green, () =>
                    {
                        GDERoomTemplatesData.RoomAutoJob autoJob = new GDERoomTemplatesData.RoomAutoJob()
                        {
                            BlueprintID = blueprint.Key,
                            Settings = new AutoJobSettings()
                            {
                                AutoJobType = AutoJobTypes.AUTO,
                                Priority = 1,
                            }
                        };

                        roomData.DefaultAutoJobs.Add(autoJob);
                    });
                    END_HOR();
                }
            }

            END_INDENT();
        }

        END_INDENT();
    }

    private void RenderOccupantGrouptData(Scriptable script, GDEOccupantGroupData occupantGroupData)
    {
        if (occupantGroupData == null) { return; }

        MARK_DIRTY(occupantGroupData);

        occupantGroupData.OccupantsMustBePlayerControlled = TOGGLE(occupantGroupData.OccupantsMustBePlayerControlled, "Occupants Must Be Player Controlled");

        for (int i = 0; i < occupantGroupData.RequiredOccupantTagObjs.Length; i++)
        {
            BEGIN_HOR();
            if (TRY_ARRAY_REMOVE_BTN<TagObjectSetting>(occupantGroupData.RequiredOccupantTagObjs, i, out var newArr))
            {
                occupantGroupData.RequiredOccupantTagObjs = newArr;
                END_HOR();
                continue;
            }

            occupantGroupData.RequiredOccupantTagObjs[i].TagObjectKey = DATA_ID_INPUT<Scriptable>(occupantGroupData.RequiredOccupantTagObjs[i].TagObjectKey, "Tag Obj ID", out var tagObjData);
            END_HOR();
        }

        occupantGroupData.RequiredOccupantTagObjs = ARRAY_ADD_BTN<TagObjectSetting>(occupantGroupData.RequiredOccupantTagObjs, "+Default Occupant Tag Object");
    }

    private void RenderPlantData(Scriptable script, GDEBlockPlantsData plantData)
    {
        if (plantData == null) { return; }

        MARK_DIRTY(plantData);

        plantData.Permissions = (BlockPermissionTypes)DROP_DOWN_MASK(plantData.Permissions, "Block Permissions");
        plantData.Prohibited = (BlockPermissionTypes)DROP_DOWN_MASK(plantData.Prohibited, "Block Prohibitions");
        plantData.MaturePermissions = (BlockPermissionTypes)DROP_DOWN_MASK(plantData.MaturePermissions, "Mature Block Permissions");
        plantData.MatureProhibitions = (BlockPermissionTypes)DROP_DOWN_MASK(plantData.MatureProhibitions, "Mature Block Prohibitions");
        plantData.MatureTag = DATA_ID_INPUT<GDETagsData>(plantData.MatureTag, "Mature Tag", out var matureTagData);

        bool showSeasons = EXPAND_TOGGLE("Seasons:", ColorUtility.blueprintBlue, -1, plantData.Key);

        if (showSeasons)
        {
            BEGIN_INDENT();

            List<Scriptable> seasons = GetDataByType<GDEBiomeSeasonsData>();

            for (int i = 0; i < seasons.Count; i++)
            {
                if (seasons[i] is GDEBiomeSeasonsData seasonData)
                {
                    bool hasSeason = plantData.Seasons.Contains(seasonData.SeasonGroup);
                    BEGIN_CLR(hasSeason ? Color.green : Color.grey);
                    BTN($"{seasonData.SeasonGroup} - {seasons[i].Key}", () =>
                    {
                        if (hasSeason)
                        {
                            plantData.Seasons.Remove(seasonData.SeasonGroup);
                        }
                        else
                        {
                            plantData.Seasons.Add(seasonData.SeasonGroup);
                        }
                    });
                    END_CLR();
                }
            }

            END_INDENT();
        }

        bool showActions = EXPAND_TOGGLE("Actions:", ColorUtility.plantGreen, -1, plantData.Key);

        if (showActions)
        {
            BEGIN_INDENT();

            for (int i = 0; i < plantData.Actions.Length; i++)
            {
                BEGIN_HOR();
                if (TRY_ARRAY_REMOVE_BTN<PlantAction>(plantData.Actions, i, out var newArr))
                {
                    plantData.Actions = newArr;
                    END_HOR();
                    continue;
                }

                LABEL("Action");
                END_HOR();

                BEGIN_INDENT();

                plantData.Actions[i].ActionID = DATA_ID_INPUT<GDETagsData>(plantData.Actions[i].ActionID, "Action ID", out var actionTagData);

                if (actionTagData == null)
                {
                    WARNING("Action ID must be a valid tag!");
                }

                END_INDENT();
            }

            plantData.Actions = ARRAY_ADD_BTN<PlantAction>(plantData.Actions, "+Plant Action");
            END_INDENT();
        }
    }

    private void RenderBlock(Scriptable script, GDEBlocksData blockData)
    {
        if (blockData == null) { return; }

        blockData.SkillLevel = INT_INPUT(blockData.SkillLevel, "Skill Level", ColorUtility.blueprintBlue);

        LABEL("Trigger Conditions:");
        BEGIN_INDENT();

        for (int i = 0; blockData.TriggerConditions != null && i < blockData.TriggerConditions.Length; i++)
        {
            BEGIN_HOR();
            if (TRY_ARRAY_REMOVE_BTN<GDEBlocksData.TriggerCondition>(blockData.TriggerConditions, i, out var newArr))
            {
                blockData.TriggerConditions = newArr;
                END_HOR();
                continue;
            }

            LABEL("Condition");
            END_HOR();

            BEGIN_INDENT();

            BEGIN_CLR(Color.red, Color.white, () =>
            {
                return string.IsNullOrEmpty(blockData.TriggerConditions[i].ActivateTriggerID) &&
                                                             string.IsNullOrEmpty(blockData.TriggerConditions[i].DeactivateTriggerID);
            });

            blockData.TriggerConditions[i].TriggerType = (TriggerTypes)DROP_DOWN(blockData.TriggerConditions[i].TriggerType, "Trigger Type");
            blockData.TriggerConditions[i].ActivateTriggerID = DATA_ID_INPUT<GDETagsData>(blockData.TriggerConditions[i].ActivateTriggerID, "Trigger To Activate", out var activateTriggerData);
            blockData.TriggerConditions[i].DeactivateTriggerID = DATA_ID_INPUT<GDETagsData>(blockData.TriggerConditions[i].DeactivateTriggerID, "Trigger To Deactivate", out var deactivateTriggerData);
            blockData.TriggerConditions[i].HideInTooltips = TOGGLE(blockData.TriggerConditions[i].HideInTooltips, "Hide In Tooltips");
            blockData.TriggerConditions[i].Invert = TOGGLE(blockData.TriggerConditions[i].Invert, "Pass If Requirements Not Met");
            blockData.TriggerConditions[i].SourceTagObjectRequirement0 = DATA_ID_INPUT<Scriptable>(blockData.TriggerConditions[i].SourceTagObjectRequirement0, "Source Tag Obj Requirement 0", out var sourceTagObjectRequirement0Data);
            blockData.TriggerConditions[i].SourceTagObjectRequirement1 = DATA_ID_INPUT<Scriptable>(blockData.TriggerConditions[i].SourceTagObjectRequirement1, "Source Tag Obj Requirement 1", out var sourceTagObjectRequirement1Data);
            blockData.TriggerConditions[i].ElapsedTime = (uint)INT_INPUT((int)blockData.TriggerConditions[i].ElapsedTime, "Elapsed Time (Mins)");

            END_INDENT();
        }

        blockData.TriggerConditions = ARRAY_ADD_BTN<GDEBlocksData.TriggerCondition>(blockData.TriggerConditions, "+Trigger Condition");
        END_INDENT();

        LABEL("Triggers:");
        BEGIN_INDENT();

        for (int i = 0; blockData.Triggers != null && i < blockData.Triggers.Length; i++)
        {
            BEGIN_HOR();
            if (TRY_ARRAY_REMOVE_BTN<GDEBlocksData.Trigger>(blockData.Triggers, i, out var newArr))
            {
                blockData.Triggers = newArr;
                END_HOR();
                continue;
            }

            LABEL("Trigger");
            END_HOR();

            BEGIN_INDENT();

            blockData.Triggers[i].Comment = COMMENT(blockData.Triggers[i].Comment);
            blockData.Triggers[i].TooltipID = DATA_ID_INPUT<GDETooltipsData>(blockData.Triggers[i].TooltipID, "Tooltip ID", out var tooltipData);

            if (tooltipData != null)
            {
                BEGIN_INDENT();
                RenderTooltip(tooltipData);
                END_INDENT();
            }

            blockData.Triggers[i].ID = DATA_ID_INPUT<GDETagsData>(blockData.Triggers[i].ID, "Trigger ID", out var triggerTagData);

            if (triggerTagData == null)
            {
                WARNING("Trigger ID must be a valid tag!");
            }

            blockData.Triggers[i].Permissions = (BlockPermissionTypes)DROP_DOWN_MASK(blockData.Triggers[i].Permissions, "Permissions");
            blockData.Triggers[i].Action = (TriggerActionTypes)DROP_DOWN(blockData.Triggers[i].Action, "Action");
            blockData.Triggers[i].ActionTagObjectID = DATA_ID_INPUT<Scriptable>(blockData.Triggers[i].ActionTagObjectID, "Action Tag Obj ID", out var actionTagObjData);
            blockData.Triggers[i].DeactivateImmediate = TOGGLE(blockData.Triggers[i].DeactivateImmediate, "Deactivate Immediate");

            LABEL("Trigger Visuals:");
            BEGIN_INDENT();
            blockData.Triggers[i].Visuals = DATA_ID_INPUT<GDEBlockVisualsData>(blockData.Triggers[i].Visuals, "Visuals ID", out var visualsData);
            blockData.Triggers[i].SFX = DATA_ID_INPUT<GDEAudioGroupsData>(blockData.Triggers[i].SFX, "SFX ID", out var audioGroupData);
            blockData.Triggers[i].FX = DATA_ID_INPUT<GDEFXGroupsData>(blockData.Triggers[i].FX, "FX ID", out var fxGroupData);
            END_INDENT();

            if (blockData.Triggers[i].Action == TriggerActionTypes.ADD_BUFF)
            {
                LABEL("Buff:");
                blockData.Triggers[i].Buff = RenderBuffData(script, blockData.Triggers[i].Buff);
            }

            END_INDENT();
        }

        blockData.Triggers = ARRAY_ADD_BTN<GDEBlocksData.Trigger>(blockData.Triggers, "+Trigger");
        END_INDENT();
    }

    private void RenderBlockVisualsData(Scriptable script, GDEBlockVisualsData visualsData)
    {
        if (visualsData == null) { return; }

        visualsData.TransitionType = (BlockTransitionTypes)DROP_DOWN(visualsData.TransitionType, "Transition Type");

        if (visualsData.TransitionType != BlockTransitionTypes.NONE)
        {
            BEGIN_HOR();
            visualsData.TransitionKey = INT_INPUT(visualsData.TransitionKey, "Transition Key");
            BTN("+", Color.green, () => { visualsData.TransitionKey++; }, 32);
            END_HOR();
            List<Scriptable> visualScripts = GetDataByType<GDEBlockVisualsData>();

            for (int i = 0; i < visualScripts.Count; i++)
            {
                if (visualScripts[i] is GDEBlockVisualsData otherVisual &&
                    otherVisual.Key != visualsData.Key &&
                    otherVisual.TransitionKey == visualsData.TransitionKey)
                {
                    WARNING($"Conflict: {otherVisual.Key}");
                }
            }
        }
    }

    private AutoJobSettings RenderAutoJobSettings(Scriptable script, AutoJobSettings settings)
    {
        MARK_DIRTY(script);

        settings.AutoJobType = (AutoJobTypes)DROP_DOWN(settings.AutoJobType, "Auto Job Type");
        settings.Enabled = TOGGLE(settings.Enabled, "Start Enabled In Room");
        settings.Priority = INT_INPUT(settings.Priority, "Priority");
        return settings;
    }

    private ICondition RenderCondition(ICondition condition, ITagObject target)
    {
        if (target == null)
        {
            LABEL("Target");
        }
        else
        {
            LABEL(target.Key);
        }

        if (condition is LessThanEqualCondition lessThanEqualCondition)
        {
            LABEL("<=", ColorUtility.purple);
            lessThanEqualCondition.Amount = FLOAT_INPUT(lessThanEqualCondition.Amount);
            lessThanEqualCondition.Normalized = TOGGLE(lessThanEqualCondition.Normalized, "Normalized (0.0 - 1.0)");
            return lessThanEqualCondition;
        }
        else if (condition is LessThanCondition lessThanCondition)
        {
            LABEL("<", ColorUtility.purple);
            lessThanCondition.Amount = FLOAT_INPUT(lessThanCondition.Amount);
            lessThanCondition.Normalized = TOGGLE(lessThanCondition.Normalized, "Normalized (0.0 - 1.0)");
            return lessThanCondition;
        }
        else if (condition is GreaterThanEqualCondition greaterThanEqualCondition)
        {
            LABEL(">=", ColorUtility.purple);
            greaterThanEqualCondition.Amount = FLOAT_INPUT(greaterThanEqualCondition.Amount);
            greaterThanEqualCondition.Normalized = TOGGLE(greaterThanEqualCondition.Normalized, "Normalized (0.0 - 1.0)");
            return greaterThanEqualCondition;
        }
        else if (condition is GreaterThanCondition greaterThanCondition)
        {
            LABEL(">", ColorUtility.purple);
            greaterThanCondition.Amount = FLOAT_INPUT(greaterThanCondition.Amount);
            greaterThanCondition.Normalized = TOGGLE(greaterThanCondition.Normalized, "Normalized (0.0 - 1.0)");
            return greaterThanCondition;
        }
        else if (condition is EqualCondition equal)
        {
            LABEL("==", ColorUtility.purple);
            equal.Amount = FLOAT_INPUT(equal.Amount);
            equal.Normalized = TOGGLE(equal.Normalized, "Normalized (0.0 - 1.0)");
            return equal;
        }
        else if (condition is NotEqualCondition notEqual)
        {
            LABEL("==", ColorUtility.purple);
            notEqual.Amount = FLOAT_INPUT(notEqual.Amount);
            notEqual.Normalized = TOGGLE(notEqual.Normalized, "Normalized (0.0 - 1.0)");
            return notEqual;
        }

        return condition;
    }

    private List<string> _scriptTypes = new List<string>()
    {
        typeof(GDEAmbientMusicData).Name,
        typeof(GDEAnimationsData).Name,
        typeof(GDEAnimationSetGroupsData).Name,
        typeof(GDEAnimationSetsData).Name,
        typeof(GDEAnimationStatesData).Name,
        typeof(GDEAppearanceGroupsData).Name,
        typeof(GDEAppearancesData).Name,
        typeof(GDEArtifactData).Name,
        typeof(GDEAttackGroupsData).Name,
        typeof(GDEAttacksData).Name,
        typeof(GDEAttributesData).Name,
        typeof(GDEAudioGroupsData).Name,
        typeof(GDEAutoJobEntityPoolData).Name,
        typeof(GDEBiomeNamesData).Name,
        typeof(GDEBiomeNoiseData).Name,
        typeof(GDEBiomesData).Name,
        typeof(GDEBiomeSeasonsData).Name,
        typeof(GDEBiomeTerrainGenData).Name,
        typeof(GDEBiomeTerrainNoiseTuningData).Name,
        typeof(GDEBiomeWeatherData).Name,
        typeof(GDEBlockFillData).Name,
        typeof(GDEBlockPlantsData).Name,
        typeof(GDEBlockPlatformsData).Name,
        typeof(GDEBlocksData).Name,
        typeof(GDEBlockTriggersData).Name,
        typeof(GDEBlockVisualsData).Name,
        typeof(GDEBlueprintCategoryData).Name,
        typeof(GDEBlueprintsData).Name,
        typeof(GDECaveData).Name,
        typeof(GDECharacterAccessoryData).Name,
        typeof(GDECharacterColorMaskData).Name,
        typeof(GDEControlGroupData).Name,
        typeof(GDEDesignationFilterData).Name,
        typeof(GDEDialogueData).Name,
        typeof(GDEDialogueGroupsData).Name,
        typeof(GDEDiscoveryGroupsData).Name,
        typeof(GDEDiscoveryNamesData).Name,
        typeof(GDEEntitiesData).Name,
        typeof(GDEEntityAgeData).Name,
        typeof(GDEEntityAgeTypeData).Name,
        typeof(GDEEntityNamesData).Name,
        typeof(GDEEntitySchema).Name,
        typeof(GDEEntitySizesData).Name,
        typeof(GDEEntitySpawnGroupsData).Name,
        typeof(GDEEntityStatusData).Name,
        typeof(GDEEntityTuningData).Name,
        typeof(GDEFactionData).Name,
        typeof(GDEFamilyMemberData).Name,
        typeof(GDEFishData).Name,
        typeof(GDEFishSpawnGroupsData).Name,
        typeof(GDEFXGroupsData).Name,
        typeof(GDEGameConstantsData).Name,
        typeof(GDEGameplayTipsData).Name,
        typeof(GDEHistoryData).Name,
        typeof(GDEHistoryTypesData).Name,
        typeof(GDEIndicationsData).Name,
        typeof(GDEInputCommandData).Name,
        typeof(GDEIntelligenceData).Name,
        typeof(GDEItemQualitiesData).Name,
        typeof(GDEItemsData).Name,
        typeof(GDEItemSlotsData).Name,
        typeof(GDELocationNamesData).Name,
        typeof(GDENotificationsData).Name,
        typeof(GDEOccupantGroupData).Name,
        typeof(GDEOverworldLodeSpawnGroupsData).Name,
        typeof(GDEOverworldLodeSpawnsData).Name,
        typeof(GDEOverworldMapGenData).Name,
        typeof(GDEOverworldNationData).Name,
        typeof(GDEOverworldVisualsData).Name,
        typeof(GDEProfessionData).Name,
        typeof(GDERacesData).Name,
        typeof(GDERealmNamesData).Name,
        typeof(GDEResearchCategoriesData).Name,
        typeof(GDEResearchData).Name,
        typeof(GDERoomCategoryData).Name,
        typeof(GDERoomNamesData).Name,
        typeof(GDERoomPermissionData).Name,
        typeof(GDERoomTemplatesData).Name,
        typeof(GDEScenariosData).Name,
        typeof(GDESelectionData).Name,
        typeof(GDESimulationData).Name,
        typeof(GDESkillsData).Name,
        typeof(GDEStartingLoadoutsData).Name,
        typeof(GDETagObjectSpawnData).Name,
        typeof(GDETagsData).Name,
        typeof(GDEToolbarMenusData).Name,
        typeof(GDETooltipsData).Name,
        typeof(GDETutorialSegmentData).Name,
        typeof(GDEUniformData).Name,
        typeof(GDEWordGroupsData).Name,
        typeof(GDEXPGrowthData).Name,
        typeof(GDEZonesData).Name,
    };

    [MenuItem("Tools/Print Scriptable.cs Types")]
    public static void FindScriptableInheritors()
    {
        // First, find the type of your Scriptable class
        System.Type scriptableType = typeof(Scriptable); // Replace 'Scriptable' with your actual class name

        // Then, gather all types in the assembly
        List<System.Type> inheritors = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(scriptableType))
            .ToList();

        List<string> typeNames = new List<string>();

        // Finally, print them to the Unity Console
        foreach (System.Type inheritor in inheritors)
        {
            //Debug.Log(inheritor.Name);
            typeNames.Add(inheritor.Name);
        }

        StringBuilder builder = new StringBuilder();

        builder.AppendLine("private List<string> _scriptTypes = new List<string>()");
        builder.AppendLine("{");

        foreach (string typeName in typeNames)
        {
            builder.AppendLine($"    typeof({typeName}).Name,");
        }

        builder.AppendLine("};");

        Debug.Log(builder.ToString());
    }
}
