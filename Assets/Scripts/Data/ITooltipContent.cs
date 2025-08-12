using UnityEngine;

public interface ITooltipContent
{
    GDETooltipsData TooltipData { get; }
    string TooltipName { get; }
    string TooltipAction { get; }
    string TooltipNamePlural { get; }
    string TooltipInlineAndName { get; }
    string TooltipInlineIcon { get; }
    string TooltipIcon { get; }
    Sprite TooltipIconSprite { get; }
    Color TooltipTextColor { get; }
    Color TooltipTypeColor { get; }
    string TooltipType { get; }
    string TooltipDescription { get; }
    int TooltipOrder { get; }
}