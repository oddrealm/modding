using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tags")]
public class GDETagsData : Scriptable, ITag
{
    public bool TrackByDefault = false;
    public bool TrackPlayerCanEdit = false;
    public TrackingTypes TrackingTypes = TrackingTypes.ITEM;

    public TagUID TagUID { get; private set; }
    public string TagID { get { return Key; } }
    public string TagGroup { get { return GroupID; } }

    public override bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking()
        {
            TagID = TagID,
            TagObjectID = Key,
            HideIfZero = true,
            StartDisabled = !TrackByDefault,
            PlayerEditDisabled = !TrackPlayerCanEdit,
            TrackingType = TrackingTypes
        };

        return true;
    }

#if ODD_REALM_APP
    public override void Init()
    {
        TagUID = TagUID.Next();
        base.Init();
    }

    public override void OnLoaded()
    {
        if (TagIDs.Count > 0)
        {
            Debug.LogError(Key + " Cannot have tags!");
            TagIDs.Clear();
        }

        base.OnLoaded();
    }
#endif
}
