using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) //Un appel par collision, donc si l'objet touche 6 autres objets, 6 appels à la méthode, onCollisionEnter2D fonctionne avec des rigidbody2D
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Background"))
        {
            Destroy(this.gameObject);
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //TODO : Enemy lose xPV
            collision.gameObject.GetComponent<Rigidbody>().AddForce(-15f * transform.forward, ForceMode.Impulse);
            Debug.Log("Ennemie touché");
        }

        /*if (collision.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }*/
    }

   /* private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision Exit!");
    }*/
}
