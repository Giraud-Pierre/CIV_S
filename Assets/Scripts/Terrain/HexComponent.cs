using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexComponent : MonoBehaviour
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
}
