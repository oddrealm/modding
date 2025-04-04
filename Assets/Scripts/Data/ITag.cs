
public interface ITag : ITooltipContent
{
    TagUID TagUID { get; }
    string TagID { get; }
    string[] GetTagGroups();
}
