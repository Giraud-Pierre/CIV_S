using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLayoutController : MonoBehaviour
{
    public void UpdateRessourcesLayout()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<ResourceElementController>().UpdateElement();
        }
    }
}
