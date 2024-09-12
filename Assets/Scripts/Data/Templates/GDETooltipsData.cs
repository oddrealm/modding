using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tooltips")]
public class GDETooltipsData : Scriptable
{
	public override GDETooltipsData TooltipData { get { return this; } }

	public new TooltipUID TooltipUID { get; private set; }
	public string Action = "";
	public string Name = "";
    public string NamePlural = "";
	public string InlineIcon = "";
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
        _inlineAndName = InlineIcon + " " + Name;

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
    public string Description = "";
	public string Icon = "";
	public Color TextColor = Color.white;
	public string Type = "";
	public Color TypeColor = Color.white;
	public int Order;

#if ODD_REALM_APP
    public override void OnReordered(int dataIndex)
    {
		Order = ObjectIndex;

        base.OnReordered(dataIndex);
    }

    public override void OnLoaded()
	{
		TooltipUID = TooltipUID.Next();

        base.OnLoaded();
	}
#endif
}