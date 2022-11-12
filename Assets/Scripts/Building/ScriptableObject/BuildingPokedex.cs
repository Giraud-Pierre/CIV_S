using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingPokedex : ScriptableObject
{
    //Liste de toutes les fiches de bâtiments
    public List<BuildingSheet> buildings = new List<BuildingSheet>();
}
