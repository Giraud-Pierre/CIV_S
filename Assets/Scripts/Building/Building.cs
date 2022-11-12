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


    public int quantityRessourceEachTurn;
    public Hex hex; //Case du bâtiment
    public Queue<Unit> queue = new Queue<Unit>(); //Liste des unités pour le centre ville

    //Constructeur
    public Building(int buildingType, BuildingPokedex pokedex, Hex newHex)
    {
        this.type = buildingType;
        this.name = pokedex.buildings[type].name;
        this.cost = pokedex.buildings[type].cost;
        this.turnsToBuild = pokedex.buildings[type].turnsToBuild;
        this.turnsRemainingUntilBuildIsComplete = this.turnsToBuild;
        this.isBuilt = false;
        this.hex = newHex;
        

        
        quantityRessourceEachTurn = pokedex.buildings[type].quantityRessourceEachTurn;
        hex = newHex;
    }
    
    public void DoTurn(HexMap hexmap)
    {
        if(this.isBuilt)
        {
            
			if(type != 0) //Si ce n'est pas un centre ville, alors c'est un bâtiment de ressource.
			{
				hexmap.AddRessource(type - 1, quantityRessourceEachTurn);
			}
			else
			{
				if(queue != null && queue.Count > 0)
				{
					hexmap.SpawnUnitAt(queue.Peek(), hexmap.worker, hex.Q, hex.R);
                    queue.Dequeue();
				}
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
