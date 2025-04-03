using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OddRealm/Event/Dialogue", order = 15)]
public class GDEDialogueData : Scriptable
{
    public string AppearanceID = "";
    public List<string> NextDialogueID = new List<string>();
    public string SpeakerTarget = "";
    public string Message = "";
    public List<string> Response = new List<string>();
    public bool FocusOnSpeaker = true;

    public List<DialogueSequence> DialogueSequences = new List<DialogueSequence>();
}
