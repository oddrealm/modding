using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Research")]
public class GDEResearchData : Scriptable
{
    public bool Enabled = false;
    public bool ShowInSaga = true;
    public string Dependency = "";
    [Header("Column")]
    public int Column = 0;
    [Header("Row")]
    public int Row = 0;
    public string ResearchCategory = "";
    public int RequireItemCountOverride = 0;
    public List<GlobalBuff> GlobalBuffs = new List<GlobalBuff>();
    [System.NonSerialized]
    public List<string> Dependencies = new List<string>();
    [System.NonSerialized]
    public List<string> TagObjectTypesUnlocked = new List<string>();
    [System.NonSerialized]
    public Dictionary<string, List<string>> TagObjectsByTypeUnlocked = new Dictionary<string, List<string>>();
    [System.NonSerialized]
    public List<string> TagObjectsUnlocked = new List<string>();
    public string RequiredDiscovery = "";

    public int ResearchCost { get { return 1; } }

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        Dependencies.Clear();
        List<ITagObject> research = DataManager.GetTagObjects<GDEResearchData>();

        for (int i = 0; i < research.Count; i++)
        {
            if (research[i] is GDEResearchData researchData &&
                researchData.Key != Key &&
                researchData.Dependency == Key)
            {
                Dependencies.Add(research[i].Key);
            }
        }

        List<ITagObject> blueprints = DataManager.GetTagObjects<GDEBlueprintsData>();

        for (int i = 0; i < blueprints.Count; i++)
        {
            GDEBlueprintsData blueprint = blueprints[i] as GDEBlueprintsData;

            if (blueprint.ResearchKey != Key) { continue; }

            AddTagObjUnlock(blueprints[i]);
        }

        List<ITagObject> rooms = DataManager.GetTagObjects<GDERoomTemplatesData>();

        for (int i = 0; i < rooms.Count; i++)
        {
            GDERoomTemplatesData room = rooms[i] as GDERoomTemplatesData;

            if (room.ResearchKey != Key) { continue; }

            AddTagObjUnlock(rooms[i]);
        }

        base.OnLoaded();
    }

    public override void OnReordered(int dataIndex)
    {
        base.OnReordered(dataIndex);

        for (int i = 0; i < TagObjectTypesUnlocked.Count; i++)
        {
            string t = TagObjectTypesUnlocked[i];
            TagObjectsByTypeUnlocked[t] = TagObjectsByTypeUnlocked[t].OrderBy((string tagObjID) =>
            {
                return DataManager.GetTagObject(tagObjID).ObjectIndex;
            }).ToList();
        }

    }

    private void AddTagObjUnlock(ITagObject tagObj)
    {
        if (!TagObjectsByTypeUnlocked.TryGetValue(tagObj.ObjectType, out var objsByType))
        {
            objsByType = new List<string>();
            TagObjectsByTypeUnlocked.Add(tagObj.ObjectType, objsByType);
            TagObjectTypesUnlocked.Add(tagObj.ObjectType);
        }

        objsByType.Add(tagObj.Key);
        TagObjectsUnlocked.Add(tagObj.Key);
    }
#endif
}
