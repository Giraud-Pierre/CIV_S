using System.Collections.Generic;
using UnityEngine;

public class InfoRessourcesViewer : MonoBehaviour
{
    [SerializeField] private GameObject infoFoodElement = default;
    [SerializeField] private GameObject infoStoneElement = default;
    [SerializeField] private GameObject infoWoodElement = default;

    public void ShowInfo(int[] resourcesNeeded)
    {
        infoFoodElement.gameObject.GetComponent<InfoElementController>().SetNumber(resourcesNeeded[0]);
        infoStoneElement.gameObject.GetComponent<InfoElementController>().SetNumber(resourcesNeeded[1]);
        infoWoodElement.gameObject.GetComponent<InfoElementController>().SetNumber(resourcesNeeded[2]);
    }

    public void ShowNegativeInfo(int[] resourcesNeeded)
    {
        infoFoodElement.gameObject.GetComponent<InfoElementController>().SetNumber(resourcesNeeded[0] * -1);
        infoStoneElement.gameObject.GetComponent<InfoElementController>().SetNumber(resourcesNeeded[1] * -1);
        infoWoodElement.gameObject.GetComponent<InfoElementController>().SetNumber(resourcesNeeded[2] * -1);
    }

    public void HideInfo()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
