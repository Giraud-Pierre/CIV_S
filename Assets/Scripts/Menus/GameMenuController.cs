using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject resourcesLayout;
    [SerializeField] private GameObject defaultBottomBar;
    [SerializeField] private GameObject actionMenuOnBuildSpace;
    [SerializeField] private GameObject actionMenuOnCityCenter;
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private TutorialsList allTutorials = default;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // TODO: Remove this debug feature
    // ! Remove only when all other features on Canvas (and his children) are implement
    public void MissingAction()
    {
        Debug.LogError("Missing Action : Button not assign");
    }

    private void Start()
    {
        if (allTutorials.tutorialsThatHaveBeenSeen[0] == false)
        {
            GetTutorial("bienvenue");
            allTutorials.tutorialsThatHaveBeenSeen[0] = true;
        }
    }

    public void UpdateResources(List<int> resources)
    {
        resourcesLayout.GetComponent<ResourcesLayoutController>().UpdateResourcesLayout(resources);
    }

    public void UpdateNumberOfTurn(int numberOfTurn)
    {
        defaultBottomBar.GetComponent<DefaultMenuController>().UpdateNumberOfTurn(numberOfTurn);
    }

    public void GetDefaultMenu()
    {
        infoPanel.SetActive(false);
        defaultBottomBar.SetActive(true);
        actionMenuOnBuildSpace.SetActive(false);
        actionMenuOnCityCenter.SetActive(false);
        buildMenu.SetActive(false);
    }

    public void GetMenuOnBuildSpace(Building build)
    {
        if (build.type == 0)
        {
            GetActionMenuOnCityCenter(build);
        }
        else
        {
            GetInfoOnBuildSpace(build);
        }
    }

    public void GetBuildMenu(bool[] buildingAvailable)
    {
        infoPanel.SetActive(false);
        defaultBottomBar.SetActive(false);
        actionMenuOnBuildSpace.SetActive(false);
        actionMenuOnCityCenter.SetActive(false);
        buildMenu.GetComponent<BuildMenuController>().GetBuildOptions(buildingAvailable);
    }

    public void GetTutorial(string titleOftheTutorial)
    {
        tutorialMenu.GetComponent<TutorialController>().NewTutorial(titleOftheTutorial);
    }

    public void GetInfoPanel(Building build)
    {
        infoPanel.GetComponent<InfoPanelController>().GetInfo(build);
    }

    private void GetInfoOnBuildSpace(Building build)
    {
        GetInfoPanel(build);
        defaultBottomBar.SetActive(false);
        actionMenuOnBuildSpace.SetActive(true);
        actionMenuOnCityCenter.SetActive(false);
        buildMenu.SetActive(false);
    }

    private void GetActionMenuOnCityCenter(Building build)
    {
        GetInfoPanel(build);
        defaultBottomBar.SetActive(false);
        actionMenuOnBuildSpace.SetActive(false);
        actionMenuOnCityCenter.GetComponent<MenuOnCityCenterController>().GetCreationOptions(build);
        buildMenu.SetActive(false);
    }
}
