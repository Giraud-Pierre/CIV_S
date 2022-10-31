using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    [SerializeField] GameObject hexTile;

    public void GenerateMap()
    {
        for (int column = 0; column< 10;column++)
        {
            for (int row =0; row<10; row++ )
            {
                Hex h = new Hex(column, row);
                Instantiate(hexTile, new Vector3(column, 0, row), Quaternion.identity, this.transform);
            }
        }
    }
    
}
