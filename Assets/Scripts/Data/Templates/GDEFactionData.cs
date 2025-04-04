using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Faction")]
public class GDEFactionData : Scriptable
{
    public string[] EnemyFactions;
    public HashSet<string> EnemyFactionsHash = new HashSet<string>();
    public bool IsPlayerControlled = false;
    public bool CanRespec = false;
    public Color SelectionColor = Color.white;
    public Color CursorHoverColor = Color.white;
    public FactionTypes FactionPathing = FactionTypes.PLAYER;
    public List<string> Statuses = new List<string>();
    public int Order = 0;

#if ODD_REALM_APP
    public override void SetOrderKey(string orderKey)
    {
        base.SetOrderKey(Order.ToString());
    }

    public override void OnLoaded()
    {
        base.OnLoaded();

        for (int i = 0; EnemyFactions != null && i < EnemyFactions.Length; i++)
        {
            EnemyFactionsHash.Add(EnemyFactions[i]);
        }
    }
#endif
}
