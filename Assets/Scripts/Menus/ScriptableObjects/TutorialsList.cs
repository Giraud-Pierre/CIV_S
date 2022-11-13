using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TutorialsList : ScriptableObject
{
    public List<TutorialSheet> tutorialList;
    public bool[] tutorialsThatHaveBeenSeen = new bool[3]{false, false, false};
}