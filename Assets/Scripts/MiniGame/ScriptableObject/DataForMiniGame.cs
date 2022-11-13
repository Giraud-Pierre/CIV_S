using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DataForMiniGame : ScriptableObject
{
    public Unit character = null;
    public int damageCharacter = 0;
    public int hitPointCharacter = 0;

    public Unit ennemy = null;
    public int damageEnnemy = 0;
    public int hitPointEnemy = 0;
    public GameObject ennemyPrefab = null;

    public bool isWin = false;

}
