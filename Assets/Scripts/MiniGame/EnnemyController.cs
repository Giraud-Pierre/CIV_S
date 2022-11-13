using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{
    
    private GameObject enemyGO;
    [SerializeField] float enemySpeed = 2f;
    [SerializeField] float frequency = 1.5f;
    [SerializeField] float magnitude = 0.1f;
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
        enemyGO.transform.position += new Vector3(Mathf.Sin(Time.time * frequency) * magnitude, 0, -enemySpeed * Time.deltaTime);
    }

    private void CollisionEnterTrigger(Collider other)
    {
        //TODO:End the game and unit lose PV and knockback
        if(other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            enemyGO.transform.position -= new Vector3(0, 0, knockback);
            Debug.Log("Enemy lose 1PV");
        }
        
    }
}
