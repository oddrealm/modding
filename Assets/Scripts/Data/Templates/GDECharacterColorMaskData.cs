using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterColorMaskData", order = 0)]
public class GDECharacterColorMaskData : Scriptable
{
    [System.Serializable]
    public class BodyPartColorPair
    {
#if UNITY_EDITOR
        [EnumFlag]
#endif
        public EntityBodyParts BodyPart = EntityBodyParts.ALL;
        public Color Target = Color.magenta;
    }

    public int Priority = 0;

    public BodyPartColorPair[] Pairs;
}
