using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingSheet : ScriptableObject
{
    public new string name;
    public List<int> cost = new List<int>();
    public int quantityRessourceEachTurn;
}
