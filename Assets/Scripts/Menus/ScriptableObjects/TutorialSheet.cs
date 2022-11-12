using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TutorialSheet : ScriptableObject
{
    public string title;

    [TextArea]
    public List<string> tutorialText;
}