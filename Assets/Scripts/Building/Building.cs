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

    public int quantityRessourceEachTurn;
    public Hex hex; //Case du bâtiment
    public List<Unit> queue = new List<Unit>(); //Liste des unités pour le centre ville

    //Constructeur
    public Building(int buildingType, BuildingPokedex pokedex, Hex newHex)
    {
        this.type = buildingType;
        this.name = pokedex.buildings[type].name;
        this.cost = pokedex.buildings[type].cost;
        quantityRessourceEachTurn = pokedex.buildings[type].quantityRessourceEachTurn;
        hex = newHex;
    }
    
    public void DoTurn(HexMap hexmap)
    {
        if(type != 0) //Si ce n'est pas un centre ville, alors c'est un bâtiment de ressource.
        {
            hexmap.AddRessource(type - 1, quantityRessourceEachTurn);
        }
        else
        {
            if(queue != null && queue.Count > 0)
            {
                hexmap.SpawnUnitAt(queue[0].unitType, hexmap.worker, hex.Q, hex.R);
                queue.RemoveAt(0);
            }
        }
    }
}
