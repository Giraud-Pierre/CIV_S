using System.Collections.Generic;
using UnityEngine;

public class BuildMenuController : MonoBehaviour
{
    // Unable to use an enumeration as an argument for a button's OnClick function.
    // Here is the match between button types and integers.
    // public enum TypeOfBuild {Sawmill = 1, Quarry = 2, Marketplace = 3, Dock = 4, HunterHut = 5};

    enum TypeOfResource {Food, Stone, Wood};

    [SerializeField] private GameObject InfoRessourcesLayout = default;
    [SerializeField] private GameObject sawmillButton = default;
    [SerializeField] private GameObject quarryButton = default;
    [SerializeField] private GameObject marketplaceButton = default;
    [SerializeField] private GameObject dockButton = default;
    [SerializeField] private GameObject hunterHutButton = default;

    private int selectedButton = 0; // No button selected by default


    private void OnEnable()
    {
        ClearPreviousSelectedButton();
        selectedButton = 0;
    }

    // Call by a "BuildButton" and get the build type as argument
    // Refer to the "TypeOfBuild" enumeration for matching button types and integers.
    public void OnClickOnButton(int buttonId)
    {
        if (buttonId == selectedButton)
        {
            DoActionForSelectedButton();
        }
        else
        {
            ClearPreviousSelectedButton();
            selectedButton = buttonId;
            SetButtonAsSelected();
        }
    }

    private void DoActionForSelectedButton()
    {
        // TODO: Add junction with action on map space and so on.
        Debug.Log("DoAction");
    }

    private void ClearPreviousSelectedButton()
    {
        InfoRessourcesLayout.GetComponent<InfoRessourcesViewer>().HideInfo();
        switch (selectedButton)
        {
            case 0:
                break;
            case 1:
                sawmillButton.GetComponent<BuildButtonController>().ClearSelectedState();
                break;
            case 2:
                quarryButton.GetComponent<BuildButtonController>().ClearSelectedState();
                break;
            case 3:
                marketplaceButton.GetComponent<BuildButtonController>().ClearSelectedState();
                break;
            case 4:
                dockButton.GetComponent<BuildButtonController>().ClearSelectedState();
                break;
            case 5:
                hunterHutButton.GetComponent<BuildButtonController>().ClearSelectedState();
                break;
            default:
                Debug.LogError("Unexpected state in ClearPreviousSelectedButton: selectedButton = " + selectedButton, gameObject);
                break;
        }
    }

    private void SetButtonAsSelected()
    {
        // TODO: Add junction with SciptableObject with action info

        switch (selectedButton)
        {
            case 0:
                break;
            case 1:
                InfoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowInfo(new List<int>{0,0,100});
                sawmillButton.GetComponent<BuildButtonController>().SetSelectedState();
                break;
            case 2:
                InfoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowInfo(new List<int>{200,0,0});
                quarryButton.GetComponent<BuildButtonController>().SetSelectedState();
                break;
            case 3:
                InfoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowInfo(new List<int>{-300,300,0});
                marketplaceButton.GetComponent<BuildButtonController>().SetSelectedState();
                break;
            case 4:
                InfoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowInfo(new List<int>{0,-400,400});
                dockButton.GetComponent<BuildButtonController>().SetSelectedState();
                break;
            case 5:
                InfoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowInfo(new List<int>{500,0,-500});
                hunterHutButton.GetComponent<BuildButtonController>().SetSelectedState();
                break;
            default:
                Debug.LogError("Unexpected state in SetButtonAsSelected: selectedButton = " + selectedButton, gameObject);
                break;
        }
    }
}
