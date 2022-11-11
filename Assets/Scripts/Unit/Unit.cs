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
    Queue<Hex> hexPath;

    //TODO : THis should probably be move to some kind of central option/config file
    const bool MOVEMENT_RULES_LIKE_CIV6 = false;
    public Hex hex { get; protected set; }


    public delegate void  UnitMovedDelegate(Hex oldHex, Hex newHex);

    public event UnitMovedDelegate OnUnitMoved;

    public void SetHexPath(Hex[] hexPath)
    {
        this.hexPath = new Queue<Hex>(hexPath);
    }

    public void AddToHexPath(Hex hexpath)
    {
        this.hexPath.Enqueue(hexpath);
    }

    public void SetHex(Hex newHex)
    {
        Hex oldHex = hex;

        if(hex != null)
        {
            hex.RemoveUnit(this);
        }
        this.hex = newHex;
        hex.AddUnit(this);

        if(OnUnitMoved != null)
        {
            OnUnitMoved(oldHex, newHex);
        }
    }

    public void DoTurn()
    {
        //Do queued move ?
        if(hexPath != null ||hexPath.Count ==0)
        {
            return;
        }
        else
        {
            while(movementRemaining > 0)
            {
                //Grab the first hex from our queu
                Hex newHex = hexPath.Dequeue();
                movementRemaining -= MovementCostToEnterHex(newHex);

                if(movementRemaining >= 0)
                {
                    //Move to the new Hex
                    SetHex(newHex);
                }
                else
                {
                    //If remaining movement insufficent, do not move and requeue the movement for next turn
                    hexPath.Enqueue(newHex);
                }
            }
            movementRemaining = movement;
        }
        

    }

    public int MovementCostToEnterHex(Hex hex)
    {
        //TODO : Override base movement ost based on 
        // our movement mode + tile type
        return hex.BaseMovementCost();
    }

    public float AggregateTurnToEnterHex(Hex hex, float turnsToDate)
    {
        //The issue at hand is that if you are trying to enter a tile
        // with a movement cost greater than your current remaining movement
        // points, this will either result in a cheaper-than expected
        // turn cost (Civ5) or a more-expensive-than expected turn cost (Civ6)
        float baseTurnstoEnterHex = MovementCostToEnterHex(hex) / movement; // Example : entering a forest is "1" turn
        float turnsRemaining = movementRemaining / movement; // Example if we are at 1/2 move, than we have 0.5 turn left
        
        float turnsToDateWhole = Mathf.Floor(turnsToDate); // Example 4.33 becomes 4
        float turnsToDateFraction = turnsToDate - turnsToDateWhole; //Example : 4.33 becomes 0.33

        if(turnsToDateFraction < 0.01f ||turnsToDateFraction > 0.99f)
        {
            Debug.LogError("Looks like we've got floating-point drift");

            if(turnsToDateFraction < 0.01f)
            {
                turnsToDateFraction = 0;
            }

            if(turnsToDateFraction > 0.99f)
            {
                turnsToDateWhole += 1;
                turnsToDateFraction = 0; 
            }
        }

        float turnsUsedAfterThisMove = turnsToDateFraction + baseTurnstoEnterHex; // Example : 0.33 + 1 (to enter the forest)

        if(turnsUsedAfterThisMove > 1)
        {
            //We have the situation where we don't actually have enough movement to complete this move
            //What are we going to do ?
            if(MOVEMENT_RULES_LIKE_CIV6)
            {
                //We aren't allowed to enter the tile this move. That means we have to ...
                // Sit idle for the remainder of this turn
                if(turnsToDateFraction == 0)
                {
                    //We have full movement, but this isn't enough to enter the tile
                    // Example : we have a max move of 2 but the tile costs 3 to enter
                    // we are good to go
                }

                else
                {
                    //We are not on a fresh turn but we nee to
                    // SIT IDLE FOR THE REMAINDER OF THIS TURN
                    turnsToDateWhole += 1;
                    turnsToDateFraction = 0;

                }
                //So now we know for a fact that we are starting the move into difficult terrain on a fresh turn

            }
        
        //Return the total tun cost of turnsToDate + turns for this move
        }
        return 0.2f;
    }

}
