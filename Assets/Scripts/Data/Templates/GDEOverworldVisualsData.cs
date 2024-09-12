using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldVisuals")]
public class GDEOverworldVisualsData : Scriptable
{
	public int TextureX = 0;
	public int TextureY = 0;
	[Header("Used for visibility")]
	public OverworldLayers Layer = OverworldLayers.TERRAIN;
	[Header("Visuals will try to transition with other visuals in this channel")]
	public int TransitionChannel;
	public int TransitionKey = 0;
	public bool HasTransition = false;
	public int TailVariantCount = 0;
	public bool Animated = false;
	public Color ColorMult = Color.white;
    public override bool ShowMinimapCutoutColor { get { return true; } }
}
