using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills")]
public class GDESkillsData : Scriptable
{
    public string ResearchKey = "";
    public string LevelUpScenario = "";
    public int XPMod = 0;
    public int EnergyCost = 0;
    public bool IsCombatSkill;

#if ODD_REALM_APP
    public override void Init()
    {
        base.Init();
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        EnsureTag("tag_skills");
    }
#endif
}
