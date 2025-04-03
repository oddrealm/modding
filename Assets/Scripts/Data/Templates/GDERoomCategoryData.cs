using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Room/RoomCategory", order = 34)]
public class GDERoomCategoryData : Scriptable
{
    public bool ShowInSelectionList = true;
    [System.NonSerialized]
    public List<GDERoomTemplatesData> Rooms = new List<GDERoomTemplatesData>();

#if ODD_REALM_APP
    public override void OnReordered(int dataIndex)
    {
        Rooms.Clear();
        List<ITagObject> rooms = DataManager.GetTagObjects<GDERoomTemplatesData>();

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i] is GDERoomTemplatesData room && room.CategoryID == Key)
            {
                Rooms.Add(room);
            }
        }

        base.OnReordered(dataIndex);
    }
#endif
}
