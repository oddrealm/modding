using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills", order = 0)]
public class GDESkillsData : Scriptable
{
    public string ResearchKey = "";
    public string LevelUpScenario = "";
    public bool VisibleToPlayer = false;
    public int XPMod = 0;
    public int EnergyCost = 0;
    public bool IsCombatSkill;
}
