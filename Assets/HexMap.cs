using JetBrains.Annotations;
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
    [SerializeField] Material[] HexMaterials;

    [SerializeField] Mesh MeshWater;
    [SerializeField] Mesh MeshFlat;
    [SerializeField] Mesh MeshWill;
    [SerializeField] Mesh MeshMountain;

    [SerializeField] Material MatOcean;
    [SerializeField] Material MatPlains;
    [SerializeField] Material MatMountains;
    [SerializeField] Material MatGrassLands;
    public int numberRows = 30;
    public int numberColumns = 60; 
    private GameObject hexGO;
    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    public Hex getHexeAt(int x, int y)
    {
        if(hexes == null)
        {
            //throw new UnityException("Hexes array not yet extentiated !");
            Debug.Log("Hexes array not yet extentiated !");
            return null;
        }
        if (x < 0)
        {
            x += numberRows;
        }

        if (y < 0)
        {
            y += numberColumns;
        }
        try
        {
            return this.hexes[x, y];
        }
        catch
        {
            Debug.LogError("GetHexAt: " + x + "," + y);
            return null;
        }
        
    }

    virtual public void GenerateMap()
    {
        hexes = new Hex[numberColumns, numberRows];
        hexToGameObjectMap = new Dictionary<Hex, GameObject>();
        for (int column = 0; column< numberColumns;column++)
        {
            for (int row =0; row<numberRows; row++ )
            {
                //Instantiate a Hex
                Hex h = new Hex(column, row);
                Vector3 pos = h.PositionFromCamera(Camera.main.transform.position, numberRows, numberColumns);
                h.Elevation = -1;
                hexes[column, row] = h;

                

                hexGO = (GameObject)Instantiate(hexTile, pos, Quaternion.identity, this.transform);
                hexGO.GetComponent<HexComponent>().hex = h;
                hexGO.GetComponent<HexComponent>().hexMap = this;
                hexToGameObjectMap[h] = hexGO;

                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);
                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                mr.material = MatOcean;

                MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
                mf.mesh = MeshWater;
            }
            
        }
        UpdateHexVisuals();

        //StaticBatchingUtility.Combine(this.gameObject);

        
    }

    public void UpdateHexVisuals()
    {
        for (int column = 0; column < numberColumns; column++)
        {
            for (int row = 0; row < numberRows; row++)
            {
                Debug.Log("Column :" + column);
                Debug.Log("Row :" + row);
                Hex h = hexes[column, row];
                GameObject hexGO = hexToGameObjectMap[h];
                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                if(h.Elevation >= 0)
                {
                    mr.material = MatGrassLands;
                }

                else
                {
                    mr.material = MatOcean;
                }

                MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
                mf.mesh = MeshWater;
            }
        }
    }

    public  Hex[] getHexesWithinRangeOf(Hex centerHex, int range)
    {
        List<Hex> results = new List<Hex>();
        for(int dx = -range; dx < range-1; dx++)
        {
            for(int dy = Mathf.Max(-range+1, -dx-range); dy < Mathf.Min(range, -dx+range-1); dy++)
            {
                results.Add(getHexeAt(centerHex.Q +dx, centerHex.R +dy));
            }
        }

        return results.ToArray();
    }

    

}
