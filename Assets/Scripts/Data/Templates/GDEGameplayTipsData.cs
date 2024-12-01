using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameplayTips")]
public class GDEGameplayTipsData : Scriptable
{
    [System.Serializable]
    public struct Text
    {
        public string TextDisplay;
        public string InputID;
    }

    public Text[] DisplayText;
}
