using UnityEngine;
using TMPro;

public class ResourceElementController : MonoBehaviour
{
    // enum TypeOfResource {Food, Stone, Wood};

    // [SerializeField] private TypeOfResource typeOfResource;
    [SerializeField] private TMP_Text textcomponent = default;

    public void UpdateElement(int number)
    {
        textcomponent.text = number.ToString();
    }
}
