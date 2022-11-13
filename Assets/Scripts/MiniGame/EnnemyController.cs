using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class EnnemyController : MonoBehaviour
{
    [SerializeField] int health = 3; //on peut modifier les points de vie de l'ennemi ici
    [SerializeField] float MovementSpeed = 1f; //on peut modifier la vitesse de déplacement de l'ennemi ici
    [SerializeField] int ennemySpeed = 2;
    [SerializeField] int frequency = 2;
    [SerializeField] float magnitude = 0.1f;


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
        transform.position +=  new Vector3(Mathf.Sin(Time.time * frequency) * magnitude, 0, -ennemySpeed * Time.deltaTime); //fait avancer l'ennemi
    }


    void Start()
    {
        player = GameObject.Find("Player").transform; //va rechercher le PlayerBody du joueur
    }


    void Update()
    {

        MoveTowardPlayer();
        check_out_limits();
    }

}