using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OccupantGroup")]
public class GDEOccupantGroupData : Scriptable
{
    public OccupantManagementTypes OccupantManageType = OccupantManagementTypes.AUTO;
    public bool OccupantsMustBePlayerControlled = true;

    [Header("Permissions:")]
    public TagObjectSetting[] RequiredOccupantTagObjs = new TagObjectSetting[] {
        new TagObjectSetting(){ TagObjectKey = "tag_races" },
        new TagObjectSetting(){ TagObjectKey = "tag_professions" },
        new TagObjectSetting(){ TagObjectKey = "tag_skills" },
        new TagObjectSetting(){ TagObjectKey = "tag_factions" },
        new TagObjectSetting(){ TagObjectKey = "tag_statuses" },
    };

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        base.OnLoaded();

#if UNITY_EDITOR
        for (int j = 0; RequiredOccupantTagObjs != null && j < RequiredOccupantTagObjs.Length; j++)
        {
            string tagObjectKey = RequiredOccupantTagObjs[j].TagObjectKey;
            string tagID = RequiredOccupantTagObjs[j].TagID;

            if (!string.IsNullOrEmpty(tagObjectKey))
            {
                ITagObject tagObj = DataManager.GetTagObject(tagObjectKey);

                if (tagObj is GDETagsData tagData)
                {
                    tagID = tagData.TagID;
                    tagObjectKey = string.Empty;
                }
                else if (tagObjectKey.Contains("skill_"))
                {
                    tagID = "tag_skills";
                }
                else if (tagObjectKey.Contains("profession_"))
                {
                    tagID = "tag_professions";
                }
                else if (tagObjectKey.Contains("race_"))
                {
                    tagID = "tag_races";
                }
                else if (tagObjectKey.Contains("faction"))
                {
                    tagID = "tag_factions";
                }
                else if (tagObjectKey.Contains("status"))
                {
                    tagID = "tag_statuses";
                }
                else if (string.IsNullOrEmpty(tagID) && tagObj.TagCount > 0)
                {
                    tagID = tagObj.GetTag(0).TagID;
                }

                if (string.IsNullOrEmpty(tagID))
                {
                    Debug.LogError($"Resource permission tag id is null or empty for {Key} and tag obj: {tagObjectKey}.");
                    continue;
                }

                if (RequiredOccupantTagObjs[j].TagID == tagID &&
                    RequiredOccupantTagObjs[j].TagObjectKey == tagObjectKey)
                {
                    continue;
                }

                RequiredOccupantTagObjs[j] = new TagObjectSetting()
                {
                    TagID = tagID,
                    TagObjectKey = tagObjectKey
                };

                Debug.LogError($"{Key} tag id: ({tagID}) tag obj: ({tagObjectKey})");

                UnityEditor.EditorUtility.SetDirty(this);
            }

        }
#endif
    }
#endif
}
