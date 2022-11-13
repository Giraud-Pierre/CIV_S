using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    //Charactéristiques de l'unité
    public string name;
    public int unitType; //0 pour un ouvrier, 1 pour un chasseur
    public int hitPoint;
    public int strength;
    public int movement;
    public int movementRemaining;
    public List<int> cost;
    public Hex hex { get; protected set; }
    public Queue<Hex> hexPath;

    //Fonction déléguée pour le mouvement
    public delegate void UnitMovedDelegate(Hex oldHex, Hex newHex);
    public event UnitMovedDelegate OnUnitMoved;

    //TODO : THis should probably be move to some kind of central option/config file
    const bool MOVEMENT_RULES_LIKE_CIV6 = false;
    
    //Constructeur
    public Unit(UnitPokedex pokedex,int newUnitType, Hex newhex)
    {
        unitType = newUnitType;
        name = pokedex.units[unitType].name;
        hitPoint = 100;
        strength = pokedex.units[unitType].strength;
        movement = pokedex.units[unitType].movement;
        movementRemaining = movement;
        cost = pokedex.units[unitType].Cost;
        hex = newhex;
        SetHexPath(new Hex[0]);
    }

    public Hex getHex()
    {
        return hex;
    }

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
        if(hexPath == null || hexPath.Count ==0)
        {
            return;
        }
        else
        {
            while(movementRemaining > 0 && hexPath.Count != 0)
            {
                //Grab the first hex from our queue
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

    public int GetHitPoint()
    {
        return hitPoint;
    }

    public int InflictDamage( int damage)
    {
        hitPoint -= damage;
        return hitPoint;
    }

}
