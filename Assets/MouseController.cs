using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    delegate void UpdateFunc();
    UpdateFunc Update_CurrentFunc;

    Vector3 lastMousePosition; //From Input.mousePosition

    // Start is called before the first frame update
    void Start()
    {
        Update_CurrentFunc = Update_DetectModeStart;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            CancelUpdateFunc();
        }
        Update_CurrentFunc();
        lastMousePosition = Input.mousePosition;
    }

    void CancelUpdateFunc()
    {
        Update_CurrentFunc = Update_DetectModeStart;
    }

    void Update_DetectModeStart()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Left mouse button just went down.
            //
        }

        else if(Input.GetMouseButton(0) && Input.mousePosition != lastMousePosition)
        {
            //Left button is being held down and the mouse move, that's the camera drag !
        }

    }
}
