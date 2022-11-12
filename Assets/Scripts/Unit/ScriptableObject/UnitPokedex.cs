using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitPokedex : ScriptableObject
{
    //Regroupe les différentes fiches de charactéristiques de l'unité.
    public List<UnitSheet> units = new List<UnitSheet>();
}

