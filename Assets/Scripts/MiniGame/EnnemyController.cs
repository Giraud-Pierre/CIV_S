using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class EnnemyController : MonoBehaviour
{
    [SerializeField] private int health = 3; //on peut modifier les points de vie de l'ennemi ici
    [SerializeField] private float MovementSpeed = 1f; //on peut modifier la vitesse de déplacement de l'ennemi ici


    private Transform player;   //recueille le transform du joueur pour orienter le déplacement de l'ennemi
    private float realMovementSpeed; //recalcule la vitesse de déplacement de l'ennemi en prenant en compte d'autre facteur

    public void check_out_limits()//Détruit l'ennemi s'il est tombé du terrain
    {
        if (transform.position.y <= -1)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) //gère la collision des ennemis avec les balles
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            //fait reculer l'ennemi
            //gameObject.GetComponent<Rigidbody>().AddForce(-15f * transform.forward, ForceMode.Impulse);

            //détruit la balle
            Destroy(collision.gameObject);

            //diminue la vie de l'ennemi
            health--;
            if (health <= 0) //Détruit l'ennemi s'il est à cours de point de vie
            {
                Destroy(gameObject);
                SceneManager.LoadScene(1);
            }
        }
    }

    private void MoveTowardPlayer() //permet de bouger l'ennemi vers le joueur
    {
        transform.LookAt(player); //permet à l'ennemi de regarder le joueur
        transform.position +=  transform.forward * realMovementSpeed; //fait avancer l'ennemi
    }


    void Start()
    {
        player = GameObject.Find("Player").transform; //va rechercher le PlayerBody du joueur
    }


    void Update()
    {
        //calcule la vitesse de déplacement réelle de l'ennemi en prenant en compte les fps du joueurs
        //et en appliquant un facteur (comme pour la MovementSpeed dans le script PlayerController)
        //(il va un peu moins que 2 fois moins vite que le joueur pour qui le facteur est 15f)
        realMovementSpeed = MovementSpeed * Time.deltaTime * 7f;

        MoveTowardPlayer();
        check_out_limits();
    }

}