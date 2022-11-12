using System.Collections.Generic;
using UnityEngine;

public class ResourcesLayoutController : MonoBehaviour
{
    public void UpdateResourcesLayout(List<int> resources)
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<ResourceElementController>().UpdateElement(resources[i]);
            i += 1;
        }
    }
}
