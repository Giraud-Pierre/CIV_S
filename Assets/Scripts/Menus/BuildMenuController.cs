using System.Collections.Generic;
using UnityEngine;

public class BuildMenuController : MonoBehaviour
{
    // Unable to use an enumeration as an argument for a button's OnClick function.
    // Here is the match between button types and integers.
    // public enum TypeOfBuild {CityCenter = 1, Farm = 2, LumberJackhutt = 3, Mine = 4, HunterHut = 5};

    enum TypeOfResource {Food, Stone, Wood};

    [SerializeField] private GameObject InfoRessourcesLayout = default;
    [SerializeField] private GameObject cityCenterButton = default;
    [SerializeField] private GameObject farmButton = default;
    [SerializeField] private GameObject lumberJackhuttButton = default;
    [SerializeField] private GameObject mineButton = default;
    // [SerializeField] private GameObject hunterHutButton = default;

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
                cityCenterButton.GetComponent<BuildButtonController>().ClearSelectedState();
                break;
            case 2:
                farmButton.GetComponent<BuildButtonController>().ClearSelectedState();
                break;
            case 3:
                lumberJackhuttButton.GetComponent<BuildButtonController>().ClearSelectedState();
                break;
            case 4:
                mineButton.GetComponent<BuildButtonController>().ClearSelectedState();
                break;
            // case 5:
            //     hunterHutButton.GetComponent<BuildButtonController>().ClearSelectedState();
            //     break;
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
                cityCenterButton.GetComponent<BuildButtonController>().SetSelectedState();
                break;
            case 2:
                InfoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowInfo(new List<int>{200,0,0});
                farmButton.GetComponent<BuildButtonController>().SetSelectedState();
                break;
            case 3:
                InfoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowInfo(new List<int>{-300,300,0});
                lumberJackhuttButton.GetComponent<BuildButtonController>().SetSelectedState();
                break;
            case 4:
                InfoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowInfo(new List<int>{0,-400,400});
                mineButton.GetComponent<BuildButtonController>().SetSelectedState();
                break;
            // case 5:
            //     InfoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowInfo(new List<int>{500,0,-500});
            //     hunterHutButton.GetComponent<BuildButtonController>().SetSelectedState();
            //     break;
            default:
                Debug.LogError("Unexpected state in SetButtonAsSelected: selectedButton = " + selectedButton, gameObject);
                break;
        }
    }
}
