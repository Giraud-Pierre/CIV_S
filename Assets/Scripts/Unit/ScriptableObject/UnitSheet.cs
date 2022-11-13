using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitSheet : ScriptableObject
{
    //Charactéristiques de l'unité
    public new string name;
    public int strength;
    public int movement;
    public List<int> Cost;
    public GameObject Prefab;
}
