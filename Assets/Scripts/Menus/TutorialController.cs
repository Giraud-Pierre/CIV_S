using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private TutorialsList allTutorials = default;
    [SerializeField] private GameObject titleArea = default;
    [SerializeField] private GameObject textArea = default;

    private List<string> currentTutorialText;
    private int currentTutorialIndex;

    public void NewTutorial(string titleOftutorial)
    {
        currentTutorialText = GetTextOfTutorial(titleOftutorial);
        currentTutorialIndex = 0;

        titleArea.GetComponent<TextMeshProUGUI>().text = titleOftutorial;
        textArea.GetComponent<TextMeshProUGUI>().text  = currentTutorialText[0];
        gameObject.SetActive(true);
    }

    public void nextDialogue()
    {
        if (currentTutorialIndex + 1 < currentTutorialText.Count)
        {
            currentTutorialIndex += 1;
            textArea.GetComponent<TextMeshProUGUI>().text  = currentTutorialText[currentTutorialIndex];
        }
    }

    public void previousDialogue()
    {
        if (currentTutorialIndex - 1 >= 0)
        {
            currentTutorialIndex -= 1;
            textArea.GetComponent<TextMeshProUGUI>().text  = currentTutorialText[currentTutorialIndex];
        }
    }

    public void CloseTutorial()
    {
        if (allTutorials.tutorialsThatHaveBeenSeen[1] == false)
        {
            NewTutorial("Règle du jeu");
            allTutorials.tutorialsThatHaveBeenSeen[1] = true;
        }
        else if (allTutorials.tutorialsThatHaveBeenSeen[2] == false)
        {
            NewTutorial("Manipulation");
            allTutorials.tutorialsThatHaveBeenSeen[2] = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private List<string> GetTextOfTutorial(string title)
    {
        foreach (TutorialSheet tutorial in allTutorials.tutorialList)
        {
            if (title == tutorial.title)
            {
                return tutorial.tutorialText;
            }
        }

        return new List<string>{"No tutorial found for " + title};
    }
}
