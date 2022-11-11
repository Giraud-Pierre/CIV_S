using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject ResourcesLayout;
    [SerializeField] private GameObject DefaultBottomBar;
    [SerializeField] private GameObject ActionMenuOnEmptySpace;
    [SerializeField] private GameObject ActionMenuOnBuildSpace;
    [SerializeField] private GameObject BuildMenu;
    [SerializeField] private GameObject TutorialMenu;

    private bool EmptySpace;

    // TODO: Remove this debug feature
    // ! Remove only when all other features on Canvas (and his children) are implement
    public void MissingAction()
    {
        Debug.LogError("Missing Action : Button not assign");
    }

    public void UpdateRessources()
    {
        ResourcesLayout.GetComponent<ResourcesLayoutController>().UpdateRessourcesLayout();
    }

    public void GetDefaultMenu()
    {
        DefaultBottomBar.SetActive(false);
        ActionMenuOnBuildSpace.SetActive(false);
        ActionMenuOnEmptySpace.SetActive(false);
        BuildMenu.SetActive(true);
    }

    public void GetActionMenu()
    {
        // TODO: Add junction with map spaces
        // Need to get the state of the space where the player is.

        if (EmptySpace)
        {
            GetActionMenuOnEmptySpace();
        }
        else
        {
            GetActionMenuOnBuildSpace();
        }
    }

    public void GetBuildMenu()
    {
        DefaultBottomBar.SetActive(false);
        ActionMenuOnBuildSpace.SetActive(false);
        ActionMenuOnEmptySpace.SetActive(false);
        BuildMenu.SetActive(true);
    }

    public void GetTutorial(List<string> Text)
    {
        // TODO(Optional): Add junction with TutorialController
    }


    private void GetActionMenuOnEmptySpace()
    {
        DefaultBottomBar.SetActive(false);
        ActionMenuOnBuildSpace.SetActive(false);
        ActionMenuOnEmptySpace.SetActive(true);
        BuildMenu.SetActive(false);
    }

    private void GetActionMenuOnBuildSpace()
    {
        DefaultBottomBar.SetActive(false);
        ActionMenuOnBuildSpace.SetActive(true);
        ActionMenuOnEmptySpace.SetActive(false);
        BuildMenu.SetActive(false);
    }
}
