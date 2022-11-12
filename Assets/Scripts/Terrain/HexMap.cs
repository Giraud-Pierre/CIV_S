using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Ressources de base
        ressources = new List<int>();
        ressources.Add(100);
        ressources.Add(200);
        ressources.Add(200);

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
            if(buildings != null)
            {
                foreach(Building building in buildings)
                {
                    building.DoTurn(this);
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

    [SerializeField] GameObject TownCenter;
    [SerializeField] GameObject Mine;
    [SerializeField] GameObject LumberCamp;
    [SerializeField] GameObject Farm;

    [SerializeField] Material MatOcean;
    [SerializeField] Material MatPlains;
    [SerializeField] Material MatMountains;
    [SerializeField] Material MatGrassLands;
    [SerializeField] Material MatDesert;

    [SerializeField] BuildingPokedex buildingPokedex;

    [SerializeField] GameObject mouseController;

    public GameObject worker;

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


    //***************Dictionnaires et Données du jeu******************
    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;
    private Dictionary<GameObject, Hex> gameObjectToHexMap;

    private HashSet<Unit> units;
    private Dictionary<Unit, GameObject> unitToGameObjectMap;
    private Dictionary<GameObject, Unit> gameObjectToUnitMap;

    private HashSet<Building> buildings;
    private Dictionary<Building, GameObject> buildingToGameObjectMap;


    

    private Node[,] pathfindingGraph;

    private List<int> ressources;
    //****************************************************************


    public GameObject GetFarmGO()
    {
        return Farm;
    }

    public GameObject GetMineGO()
    {
        return Mine;
    }

    public GameObject GetTownCenterGO()
    {
        return TownCenter;
    }

    public GameObject GetLumberCampGO()
    {
        return LumberCamp;
    }

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
        else if( x < 0 || x >= numberColumns)
        {
            Debug.LogError("GetHexAt: " + x + "," + y);
            return null;
        }

        if (allowWrapNorthSouth)
        {
            y = y % numberRows;
            if (y < 0)
            {
                y += numberRows;
            }
        }
        else if (y < 0 || y >= numberRows)
        {
            Debug.LogError("GetHexAt: " + x + "," + y);
            return null;
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
    public GameObject GetHexeGameobjectFromDictionnary(Hex hex)
    {
        return hexToGameObjectMap.ContainsKey(hex) ? hexToGameObjectMap[hex] : null;
    }

    

    public Hex GetHexFromDictionnary(GameObject gameObject)
    {
        return gameObjectToHexMap.ContainsKey(gameObject) ? gameObjectToHexMap[gameObject] : null;
    }

    public Unit GetUnitFromDictionnary(GameObject gameObject)
    {
        return gameObjectToUnitMap.ContainsKey(gameObject) ? gameObjectToUnitMap[gameObject] : null;
    }

    public Vector3 GetHexPosition(int q, int r)
    {
        Hex h = hexes[q, r];
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
                    h.iswalkable = true;
                    if (h.Moisture >= MoistureForest && h.Elevation < HeightMountain)
                    {
                        mr.material = MatGrassLands;
                        Vector3 p = hexGO.transform.position;
                        if (h.Elevation >= HeightHill)
                        {
                            p.y += 0.25f;
                        }
                        GameObject.Instantiate(ForestPrefab, p, Quaternion.identity, hexGO.transform);
                        h.SetTypeOfField(4);
                    }

                    else if (h.Moisture >= MoistureGrasslands)
                    {
                        mr.material = MatGrassLands;
                        h.SetTypeOfField(1);
                    }

                    else if (h.Moisture >= MoisturePlains)
                    {
                        mr.material = MatPlains;
                        h.SetTypeOfField(1);

                    }

                    else
                    {
                        h.SetTypeOfField(2);
                        mr.material = MatDesert;
                    }
                }

                if (h.Elevation >= HeightMountain)
                {
                    h.iswalkable = false;
                    mr.material = MatMountains;
                    mf.mesh = MeshMountain;
                    h.SetTypeOfField(3);
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

    public Hex[] getHexesWithinRangeOf(Hex centerHex, int range)
    {
        List<Hex> results = new List<Hex>();
        for(int dx = -range; dx <= range; dx++)
        {
            for(int dy = Mathf.Max(-range, -dx-range); dy <= Mathf.Min(range, -dx+range); dy++)
            {
                Hex neighbour = getHexeAt(centerHex.Q + dx, centerHex.R + dy);
                if (neighbour != null && neighbour != centerHex)
                {
                    results.Add(neighbour);
                }
                
            }
        }

        return results.ToArray();
    }

    public class Node{
        public List<Node> neighbours;
        public Hex hex;

        public Node()
        {
            neighbours = new List<Node>();
        }
        public float Distance(Node n)
        {
            return Hex.Distance(hex,n.hex);
        }
    }

    public void GeneratePathfindingGraph()
    {
        pathfindingGraph = new Node[numberColumns, numberRows];

        for (int column = 0; column < numberColumns; column++)
        {
            for (int row = 0; row < numberRows; row++)
            {
                pathfindingGraph[column, row] = new Node();
            }
        }

        for (int column = 0; column < numberColumns; column++)
        {
            for (int row =0; row < numberRows; row++)
            {
                Hex newHex = hexes[column, row];
                pathfindingGraph[column, row].hex = newHex;

                Hex[] neighbours = getHexesWithinRangeOf(newHex, 1);
                foreach (Hex hex in neighbours)
                {
                    if (hex.iswalkable) 
                    {
                        pathfindingGraph[column, row].neighbours.Add(pathfindingGraph[hex.Q, hex.R]);
                    }
                }
            }
        }
    }

    public Node[,] GetPathFindingGraph()
    {
        return pathfindingGraph;
    }

    public void SpawnUnitAt(Unit unit, GameObject prefab, int q, int r)
    {
        if(units == null)
        {
            units = new HashSet<Unit>();
            unitToGameObjectMap = new Dictionary<Unit, GameObject>();
        }

        if (ressources[0] > 50) //Coût de l'unité.
        {
            Hex myHex = hexes[q, r];
            GameObject myHexGO = hexToGameObjectMap[myHex];
            unit.SetHex(myHex);
            unit.SetHexPath(new Hex[0]);
            GameObject unitGO = Instantiate(prefab, myHexGO.transform.position, Quaternion.identity, myHexGO.transform);

            UnitView unitView = unitGO.GetComponent<UnitView>();
            unit.OnUnitMoved += unitView.OnUnitMoved;
            unitView.unit = unit;
            unitView.hexMap = this;
            unitView.hex = myHex;

            units.Add(unit);
            unitToGameObjectMap[unit] = unitGO;
            gameObjectToUnitMap[unitGO] = unit;

            ressources[0] -= 50;
        }

        
    }

    public void AddRessource(int ressourceType, int quantity)
    {
        ressources[ressourceType] += quantity;
    }

    public List<int> GetRessource()
    {
        return ressources;
    }

}
