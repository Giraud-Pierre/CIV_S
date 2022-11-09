using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject textArea = default;

    private List<string> Text;

    public void NewTutorial(List<string> newText)
    {
        Text = newText;
        // TODO: Add text in the textArea
        gameObject.SetActive(true);
    }

    public void nextDialogue()
    {
        // TODO: Add Feature : nextDialogue
        Debug.LogError("Missing Action : nextDialogue not implements");
    }

    public void previousDialogue()
    {
        // TODO: Add Feature : previousDialogue
        Debug.LogError("Missing Action : previousDialogue not implements");
    }

    public void CloseTutorial()
    {
        gameObject.SetActive(false);
    }
}
