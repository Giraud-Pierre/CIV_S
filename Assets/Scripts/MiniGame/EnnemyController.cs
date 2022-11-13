using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{
    
    private GameObject enemyGO;
    [SerializeField] float enemySpeed = 2f;
    [SerializeField] float frequency = 1.5f;
    [SerializeField] float magnitude = 0.1f;
    [SerializeField] GameObject player;
    private int knockback = 5;
    private Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        enemyGO = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //enemyGO.transform.position += new Vector3(Mathf.Sin(Time.time * frequency) * magnitude, 0, -enemySpeed * Time.deltaTime);
        MoveTowardPlayer();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //TODO:End the game and unit lose PV and knockback
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            //enemyGO.transform.position += new Vector3(0, 0, knockback);
            this.gameObject.GetComponent<Rigidbody>().AddForce(-15f * transform.forward, ForceMode.Impulse);
            Debug.Log("Enemy lose 1PV");
        }
    }

    private void MoveTowardPlayer()
    {
        transform.LookAt(player.transform);
        transform.position += new Vector3(Mathf.Sin(Time.time * frequency) * magnitude, 0, -enemySpeed * Time.deltaTime);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
