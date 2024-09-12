using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BlockVisuals")]
public class GDEBlockVisualsData : Scriptable
{
	[System.Serializable]
	public struct Light
	{
		public byte ReductionThreshold;
		public byte SkylightReduction;
		public byte SkylightPass;
		public bool BlockAmbientLight;
	}

	[Header("Texture coordinates")]
	public int TextureX = 0;
	public int TextureY = 0;
	[Header("Sec Texture coordinates - (does not have a transition)")]
	public int SecondaryTextureX = 0;
	public int SecondaryTextureY = 0;
    [Header("FX Texture coordinates - (does not have a transition)")]
    public int FXTextureX = 0;
    public int FXTextureY = 0;
    [Header("Transitions")]
	public int TransitionKey = 0;
	public int TransitionKeyDelta = 0;
	public BlockTransitionTypes TransitionType = 0;
	public bool CanTransitionNULL = false;
	public bool IsEntityShadowObstruction = false;
	public bool ReceiveSkylightColor = false;
	public bool ReceiveReducedLighting = false;
	public bool OnlyRenderIfVisible = false;
	public BlockColorAddFlag ColorAddType = 0;
	public BlockVisibilityFlags VisibilityFlag = 0;
	public BlockPermissionTypes PermissionFlags;
	public BlockRotationTypes RotationType = 0;
	public bool ShowPermissionColor = false;
	public float DepthOffset = 0.0f;
	public bool HideSurfaceWhenObscuredAbove = false;
	public bool HideSurfaceWhenSameTypeAbove = false;
	public bool CanTransitionIfObscuredAbove = false;
	public bool HasSurface = false;
	public bool CanFade = false;
	public bool HasSeasonOffset = false;
	public int WeatherY = 0;
	public bool ShowAmbientLight = false;
	public int TailVariants = 0;
	public bool ShowTailVariant = false;
	public bool ShowAnims = false;
	public bool ShowLighting = false;
	public float DropShadow = 1f;
	

	public Light LightSettings;
	public override bool ShowMinimapCutoutColor { get { return HasSurface; } }

	public override bool ShowOnMinimap { get { return (VisibilityFlag & BlockVisibilityFlags.TERRAIN) != BlockVisibilityFlags.NONE; } }

#if ODD_REALM_APP
    public void PopulateRenderUVs(RenderTileDataUVs uvs)
    {
        uvs.Written = true;
        uvs.Rotation = 0;
        uvs.CanFade = this.CanFade;
        //uvs.ObscuredAbove = false;
        uvs.PermissionFlags = PermissionFlags;
        uvs.PermissionFlagsAbove = BlockPermissionTypes.NONE;
        uvs.TransitionType = this.TransitionType;
        uvs.RotationType = this.RotationType;
        uvs.TransitionKey = this.TransitionKey;
        uvs.TransitionKeyDelta = this.TransitionKeyDelta;
		uvs.CanTransitionNull = this.CanTransitionNULL;
        uvs.TextureX = this.TextureX;
        uvs.TextureY = this.TextureY;
		uvs.SecondaryTextureX = this.SecondaryTextureX;
		uvs.SecondaryTextureY = this.SecondaryTextureY;
		//uvs.DepthOffset = this.DepthOffset;
        uvs.WeatherY = this.WeatherY;
        uvs.CanTransitionIfObscuredAbove = this.CanTransitionIfObscuredAbove;
        uvs.HideSurfaceWhenObscuredAbove = this.HideSurfaceWhenObscuredAbove;
        uvs.HideSurfaceWhenSameTypeAbove = this.HideSurfaceWhenSameTypeAbove;
        uvs.HasSurface = this.HasSurface;
        uvs.TailVariants = this.TailVariants;
        uvs.ShowAnims = this.ShowAnims;
        uvs.ShowLighting = this.ShowLighting;
        uvs.ShowPermissionColor = this.ShowPermissionColor;
        uvs.ColorAddType = this.ColorAddType;
        uvs.HasSeasonOffset = this.HasSeasonOffset;
        uvs.ShowAmbientLight = this.ShowAmbientLight;
		uvs.ReceiveSkylightColor = this.ReceiveSkylightColor;
        uvs.ShowTailVariant = this.ShowTailVariant;
        uvs.ReceiveReducedLighting = this.ReceiveReducedLighting;
        uvs.VisibilityFlag = this.VisibilityFlag;
    }
#endif
}