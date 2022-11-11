using UnityEngine;
using UnityEngine.UI;

public class BuildButtonController : MonoBehaviour
{
    [SerializeField] private Image imageComponent = default;

    public void ClearSelectedState()
    {
        imageComponent.enabled = false;
    }

    public void SetSelectedState()
    {
        imageComponent.enabled = true;
    }
}
