using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Faction")]
public class GDEFactionData : Scriptable
{
    public string[] EnemyFactions;
    public HashSet<string> EnemyFactionsHash = new HashSet<string>();
    public bool IsPlayerControlled = false;
    public bool UnlockRacialResearch = false;
    public bool CanRespec = false;
    public Color SelectionColor = Color.white;
    public Color CursorHoverColor = Color.white;
    public Color HighlightColor = Color.white;
    public FactionTypes FactionType = FactionTypes.PLAYER;
    public List<string> Statuses = new List<string>();
    public int Order = 0;
    public bool TrackByDefault = false;

    public override bool TryGetDefaultTracking(out DefaultTracking tracking)
    {
        tracking = new DefaultTracking()
        {
            TagID = "",
            TagObjectID = Key,
            HideIfZero = true,
            StartDisabled = !TrackByDefault,
        };

        return true;
    }

#if ODD_REALM_APP
    public override void SetOrderKey(string orderKey)
    {
        base.SetOrderKey(Order.ToString());
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        EnsureTag("tag_factions");

        for (int i = 0; EnemyFactions != null && i < EnemyFactions.Length; i++)
        {
            EnemyFactionsHash.Add(EnemyFactions[i]);
        }
    }
#endif
}
