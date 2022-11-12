using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public string name;
    //0 pour un centre ville, 1 pour un moulin, 2 pour une cabane de bucheron, 3 pour une mine
    public int type;

    public int turnsToBuild;
    public int turnsRemainingUntilBuildIsComplete;

    public Boolean isBuilt;

    //Contenu dans BuildingPokedex.buildings[buildingType] avec dans l'ordre nourriture, bois, pierre.
    public int[] cost = new int[3]; 

    public Hex hex; //Case du bâtiment
    public List<Unit> queue = new List<Unit>(); //Liste des unités pour le centre ville

    public Building(int buildingType, BuildingPokedex pokedex, Hex newHex)
    {
        this.type = buildingType;
        this.name = pokedex.buildings[type].name;
        this.cost = pokedex.buildings[type].cost;
        this.turnsToBuild = pokedex.buildings[type].turnsToBuild;
        this.turnsRemainingUntilBuildIsComplete = this.turnsToBuild;
        this.isBuilt = false;
        this.hex = newHex;
    }

    public void DoTurn()
    {
        if(this.isBuilt)
        {
            switch (type)
            {
                case 0:
                    //TODO :if there are units waiting to be created, create a unit
                    //doturn for citycenter
                    break;
                case 1:
                    //TODO :  give 20 of food
                    //doturn for farm
                    break;
                case 2:
                    //TODO : give 20 of wood
                    //doturn for lumber camp
                    break;
                case 3:
                    //TODO :  give 20 of stone
                    //do turn for mine
                    break;

            }
        }

        else
        {
            turnsRemainingUntilBuildIsComplete--;
            if(turnsRemainingUntilBuildIsComplete == 0)
            {
                this.isBuilt = true;
            }
        }

        
    }
}
