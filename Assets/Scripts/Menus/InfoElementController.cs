using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoElementController : MonoBehaviour
{
    [SerializeField] private GameObject getLayout = default;
    [SerializeField] private TMP_Text getText = default;
    [SerializeField] private GameObject loseLayout = default;
    [SerializeField] private TMP_Text loseText = default;

    public void SetNumber(int number)
    {
        if ( number == 0 )
        {
            return;
        }
        else if ( number > 0 )
        {
            getText.text = number.ToString();
            loseLayout.SetActive(false);
            getLayout.SetActive(true);
            gameObject.SetActive(true);
        }
        else
        {
            loseText.text = number.ToString();
            getLayout.SetActive(false);
            loseLayout.SetActive(true);
            gameObject.SetActive(true);
        }
    }
}
