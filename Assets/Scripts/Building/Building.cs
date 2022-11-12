using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public string name;
    //0 pour un centre ville, 1 pour un moulin, 2 pour une cabane de bucheron, 3 pour une mine
    public int type;

    //Contenu dans BuildingPokedex.buildings[buildingType] avec dans l'ordre nourriture, bois, pierre.
    public List<int> cost = new List<int>(3); 

    public Hex hex; //Case du bâtiment
    public List<Unit> queue = new List<Unit>(); //Liste des unités pour le centre ville

    public Building(int buildingType, BuildingPokedex pokedex, Hex newHex)
    {
        this.type = buildingType;
        this.name = pokedex.buildings[type].name;
        this.cost = pokedex.buildings[type].cost;
        hex = newHex;
    }

    public void DoTurn()
    {
        switch (type)
        {
            case 0:
                //doturn for citycenter
                break;

        }
    }
}
