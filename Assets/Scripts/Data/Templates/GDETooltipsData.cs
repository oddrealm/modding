using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tooltips")]
public class GDETooltipsData : Scriptable
{
    public override GDETooltipsData TooltipData { get { return this; } }

    public new TooltipUID TooltipUID { get; private set; }
    public string Action = string.Empty;
    public string Name = string.Empty;
    public string NamePlural = string.Empty;
    public string InlineIcon = string.Empty;
    public string InlineAndName
    {
        get
        {
            if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(InlineIcon))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(Name))
            {
                return InlineIcon;
            }

            if (string.IsNullOrEmpty(InlineIcon))
            {
                return Name;
            }

            if (string.IsNullOrEmpty(_inlineAndName))
            {
                RebuildInlineAndName();

#if DEV_TESTING
                _prevInline = InlineIcon;
#endif
            }

#if DEV_TESTING
            if (_prevInline != InlineIcon)
            {
                RebuildInlineAndName();
            }
            else if (_prevName != Name)
            {
                RebuildInlineAndName();
            }
#endif

            return _inlineAndName;
        }
    }

    public void RebuildInlineAndName()
    {
        if (string.IsNullOrEmpty(InlineIcon) && string.IsNullOrEmpty(Name))
        {
            _inlineAndName = string.Empty;
        }
        else if (!string.IsNullOrEmpty(InlineIcon) && !string.IsNullOrEmpty(Name))
        {
            _inlineAndName = InlineIcon + " " + Name;
        }
        else
        {
            _inlineAndName = InlineIcon ?? Name;
        }

#if DEV_TESTING
        _prevInline = InlineIcon;
        _prevName = Name;
#endif
    }

    private string _inlineAndName;
#if DEV_TESTING
    private string _prevInline;
    private string _prevName;
#endif
    public string Description = string.Empty;
    public string DiscoveryHint = string.Empty;
    public string Icon = string.Empty;
    public Color TextColor = Color.white;
    public string Type = string.Empty;
    public Color TypeColor = Color.white;
    public int Order;

#if ODD_REALM_APP
    public override void OnReordered(int dataIndex)
    {
        Order = OrderIndex;

        base.OnReordered(dataIndex);
    }

    public override void OnLoaded()
    {
        TooltipUID = TooltipUID.Next();

        base.OnLoaded();
    }
#endif
}
