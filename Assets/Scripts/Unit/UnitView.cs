using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitView : MonoBehaviour,IClick
{
    public Unit unit;       //TODO: eventuellement les mettre en privé et faire des méthode pour les changer et y accèder
    public HexMap hexMap;
    public Hex hex;

    private Animator animator; //Où changer l'animation
    [SerializeField] private RuntimeAnimatorController idle;
    [SerializeField] private RuntimeAnimatorController walking;
    [SerializeField] private RuntimeAnimatorController mining;
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

            //La référence va être soit la case de l'unité, soit la dernière case dans sa liste de déplacements
            Hex sourceHex = (unit.hexPath.Count == 0)? hex : unit.hexPath.Last(); 

            if (Hex.Distance(sourceHex, targetHexComponent.hex) <= 1) //Si on clic droit sur une case à 1 case de distance, on l'ajoute à la liste
            {
                if (targetHexComponent.hex.iswalkable)
                {
                    unit.AddToHexPath(targetHexComponent.hex);
                    targetHexComponent.HighlightHexagon();
                }
                else
                {
                    Debug.Log("Destination non joignable");
                }
            }
            else //Sinon on trouve la liste de cases qui va permettre de la rejoindre le plus vite possible à partir de la source ("sourceHex")
            {
                //Renvoie le liste de case permettant de rejoindre la cible le plus vite possible.
                List<HexMap.Node> path = DijkstraPathfinding(hexMap.GetPathFindingGraph(), sourceHex, targetHexComponent.hex);

                if(path == null) //Si on ne peut pas rejoindre la case (parce qu'il y a de l'eau ou des montagnes qui l'enclavent)
                {
                    Debug.Log("Destionation non joignable");
                }
                else //Sinon on l'ajoute au chemin à parcourir et l'affiche.
                { 
                    foreach (HexMap.Node node in path)
                    {
                        unit.AddToHexPath(node.hex);
                        hexMap.GetHexeGameobjectFromDictionnary(node.hex).GetComponent<HexComponent>().HighlightHexagon();
                    }
                }
            }
            //TODO éventuellement tenir compte des ennemis.
            
        }
        else if(gameobject == gameObject) //Si clic droit sur l'unité --> reset la liste de déplacements de l'unité
        {
            foreach (Hex hex in unit.hexPath)
            {
                hexMap.GetHexeGameobjectFromDictionnary(hex).GetComponent<HexComponent>().UnHighlightHexagon();
            }
            unit.SetHexPath(new Hex[0]);
        }
        //*****************TODO: else if(gameobject.tag == "Ennemy") { DoAttack() }
    }

    public List<HexMap.Node> DijkstraPathfinding(HexMap.Node[,] pathFindingGraph, Hex unitHex, Hex targetHex)
    {
        /*Renvoie une liste des noeuds à parcourrir pour arriver à la destination le plus vite possible à partir du
        pathfindingGraph (array de listes chainées décrivant les voisins de chaque noeuds), l'hexagone sur lequel est
        l'unité et l'hexagone ciblé.*/

        HexMap.Node source = pathFindingGraph[unitHex.Q, unitHex.R]; //Noeud de l'hexagone de l'unité
        HexMap.Node target = pathFindingGraph[targetHex.Q,targetHex.R]; //Noeud de l'hexagone de la case souhaitée

        Dictionary<HexMap.Node, float> distance = new Dictionary<HexMap.Node, float>(); //Distance de chaque noeud par rapport au noeud source

        //Noeud Précédent pour chaque noeud pour retourner au noeud source le plus vite possible
        Dictionary<HexMap.Node, HexMap.Node> previousNodes = new Dictionary<HexMap.Node, HexMap.Node>(); 

        List<HexMap.Node> unvisitedNodesQueue = new List<HexMap.Node>(); //Liste des noeuds qui n'ont pas été examinés encore

        //Initialise les dictionnaires et remplit la liste des unvisited nodes
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

            //Le noeud u va être le unvisited node avec la distance la plus faible (le premier va être la source).
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
                break; //sort de la boucle While car on a trouvé la target
            }

            unvisitedNodesQueue.Remove(u); //On remove le noeud u considéré de la liste des noeuds non évalués

            foreach (HexMap.Node v in u.neighbours) 
            {
                //On calcule sa distance relative au noeud source ou le coût en déplacement pour s'y déplacer
                //float alt = distance[u] + u.Distance(v); 
                float alt = distance[u] + v.hex.BaseMovementCost();
                if (alt < distance[v])
                {
                    distance[v] = alt;
                    previousNodes[v] = u; //On lui attribue son noeud précédent
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
            //On a trouvé le chemin le plus court.
            List<HexMap.Node> path = new List<HexMap.Node>(); //Liste qui va recueillir la liste des noeuds de la target jusqu'à la source
            HexMap.Node current = target;
            while (current != null)
            {
                path.Add(current);
                current = previousNodes[current];
            }
            path.Reverse(); //On inverse la liste pour partir de la source vers la target
            if (path[0] == source)
            {
                //On enlève la source si elle est présente car l'unité étant déjà dessus, elle ne doit pas se déplacer sur l'hexagone correspondant
                path.RemoveAt(0); 
            }
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
