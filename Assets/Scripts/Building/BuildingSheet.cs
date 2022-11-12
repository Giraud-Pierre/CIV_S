using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingSheet : ScriptableObject
{
    public new string name;
    public int[] cost = new int[3];
    public int turnsToBuild;
}
