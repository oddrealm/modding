using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AttackGroups")]
public class GDEAttackGroupsData : Scriptable
{
	public List<string> Attacks = new List<string>();

#if ODD_REALM_APP
    public override void OnLoaded()
    {
        for (int i = 0; i < Attacks.Count; i++)
        {
            if (DataManager.TagObjectExists(Attacks[i])) { continue; }

            Debug.LogError($"{Key} attack {Attacks[i]} does not exist!");
        }

        base.OnLoaded();
    }
#endif
}
