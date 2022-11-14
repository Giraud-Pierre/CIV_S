using UnityEngine;
using TMPro;

public class InfoPanelController : MonoBehaviour
{
    [SerializeField] private GameObject nameText = default;
    [SerializeField] private GameObject resourcesPerTurnLayout = default;
    [SerializeField] private GameObject infoResourcesComponent = default;
    [SerializeField] private GameObject descriptionLayout = default;
    [SerializeField] private GameObject inBuildLayout = default;
    [SerializeField] private GameObject numberOfTurnBeforeConstruction = default;

    private Building build;

    public void GetInfo(Building newBuild)
    {
        build = newBuild;

        ChangeName();
        GetDescription();
        GetInBuildInfo();

        gameObject.SetActive(true);
    }

    private void ChangeName()
    {
        Debug.Log(build.name);
        nameText.GetComponent<TextMeshProUGUI>().text = build.name;
    }

    private void GetDescription()
    {
        // If it’s a "Town Center"
        if (build.type == 0)
        {
            resourcesPerTurnLayout.SetActive(false);
            descriptionLayout.SetActive(true);
        }
        else
        {
            int[] resourcesPerTurn = GenerateResourcesArray();
            infoResourcesComponent.GetComponent<InfoRessourcesViewer>().ShowInfo(resourcesPerTurn);

            descriptionLayout.SetActive(false);
            resourcesPerTurnLayout.SetActive(true);
        }
    }

    private int[] GenerateResourcesArray()
    {
        int[] resourcesArray = new int[3]{ 0, 0, 0};

        resourcesArray[build.type - 1] = build.quantityRessourceEachTurn;

        return resourcesArray;
    }

    private void GetInBuildInfo()
    {
        if (build.isBuilt != false)
        {
            inBuildLayout.SetActive(false);
        }
        else
        {
            numberOfTurnBeforeConstruction.GetComponent<TextMeshProUGUI>().text = build.turnsRemainingUntilBuildIsComplete.ToString();

            inBuildLayout.SetActive(true);
        }
    }
}
