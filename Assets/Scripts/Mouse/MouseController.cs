using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    [SerializeField] Texture2D cursor;
    [SerializeField] Texture2D cursorClicked;

    private Camera mainCamera;
    private CursorControls controls;
    private GameObject selectedGameObject = null;

    delegate void RightClickAction();
    RightClickAction CurrenRightClickAction;

    Vector3 lastMousePosition; //From Input.mousePosition
    float cameraSpeed = 10f;

    private void Awake()
    {
        controls = new CursorControls();
        ChangeCursor(cursor);
        Cursor.lockState = CursorLockMode.Confined;
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        controls.Mouse.leftClick.performed += _ => LeftClick();     //Lors de l'appui sur le clic gauche
        controls.Mouse.rightClick.started += _ => StartRightClick(); //Lors de l'appui sur le clic droit
        controls.Mouse.rightClick.performed += _ => EndRightClick(); //Lors du relachement du clic gauche

        CurrenRightClickAction = DoNothing;
    }

    // Update is called once per frame
    void Update()
    {
        CurrenRightClickAction();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    void MoveCamera()
    {
        Vector3 movement = lastMousePosition - Input.mousePosition;
        if (movement != Vector3.zero)
        {
            movement.z = movement.y;
            movement.y = 0;
            //Left button is being held down and the mouse move, that's the camera drag !
            mainCamera.transform.Translate(movement*cameraSpeed*Time.deltaTime, Space.World);
        }
        lastMousePosition = Input.mousePosition;
    }

    private void StartRightClick()
    {
        ChangeCursor(cursorClicked);
        if (selectedGameObject == null)
        {
            lastMousePosition = Input.mousePosition;
            CurrenRightClickAction = MoveCamera;
        }
    }

    private void EndRightClick()
    {
        ChangeCursor(cursor);
        if (selectedGameObject == null)
        {
            CurrenRightClickAction = DoNothing;
        }
        else
        {
            Ray ray = mainCamera.ScreenPointToRay(controls.Mouse.Position.ReadValue<Vector2>());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    selectedGameObject.GetComponent<IClick>().OnRightClickAction(hit.collider.gameObject);
                }
            }
        }
    }

    private void ChangeCursor(Texture2D cursorType)
    {
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }

    private void DoNothing()
    {

    }

    private void LeftClick()
    {
        ChangeCursor(cursor);
        DetectObject();
    }

    private void DetectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(controls.Mouse.Position.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                if(selectedGameObject == null)
                {
                    selectedGameObject.GetComponent<IClick>().OnLeftClickOnOtherAction();
                    selectedGameObject = hit.collider.gameObject;
                    selectedGameObject.GetComponent<IClick>().OnLeftClickAction();
                }
                else
                {
                    selectedGameObject = null;
                    //***************TODO : déselectionne le gameobject
                }
            }
        }
    }
}
