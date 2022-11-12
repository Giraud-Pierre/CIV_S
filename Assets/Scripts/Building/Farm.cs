using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Farm : Building
{
    [SerializeField] GameObject farmGO;
    public Farm(string name, int cost):base(name, cost)
    {
        this.typeOfBuilding = 1;
    }
}
