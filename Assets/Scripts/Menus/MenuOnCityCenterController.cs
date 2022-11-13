using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOnCityCenterController : MonoBehaviour
{
    [SerializeField] private HexMap hexMap = default;
    [SerializeField] private UnitPokedex unitsInfo = default;

    [SerializeField] private GameObject canvas = default;

    [SerializeField] private GameObject infoRessourcesLayout = default;
    [SerializeField] private GameObject farmerButton = default;
    // [SerializeField] private GameObject hunterButton = default;

    private Hex hex;
    private int selectedButton = 0; // No button selected by default


    public void GetCreationOptions(Building cityCenter)
    {
        hex = cityCenter.hex;
        // ChangeStateOfButtons();

        gameObject.SetActive(true);

        ClearPreviousSelectedButton();
        selectedButton = 0;
    }

    // Call by an "ActionButton" and get the build type as argument
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
        hexMap.GetComponent<HexMap_Continent>().SpawnUnitAt(selectedButton - 1, hex.Q, hex.R);
        selectedButton = 0;
        canvas.GetComponent<GameMenuController>().GetDefaultMenu();
    }

    // private void ChangeStateOfButtons()
    // {
    //     farmerButton.SetActive(buttonAvailable[0]);
    //     hunterButton.SetActive(buttonAvailable[1]);
    // }

    private void ClearPreviousSelectedButton()
    {
        infoRessourcesLayout.GetComponent<InfoRessourcesViewer>().HideInfo();
        switch (selectedButton)
        {
            case 0:
                break;
            case 1:
                farmerButton.GetComponent<BuildButtonController>().ClearSelectedState();
                break;
            // case 2:
            //     hunterButton.GetComponent<BuildButtonController>().ClearSelectedState();
            //     break;
            default:
                Debug.LogError("Unexpected state in CityCenter.ClearPreviousSelectedButton: selectedButton = " + selectedButton, gameObject);
                break;
        }
    }

    private void SetButtonAsSelected()
    {
        // TODO: (Optional) Improve the way to get information from ScriptableObject buildingInfo

        switch (selectedButton)
        {
            case 0:
                break;
            case 1:
                infoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowNegativeInfo(unitsInfo.units[0].Cost.ToArray());
                farmerButton.GetComponent<BuildButtonController>().SetSelectedState();
                break;
            // case 2:
            //     infoRessourcesLayout.GetComponent<InfoRessourcesViewer>().ShowNegativeInfo(unitsInfo.units[1].Cost.ToArray());
            //     hunterButton.GetComponent<BuildButtonController>().SetSelectedState();
            //     break;
            default:
                Debug.LogError("Unexpected state in SetButtonAsSelected: selectedButton = " + selectedButton, gameObject);
                break;
        }
    }
}
