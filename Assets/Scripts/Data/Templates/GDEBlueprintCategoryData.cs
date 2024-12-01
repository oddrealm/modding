using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BlueprintCategory")]
public class GDEBlueprintCategoryData : Scriptable
{
    public SelectionTypes VisibleSelectionList = SelectionTypes.BUILD;
    public BlockVisibilityFlags VisibilityFlags = BlockVisibilityFlags.BLUEPRINTS;

    public Color JobNormalTxtColor;
    public Color JobNormalBtnColor;
    public Color JobSelectedTxtColor;
    public Color JobSelectedBtnColor;

    [System.NonSerialized]
    public List<GDEBlueprintsData> Blueprints = new List<GDEBlueprintsData>();

#if ODD_REALM_APP
    public override void OnReordered(int dataIndex)
    {
        Blueprints.Clear();
        List<ITagObject> blueprints = DataManager.GetTagObjects<GDEBlueprintsData>();

        for (int i = 0; i < blueprints.Count; i++)
        {
            if (blueprints[i] is GDEBlueprintsData blueprint && blueprint.CategoryID == Key)
            {
                Blueprints.Add(blueprint);
            }
        }

        base.OnReordered(dataIndex);
    }
#endif
}
