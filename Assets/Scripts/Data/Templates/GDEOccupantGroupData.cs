using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.GameData;

[CreateAssetMenu(menuName = "ScriptableObjects/OccupantGroup")]
public class GDEOccupantGroupData : Scriptable
{
	public OccupantManagementTypes OccupantManageType = OccupantManagementTypes.AUTO;

    public bool OccupantsMustBePlayerControlled = true;
	
    [Header("Permissions:")]
    public TagObjectSetting[] AvailableOccupantTagObjs = new TagObjectSetting[] {
        new TagObjectSetting(){ TagObjectKey = "tag_races", Max = -1, Min = -1 },
        new TagObjectSetting(){ TagObjectKey = "tag_professions", Max = -1, Min = -1 },
        new TagObjectSetting(){ TagObjectKey = "tag_skills", Max = -1, Min = -1 },
        new TagObjectSetting(){ TagObjectKey = "tag_factions", Max = -1, Min = -1 },
        new TagObjectSetting(){ TagObjectKey = "tag_statuses", Max = -1, Min = -1 },
    };

    public TagObjectSetting[] RequiredOccupantTagObjs = new TagObjectSetting[] {
        new TagObjectSetting(){ TagObjectKey = "tag_races", Max = -1, Min = -1 },
        new TagObjectSetting(){ TagObjectKey = "tag_professions", Max = -1, Min = -1 },
        new TagObjectSetting(){ TagObjectKey = "tag_skills", Max = -1, Min = -1 },
        new TagObjectSetting(){ TagObjectKey = "tag_factions", Max = -1, Min = -1 },
        new TagObjectSetting(){ TagObjectKey = "tag_statuses", Max = -1, Min = -1 },
    };
}
