using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClick
{
    public void OnLeftClickAction();
    public void OnLeftClickOnOtherAction();
    public void OnRightClickAction(GameObject gameobject);
}
