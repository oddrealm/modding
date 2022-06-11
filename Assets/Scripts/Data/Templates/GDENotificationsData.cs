using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Notifications")]
public class GDENotificationsData : ScriptableObject
{
	public string Key { get { return name; } }
	public int Index = 0;
	public bool Enabled = false;
	public NotificationTypes NotificationType = 0;
	public int Priority = 0;
	public int FilterType = 0;
	public string Sound = "";
	public int FactionToShowPipFor = 0;
	public List<string> DefaultPermitted = new List<string>();
	public string InlineIcon = "";
	public bool ShowInSettings = false;
	public string FriendlyName = "";
	public string DescriptionDisplay = "";
	public string DuplicateDescriptionDisplay = "";
	public string UIToOpen = "";
	public bool ShowPip = false;
	public float PipInterval = 0.0f;
	public bool PauseGameOnCreate = false;
	public float PauseGameInterval = 0.0f;
	public bool FocusCamOnCreate = false;
	public float FocusCamInterval = 0.0f;
	public bool CanSetFaction = false;
	public bool CanSetRace = false;
}
