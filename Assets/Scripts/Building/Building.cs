using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public string name;
    public int cost;

    //typeOfBuilding : 0 => townCenter ; 1 => Farm ; 2 => LumberCamp ; 3 => Mine 
    public uint typeOfBuilding;

    public Building(string name, int cost)
    {
        this.name = name;
        this.cost = cost;
    }   
}
