using UnityEngine;
using TMPro;

public class DefaultMenuController : MonoBehaviour
{
    [SerializeField] private GameObject numberOfTurnText = default;

    public void UpdateNumberOfTurn(int number)
    {
        numberOfTurnText.GetComponent<TextMeshProUGUI>().text = number.ToString();
    }

}
