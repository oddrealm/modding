using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BlockVisuals")]
public class GDEBlockVisualsData : ScriptableObject
{
	public string Key;
	public bool ReceiveSkylightColor = false;
	public bool OnlyRenderIfVisible = false;
	public BlockRotationTypes RotationType = 0;
	public BlockVisibilityFlags VisibilityFlag = 0;
	public bool ShowPermissionColor = false;
	public BlockColorAddFlag ColorAddType = 0;
	public int HasTriggerOffset = 0;
	public float DepthOffset = 0.0f;
	public bool HideSurfaceWhenObscuredAbove = false;
	public bool HideSurfaceWhenSameTypeAbove = false;
	public bool CanTransitionIfObscuredAbove = false;
	public bool ObscureDirectSurfaceBelow = false;
	public bool HasSurface = false;
	public bool HasSeasonOffset = false;
	public bool HasMapColor = false;
	public Color MapColor = Color.magenta;
	public int TextureX = 0;
	public int TextureY = 0;
	public int WeatherY = 0;
	public BlockTransitionTypes TransitionType = 0;
	public int TransitionKey = 0;
	public bool CanTransitionNULL = false;
	public bool IsRotationFixture = false;
	public bool BlockAmbientLight = false;
	public bool ShowAmbientLight = false;
	public int TailVariants = 0;
	public bool ShowTailVariant = false;
	public bool ShowAnims = false;
	public int TemperatureStrength = 0;
	public int TemperatureHorizontalPass = 0;
	public int TemperatureDownPass = 0;
	public int TemperatureUpPass = 0;
	public bool ShowLighting = false;
	public int SpotlightStrength = 0;
	public int SpotlightHorizontalPass = 0;
	public int SpotlightDownPass = 0;
	public int SpotlightUpPass = 0;
	public string AddVFX = "";
	public string RemoveVFX = "";
	public string InteractVFX = "";
	public string IdleVFX = "";
	public int SkylightPass = 0;
	public int SkylightMin = 0;
	public int SkylightMax = 0;

	public void Clone(GDEBlockVisualsData other)
    {
		if (other == null) { return; }

		ReceiveSkylightColor = other.ReceiveSkylightColor;
		OnlyRenderIfVisible = other.OnlyRenderIfVisible;
		RotationType = other.RotationType;
		VisibilityFlag = other.VisibilityFlag;
		ShowPermissionColor = other.ShowPermissionColor;
		ColorAddType = other.ColorAddType;
		HasTriggerOffset = other.HasTriggerOffset;
		DepthOffset = other.DepthOffset;
		HideSurfaceWhenObscuredAbove = other.HideSurfaceWhenObscuredAbove;
		HideSurfaceWhenSameTypeAbove = other.HideSurfaceWhenSameTypeAbove;
		CanTransitionIfObscuredAbove = other.CanTransitionIfObscuredAbove;
		ObscureDirectSurfaceBelow = other.ObscureDirectSurfaceBelow;
		HasSurface = other.HasSurface;
		HasSeasonOffset = other.HasSeasonOffset;
		HasMapColor = other.HasMapColor;
		MapColor = other.MapColor;
		TextureX = other.TextureX;
		TextureY = other.TextureY;
		WeatherY = other.WeatherY;
		TransitionType = other.TransitionType;
		TransitionKey = other.TransitionKey;
		CanTransitionNULL = other.CanTransitionNULL;
		IsRotationFixture = other.IsRotationFixture;
		BlockAmbientLight = other.BlockAmbientLight;
		ShowAmbientLight = other.ShowAmbientLight;
		TailVariants = other.TailVariants;
		ShowTailVariant = other.ShowTailVariant;
		ShowAnims = other.ShowAnims;
		TemperatureStrength = other.TemperatureStrength;
		TemperatureHorizontalPass = other.TemperatureHorizontalPass;
		TemperatureDownPass = other.TemperatureDownPass;
		TemperatureUpPass = other.TemperatureUpPass;
		ShowLighting = other.ShowLighting;
		SpotlightStrength = other.SpotlightStrength;
		SpotlightHorizontalPass = other.SpotlightHorizontalPass;
		SpotlightDownPass = other.SpotlightDownPass;
		SpotlightUpPass = other.SpotlightUpPass;
		AddVFX = other.AddVFX;
		RemoveVFX = other.RemoveVFX;
		InteractVFX = other.InteractVFX;
		IdleVFX = other.IdleVFX;
		SkylightPass = other.SkylightPass;
		SkylightMin = other.SkylightMin;
		SkylightMax = other.SkylightMax;
	}
}
