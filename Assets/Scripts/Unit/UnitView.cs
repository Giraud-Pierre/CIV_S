using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    private void Start()
    {
       newPosition = this.transform.position;
    }

    Vector3 newPosition;
    Vector3 currentVelocity;
    float smoothTime = 0.5f;

    public void OnUnitMoved(Hex oldHex, Hex nexHex)
    {
        //Animate the unit moving from oldHex to newHex
        this.transform.position = oldHex.PositionFromCamera();
        newPosition = nexHex.PositionFromCamera();
        currentVelocity = Vector3.zero;

        if(Vector3.Distance(this.transform.position,newPosition) > 2)
        {
            //This OnUNitMoved is considerably more than the expected move
            //between two adjacent tiles -- it's probably a map seam thing 
            // So just teleport
            this.transform.position = newPosition;

        }
    }

    void Update()
    {
        this.transform.position = Vector3.SmoothDamp(this.transform.position, newPosition, ref currentVelocity, smoothTime);
    }
}
