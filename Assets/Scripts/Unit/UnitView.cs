using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Linq;

public class UnitView : MonoBehaviour,IClick
{
    public Unit unit;       //TODO: eventuellement les mettre en privé et faire des méthode pour les changer et y accèder
    public HexMap hexMap;
    public Hex hex;

    private Animator animator; //Où changer l'animation
    [SerializeField] private AnimatorController idle;
    [SerializeField] private AnimatorController walking;
    [SerializeField] private AnimatorController mining;
    [SerializeField] private GameObject sledgeHammer;
    [SerializeField] private GameObject selectedCylinder;
    private bool isMining = false;
    private bool isMoving = false;

    Vector3 newPosition;
    Vector3 currentVelocity;
    float smoothTime = 0.5f;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        newPosition = this.transform.position;
        animator.runtimeAnimatorController = idle;
        sledgeHammer.SetActive(false);
        selectedCylinder.SetActive(false);
    }

    public void OnUnitMoved(Hex oldHex, Hex nexHex)
    {
        //Animate the unit moving from oldHex to newHex
        this.transform.position = oldHex.PositionFromCamera();
        newPosition = nexHex.PositionFromCamera();
        currentVelocity = Vector3.zero;

        hex = nexHex;

        isMoving = true;
        animator.runtimeAnimatorController = walking;

        if(Vector3.Distance(this.transform.position,newPosition) > 2)
        {
            //This OnUNitMoved is considerably more than the expected move
            //between two adjacent tiles -- it's probably a map seam thing 
            // So just teleport
            this.transform.position = newPosition;

        }
    }

    public void StopMoving()
    {
        isMoving = false;
        animator.runtimeAnimatorController = idle;
        gameObject.transform.parent = hexMap.GetHexeGameobjectFromDictionnary(hex).transform; //Change le parent du gameobject de l'unité
    }

    public void OnLeftClickAction()
    {
        //*************TODO: sélectionne le personnage (Change UI, surbrillance, surbrillance des hexagone de sa liste hexpath)
        selectedCylinder.SetActive(true);
        if(unit.hexPath != null && unit.hexPath.Count != 0)
        {
            foreach(Hex hex in unit.hexPath)
            {
                hexMap.GetHexeGameobjectFromDictionnary(hex).GetComponent<HexComponent>().HighlightHexagon();
            }
        }
    }

    public void OnLeftClickOnOtherAction()
    {
        selectedCylinder.SetActive(false);
        if (unit.hexPath != null && unit.hexPath.Count != 0)
        {
            foreach (Hex hex in unit.hexPath)
            {
                hexMap.GetHexeGameobjectFromDictionnary(hex).GetComponent<HexComponent>().UnHighlightHexagon();
            }
        }
    }

    public void OnRightClickAction(GameObject gameobject)
    {
        if(gameobject.tag == "Hexagon")
        {
            HexComponent targetHexComponent = gameobject.GetComponent<HexComponent>();
            if (Hex.Distance(hex,targetHexComponent.hex) <= 1)
            {
                unit.AddToHexPath(targetHexComponent.hex);
                targetHexComponent.HighlightHexagon();
            }
            else
            {
                //Pathfinding
                List<HexMap.Node> path = DijkstraPathfinding(hexMap.GetPathFindingGraph(), hex, targetHexComponent.hex);
                foreach(HexMap.Node node in path)
                {
                    unit.AddToHexPath(node.hex);
                    hexMap.GetHexeGameobjectFromDictionnary(node.hex).GetComponent<HexComponent>().HighlightHexagon();
                }
                
            }
            //*************TODO: vérifier si la case est adjacente ou s'il va falloir calculer un chemin (et si la case est disponible (!= eau ou ennemi))
            
        }
        //*****************TODO: else if(gameobject.tag == "Ennemy") { DoAttack() }
    }

    private List<HexMap.Node> DijkstraPathfinding(HexMap.Node[,] pathFindingGraph, Hex unitHex, Hex targetHex)
    {
        HexMap.Node source = pathFindingGraph[unitHex.Q, unitHex.R];
        HexMap.Node target = pathFindingGraph[targetHex.Q,targetHex.R];

        Dictionary<HexMap.Node, float> distance = new Dictionary<HexMap.Node, float>();
        Dictionary<HexMap.Node, HexMap.Node> previousNodes = new Dictionary<HexMap.Node, HexMap.Node>();

        List<HexMap.Node> unvisitedNodesQueue = new List<HexMap.Node>();

        //Creates the dictionnaries and set the list of unvisited nodes
        distance[source] = 0;
        previousNodes[source] = null;
        foreach (HexMap.Node v in pathFindingGraph)
        {
            if (v != source)
            {
                distance[v] = Mathf.Infinity;
                previousNodes[v] = null;
            }
            unvisitedNodesQueue.Add(v);
        }


        while (unvisitedNodesQueue.Count > 0)
        {

            //Le noeud u va être le unvisited node avec la distance la plus faible.
            HexMap.Node u = null;

            foreach(HexMap.Node nextU in unvisitedNodesQueue)
            {
                if(u == null || distance[nextU] < distance[u])
                {
                    u = nextU;
                }
            }
            
            if(u == target)
            {
                break; //sort de la boucle While
            }

            unvisitedNodesQueue.Remove(u);

            foreach (HexMap.Node v in u.neighbours)
            {
                float alt = distance[u] + u.Distance(v);
                if (alt < distance[v])
                {
                    distance[v] = alt;
                    previousNodes[v] = u;
                }
            }
        }
        //Soit on a trouvé le chemin le plus court, soit il n'y a pas de chemin
        if (previousNodes[target] == null)
        {
            //pas de route
            return null;
        }
        else
        {
            List<HexMap.Node> path = new List<HexMap.Node>();
            HexMap.Node current = target;
            while (current != null)
            {
                path.Add(current);
                current = previousNodes[current];
            }
            path.Reverse();
            return path;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, newPosition, ref currentVelocity, smoothTime);
            if(Vector3.Distance(this.transform.position, newPosition) <= 0.1f)
            {
                StopMoving();
            }
        }
    }
}
