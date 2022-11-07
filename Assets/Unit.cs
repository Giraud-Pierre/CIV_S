using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public string name = "Dwarf";
    public int hitPoint = 100;
    public int strength = 8;
    public int movement = 2;
    public int movementRemaining = 2;
    public Hex hex { get; protected set; }
    public void SetHex(Hex hex)
    {
        if(hex != null)
        {
            hex.RemoveUnit(this);
        }
        this.hex = hex;
        hex.AddUnit(this);
    }

    public void DoTurn()
    {
        //Do queued move ?
    }
}
