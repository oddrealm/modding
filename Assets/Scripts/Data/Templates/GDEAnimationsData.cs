using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Animation/Animations", order = 0)]
public class GDEAnimationsData : Scriptable
{
    [System.Serializable]
    public class AnimAccessories
    {
        public EntityAccessoryPoints Point = EntityAccessoryPoints.HEAD;
        public Vector3Int Position = new Vector3Int();
        public Vector2Int TextureOffset = new Vector2Int();
        public int Rotation;

        public AnimAccessories() { }
        public AnimAccessories(AnimAccessories clone)
        {
            Point = clone.Point;
            Position = clone.Position;
            TextureOffset = clone.TextureOffset;
            Rotation = clone.Rotation;
        }
    }

    [System.Serializable]
    public class AnimFrame
    {
        public Sprite AnimSprite;
        public AnimAccessories[] Accessories;
        public EntityAnimationTriggers Triggers;
        public EntityAnimationEvents Events;
        public Vector2Int ShadowPosition = new Vector2Int(0, -1);

        public AnimFrame() { }
        public AnimFrame(AnimFrame clone)
        {
            Clone(clone);
        }

        public void Clone(AnimFrame clone)
        {
            AnimSprite = clone.AnimSprite;

            if (clone.Accessories != null && clone.Accessories.Length > 0)
            {
                Accessories = new AnimAccessories[clone.Accessories.Length];

                for (int i = 0; i < Accessories.Length; i++)
                {
                    Accessories[i] = new AnimAccessories(clone.Accessories[i]);
                }
            }

            ShadowPosition = clone.ShadowPosition;
            Triggers = clone.Triggers;
            Events = clone.Events;
        }
    }

    public bool Loop = true;
    public int FramesPerSecond = 16;
    public AnimFrame[] Frames;
}
