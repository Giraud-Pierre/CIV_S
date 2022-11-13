using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    [SerializeField] GameObject hexTile;
    [SerializeField] GameObject canvas;

    [SerializeField] Mesh MeshWater;
    [SerializeField] Mesh MeshFlat;
    [SerializeField] Mesh MeshHill;
    [SerializeField] Mesh MeshMountain;

    [SerializeField] GameObject ForestPrefab;

    [SerializeField] Material MatOcean;
    [SerializeField] Material MatPlains;
    [SerializeField] Material MatMountains;
    [SerializeField] Material MatGrassLands;
    [SerializeField] Material MatDesert;

    [SerializeField] BuildingPokedex buildingPokedex;
    [SerializeField] UnitPokedex unitPokedex;

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

    //***************Dictionnaires et Donnees du jeu******************
    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    private HashSet<Unit> units;
    private Dictionary<Unit, GameObject> unitToGameObjectMap;
    private Dictionary<GameObject, Unit> gameObjectToUnitMap;

    private HashSet<Building> buildings;
    private Dictionary<Building, GameObject> buildingToGameObjectMap;


    private static HexMap hexMapInstance;

    private Node[,] pathfindingGraph;

    private List<int> ressources;
    private GameObject hexGO;
    private GameObject selectedGameObject;
    private int numberOfTurn;
    //****************************************************************

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        if(hexMapInstance == null)
        {
            //Ressources de base
            ressources = new List<int>();
            ressources.Add(300);
            ressources.Add(300);
            ressources.Add(300);

            numberOfTurn = 1;

            GenerateMap();
            hexMapInstance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }


    }
    void Start()
    {

    }

    void Update()
    {
        //TESTING : Press spacebar to advance to next turn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoTurn();
        }
    }

    public GameObject GetPrefabBuilding(int typeOfBuilding)
    {
        return buildingPokedex.buildings[typeOfBuilding].prefab;
    }
    public GameObject GetPrefabUnit(int typeOfUnit)
    {
        return unitPokedex.units[typeOfUnit].Prefab;
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
            //Debug.LogError("GetHexAt: " + x + "," + y);
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

    public void SpawnUnitAt(int unitType, int q, int r)
    {
        if(units == null)
        {
            units = new HashSet<Unit>();
            unitToGameObjectMap = new Dictionary<Unit, GameObject>();
            gameObjectToUnitMap = new Dictionary<GameObject, Unit>();
        }

        GameObject prefab = GetPrefabUnit(unitType);

        if (
                ressources[0] > unitPokedex.units[unitType].Cost[0] && 
                ressources[1] > unitPokedex.units[unitType].Cost[1] &&
                ressources[2] > unitPokedex.units[unitType].Cost[2]) //Cout de l'unité.
        {
            Hex myHex = hexes[q, r];
            GameObject myHexGO = hexToGameObjectMap[myHex];

            Unit unit;

            unit = new Unit(unitPokedex, unitType, myHex);
            

            GameObject unitGO = Instantiate(prefab, myHexGO.transform.position, Quaternion.identity, myHexGO.transform);

            UnitView unitView = unitGO.GetComponent<UnitView>();
            unit.OnUnitMoved += unitView.OnUnitMoved;
            unitView.unit = unit;
            unitView.hexMap = this;
            unitView.hex = myHex;

            myHex.AddUnit(unit);

            units.Add(unit);
            unitToGameObjectMap[unit] = unitGO;
            gameObjectToUnitMap[unitGO] = unit;

            AddRessource(0, -1 * unitPokedex.units[unitType].Cost[0]);
            AddRessource(1, -1 * unitPokedex.units[unitType].Cost[1]);
            AddRessource(2, -1 * unitPokedex.units[unitType].Cost[2]);
        }

        
    }

    public void DestroyUnit(Unit unit)
    {
        units.Remove(unit);
        unit.hex.RemoveUnit(unit);
        GameObject unitObject = unitToGameObjectMap[unit];
        gameObjectToUnitMap.Remove(unitObject);
        Destroy(unitObject);
        unitToGameObjectMap.Remove(unit);

    }

    public void build(int typeOfBuilding)
    {
        Hex hex = selectedGameObject.GetComponent<UnitView>().hex;
        if(buildings == null)
        {
            buildings = new HashSet<Building>();
            buildingToGameObjectMap = new Dictionary<Building, GameObject>();
        }
        bool canBuildOnHex = false;
        if(typeOfBuilding == 3)
        {
            canBuildOnHex = hex.CanBuild(typeOfBuilding, getHexesWithinRangeOf(hex, 1));
        }
        else
        {
            canBuildOnHex = hex.CanBuild(typeOfBuilding);
        }
        if(canBuildOnHex)
        {
            int foodCost = buildingPokedex.buildings[typeOfBuilding].cost[0];
            int woodCost = buildingPokedex.buildings[typeOfBuilding].cost[1];
            int stoneCost = buildingPokedex.buildings[typeOfBuilding].cost[2];

            if (ressources[0] >=  foodCost && ressources[1] >= woodCost && ressources[2] >= stoneCost)
            {
                AddRessource(0,-1 * foodCost);
                AddRessource(1, -1 * woodCost);
                AddRessource(2, -1 * stoneCost);
                
                Building building = new Building(typeOfBuilding, buildingPokedex, hex);
                buildings.Add(building);
                hex.addBuilding(building);
            }
        }
        
    }

    public void buildComplete(Building build)
    {
        Hex hex = build.hex;
        int typeOfBuilding = build.type;

        GameObject hexGO = GetHexeGameobjectFromDictionnary(hex);
        Vector3 p = hexGO.transform.position;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 210, 0));
        if (hex.Elevation >= HeightHill)
        {
            p.y += 0.246f;
        }

        if (hex.getTypeOfField() == 4)
        {
            p.x -= 0.048f;
            p.z -= 0.425f;
        }

        if (typeOfBuilding == 0)
        {
            rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
        else if (typeOfBuilding == 1)
        {
            rotation = Quaternion.Euler(new Vector3(0, 160, 0));
        }
        GameObject buildingGO = Instantiate(GetPrefabBuilding(build.type), p, rotation, hexGO.transform);
        buildingToGameObjectMap[build] = buildingGO;
    }

    public void AddRessource(int ressourceType, int quantity)
    {
        ressources[ressourceType] += quantity;
        canvas.GetComponent<GameMenuController>().UpdateResources(ressources);
    }

    public List<int> GetRessource()
    {
        return ressources;
    }

    public void DoTurn()
    {
        mouseController.GetComponent<MouseController>().UnselectAtEndTurn();
        selectedGameObject = null;
        numberOfTurn += 1;
        canvas.GetComponent<GameMenuController>().UpdateNumberOfTurn(numberOfTurn);
        canvas.GetComponent<GameMenuController>().GetDefaultMenu();

        if (units != null)
        {
            Debug.Log(units.Count);
            foreach (Unit unit in units)
            {
                unit.DoTurn();
            }
            foreach (Unit unit in units)
            {
                Debug.Log("unity check Destroy");
                if (unit.GetHitPoint() < 0)
                {
                    Debug.Log("Le {0} est mort");
                    DestroyUnit(unit);
                }
            }
            
        }
        if(buildings != null)
        {
            foreach(Building building in buildings)
            {
                building.DoTurn(this);
            }
        }
        if(numberOfTurn % 10 == 0)
        {
            List<int> targetPosition = LoofForValidPositionForEnnemy(units.First().hex);

            SpawnUnitAt(2, targetPosition[0], targetPosition[1]);
        }
    }

    public void ChangeSelectedObject(GameObject selectedObject)
    {
        selectedGameObject = selectedObject;
    }

    public int GetNumberOfTurn()
    {
        return numberOfTurn;
    }

    public GameObject GetGameobjectFromUnit( Unit unit)
    {
        return unitToGameObjectMap[unit];
    }


    private List<int> LoofForValidPositionForEnnemy(Hex centerHex)
    {
        //Cherche une case valide pour faire spawner les ennemis.
        //Une vase valide est walkable et possède au moins 3 cases voisines
        //qui sont aussi walkable (pour éviter de spawner dans une île)
        int row;
        int col;
        Hex hex;
        Hex[] neighbourHex;
        int numberOfWalkableNeighbours;
        bool validPosition = false;
        do
        {
            //Choisi une case au hasard
            do
            {
                col = centerHex.Q + Random.Range(-5, 5);
                row = centerHex.R + Random.Range(-5, 5);
            } while (col > numberColumns || col < 0 || row > numberRows || row < 0);
            hex = getHexeAt(col,  row);

            if (hex != null && hex.iswalkable) //Vérifie si la case choisie au hasard est walkable
            {
                //On récupère les cases voisines
                numberOfWalkableNeighbours = 0;
                neighbourHex = getHexesWithinRangeOf(hex, 1);
                //On regarde combien de cases voisines sont walkable
                foreach (Hex currentHex in neighbourHex)
                {
                    if (currentHex != null && currentHex.iswalkable)
                    {
                        numberOfWalkableNeighbours += 1;
                    }
                }
                if (numberOfWalkableNeighbours > 2)
                {
                    //S'il y a au moins 3 voisins walkable, on a une position valide
                    validPosition = true;
                }
            }

        } while (!validPosition);

        List<int> result = new List<int>();
        result.Add(col);
        result.Add(row);

        return result;
    }

}
