using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexComponent : MonoBehaviour,IClick
{
    // Start is called before the first frame update
    public Hex hex;
    public HexMap hexMap;
    public void UpdatePosition()
    {
        this.transform.position = hex.PositionFromCamera(
            Camera.main.transform.position,
            hexMap.numberRows,
            hexMap.numberColumns
        );
    }

    public void OnLeftClickAction()
    {
        //*****************TODO: sélectionne l'hexagone (surbrillance, UI, ...)
    }

    public void OnLeftClickOnOtherAction()
    {

    }

    public void OnRightClickAction(GameObject gameobject)
    {
        //Do nothing, peut-être désélectionner l'hexagone ?
    }
}
