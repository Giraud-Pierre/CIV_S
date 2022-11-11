using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

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
        Debug.Log("setactive");
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
                
            }
        }
    }

    public void OnLeftClickOnOtherAction()
    {

    }

    public void OnRightClickAction(GameObject gameobject)
    {
        if(gameobject.tag == "Hexagon")
        {
            //*************TODO: vérifier si la case est adjacente ou s'il va falloir calculer un chemin (et si la case est disponible (!= eau ou ennemi))
            unit.AddToHexPath(gameobject.GetComponent<HexComponent>().hex);
        }
        //*****************TODO: else if(gameobject.tag == "Ennemy") { DoAttack() }
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
