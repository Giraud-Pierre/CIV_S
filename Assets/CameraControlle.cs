using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraControlle : MonoBehaviour
{
    private Vector3 cameraUp; 
    [SerializeField] private Transform cameraTransform;
    private Vector2 input;
    private float cameraSpeed = 5;

    private Vector3 cameraRight;
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
   
        Vector3 translate = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
            );
        this.transform.Translate(translate * cameraSpeed * Time.deltaTime, Space.World);

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
