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

    void Update()
    {
        //TESTING : Press spacebar to advance to next turn
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mouseController.GetComponent<MouseController>().UnselectAtEndTurn();
            if (units != null)
            {
                foreach(Unit unit in units)
                {
                    unit.DoTurn();
                }
            }
        }
    }

    [SerializeField] GameObject hexTile;

    [SerializeField] Mesh MeshWater;
    [SerializeField] Mesh MeshFlat;
    [SerializeField] Mesh MeshHill;
    [SerializeField] Mesh MeshMountain;

    [SerializeField] GameObject ForestPrefab;
    [SerializeField] GameObject JunglePrefab;

    [SerializeField] Material MatOcean;
    [SerializeField] Material MatPlains;
    [SerializeField] Material MatMountains;
    [SerializeField] Material MatGrassLands;
    [SerializeField] Material MatDesert;

    [SerializeField] GameObject mouseController;

    public GameObject UnitDwarfPrefab;

    protected float HeightMountain = 0.85f;
    protected float HeightHill = 0.6f;
    protected float HeightFlat = 0.0f;
    public bool allowWrapEastWest = true;
    public bool allowWrapNorthSouth = false;

    protected float MoistureJungle = 0.66f;
    protected float MoistureForest = 0.33f;
    protected float MoistureGrasslands = 0f;
    protected float MoisturePlains = -0.5f;


    public int numberRows = 30;
    public int numberColumns = 60; 
    private GameObject hexGO;

    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    private HashSet<Unit> units;
    private Dictionary<Unit, GameObject> unitToGameObjectMap;

    public Hex getHexeAt(int x, int y)
    {
        if(hexes == null)
        {
            //throw new UnityException("Hexes array not yet extentiated !");
            Debug.Log("Hexes array not yet extentiated !");
            return null;
        }

        if(allowWrapEastWest)
        {
            x = x % numberColumns;
            if (x < 0)
            {
                x += numberColumns;
            }
        }
        
        /*if (y < 0)
        {
            y += numberColumns;
        }*/
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
    public GameObject GetHexeGameobjectFromDictionnary(Hex hex)
    {
        return hexToGameObjectMap.ContainsKey(hex) ? hexToGameObjectMap[hex] : null;
    }

    public Vector3 GetHexPosition(int q, int r)
    {
        Hex h = getHexeAt(q, r);
        return GetHexPosition(h);
    }

    public Vector3 GetHexPosition(Hex hex)
    {
        return hex.PositionFromCamera(Camera.main.transform.position, numberRows, numberColumns);
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
                Hex h = new Hex(this, column, row);
                Vector3 pos = h.PositionFromCamera(Camera.main.transform.position, numberRows, numberColumns);
                h.Elevation = -0.5f;
                h.Moisture = 0f;
                hexes[column, row] = h;

                

                hexGO = (GameObject)Instantiate(hexTile, pos, Quaternion.identity, this.transform);
                HexComponent hexComponenent = hexGO.GetComponent<HexComponent>();
                hexComponenent.hex = h;
                hexComponenent.hexMap = this;
                hexToGameObjectMap[h] = hexGO;

                //********Debug pour afficher les colonnes et lignes des cases
                //hexComponenent.SetRowCol(row, column);
                //********************

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
                //Debug.Log("Column :" + column);
                //Debug.Log("Row :" + row);
                Hex h = hexes[column, row];
                GameObject hexGO = hexToGameObjectMap[h];
                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();

                if(h.Elevation >= HeightFlat)
                {
                    if (h.Moisture >= MoistureJungle)
                    {
                        mr.material = MatGrassLands;
                        Vector3 p = hexGO.transform.position;
                        if(h.Elevation >= HeightHill)
                        {
                            p.y += 0.25f;
                        }
                        //TODO : Spawn Jungle
                        GameObject.Instantiate(JunglePrefab, p, Quaternion.identity, hexGO.transform);
                    }


                    else if (h.Moisture >= MoistureForest)
                    {
                        mr.material = MatGrassLands;
                        Vector3 p = hexGO.transform.position;
                        if (h.Elevation >= HeightHill)
                        {
                            p.y += 0.25f;
                        }
                        GameObject.Instantiate(ForestPrefab, p, Quaternion.identity, hexGO.transform);
                    }

                    else if (h.Moisture >= MoistureGrasslands)
                    {
                        mr.material = MatGrassLands;
                    }

                    else if (h.Moisture >= MoisturePlains)
                    {
                        mr.material = MatPlains;

                    }

                    else
                    {
                        mr.material = MatDesert;
                    }
                }

                if (h.Elevation >= HeightMountain)
                {
                    mr.material = MatMountains;
                    mf.mesh = MeshMountain;
                }

                else if (h.Elevation >= HeightHill)
                {
                    mf.mesh = MeshHill;
                }

                else if (h.Elevation >= HeightFlat)
                {
                    mf.mesh = MeshWater;
                }

                else
                {
                    mr.material = MatOcean;
                    mf.mesh = MeshWater;
                }
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

    public void SpawnUnitAt(Unit unit, GameObject prefab, int q, int r)
    {
        if(units == null)
        {
            units = new HashSet<Unit>();
            unitToGameObjectMap = new Dictionary<Unit, GameObject>();
        }


        Hex myHex = getHexeAt(q, r);
        GameObject myHexGO = hexToGameObjectMap[myHex];
        unit.SetHex(myHex);
        Hex[] hexPath = new Hex[0];
        unit.SetHexPath(hexPath);
        GameObject unitGO = Instantiate(prefab, myHexGO.transform.position, Quaternion.identity, myHexGO.transform);

        UnitView unitView = unitGO.GetComponent<UnitView>();
        unit.OnUnitMoved += unitView.OnUnitMoved;
        unitView.unit = unit;
        unitView.hexMap = this;
        unitView.hex = myHex;

        units.Add(unit);
        unitToGameObjectMap[unit] = unitGO;
    }

}
