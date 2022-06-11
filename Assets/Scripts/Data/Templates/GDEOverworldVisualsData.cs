using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/OverworldVisuals")]
public class GDEOverworldVisualsData : ScriptableObject
{
	public string Key { get { return name; } }
	public int MeshIndex = 0;
	public int TextureX = 0;
	public int TextureY = 0;
	public int TransitionKey = 0;
	public bool HasTransition = false;
	public int TailVariantCount = 0;
	public bool Animated = false;
}
