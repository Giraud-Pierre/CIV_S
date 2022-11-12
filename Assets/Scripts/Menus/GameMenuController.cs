using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject resourcesLayout;
    [SerializeField] private GameObject defaultBottomBar;
    // [SerializeField] private GameObject actionMenuOnEmptySpace;
    [SerializeField] private GameObject actionMenuOnBuildSpace;
    [SerializeField] private GameObject actionMenuOnCityCenter;
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject tutorialMenu;

    private bool emptySpace;
    private bool isCityCenter;

    // TODO: Remove this debug feature
    // ! Remove only when all other features on Canvas (and his children) are implement
    public void MissingAction()
    {
        Debug.LogError("Missing Action : Button not assign");
    }

    private void Start()
    {
        GetTutorial("bienvenue");
    }

    public void UpdateResources(List<int> resources)
    {
        resourcesLayout.GetComponent<ResourcesLayoutController>().UpdateResourcesLayout(resources);
    }

    public void GetDefaultMenu()
    {
        defaultBottomBar.SetActive(true);
        actionMenuOnBuildSpace.SetActive(false);
        // actionMenuOnEmptySpace.SetActive(false);
        actionMenuOnCityCenter.SetActive(false);
        buildMenu.SetActive(false);
    }

    public void GetActionMenu()
    {
        // TODO: Add junction with map spaces
        // Need to get the state of the space where the player is.

        if (emptySpace)
        {
            GetBuildMenu();
        }
        else
        {
            if (isCityCenter)
            {
                GetActionMenuOnCityCenter();
            }
            else
            {
                GetActionMenuOnBuildSpace();
            }
        }
    }

    public void GetBuildMenu()
    {
        defaultBottomBar.SetActive(false);
        actionMenuOnBuildSpace.SetActive(false);
        // actionMenuOnEmptySpace.SetActive(false);
        actionMenuOnCityCenter.SetActive(false);
        buildMenu.SetActive(true);
    }

    public void GetTutorial(string titleOftheTutorial)
    {
        tutorialMenu.GetComponent<TutorialController>().NewTutorial(titleOftheTutorial);
    }


    // private void GetActionMenuOnEmptySpace()
    // {
    //     defaultBottomBar.SetActive(false);
    //     actionMenuOnBuildSpace.SetActive(false);
    //     actionMenuOnEmptySpace.SetActive(true);
    //     actionMenuOnCityCenter.SetActive(false);
    //     buildMenu.SetActive(false);
    // }

    private void GetActionMenuOnBuildSpace()
    {
        defaultBottomBar.SetActive(false);
        actionMenuOnBuildSpace.SetActive(true);
        // actionMenuOnEmptySpace.SetActive(false);
        actionMenuOnCityCenter.SetActive(false);
        buildMenu.SetActive(false);
    }

    private void GetActionMenuOnCityCenter()
    {
        defaultBottomBar.SetActive(false);
        actionMenuOnBuildSpace.SetActive(false);
        // actionMenuOnEmptySpace.SetActive(false);
        actionMenuOnCityCenter.SetActive(true);
        buildMenu.SetActive(false);
    }
}
