using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = this.transform.position;
    }

    Vector3 oldPosition;

    // Update is called once per frame
    void Update()
    {
        //Code to click and drag camera 
        // WASD 
        // ZOOM in and out
        CheckIfCameraMoved();
    }

    public void PanToHex(Hex hex)
    {
        // TODO : Move camera to hex
    }

    void CheckIfCameraMoved()
    {
        if(oldPosition != this.transform.position)
        {
            //Something moved the camera.
            oldPosition = this.transform.position;

            //TODO : Probably HexMap will have a dictionary of all these later
            HexComponent[] hexes = GameObject.FindObjectsOfType<HexComponent>();

            foreach(HexComponent hex in hexes)
            {
                hex.UpdatePosition();
            }
        }
    }
}
