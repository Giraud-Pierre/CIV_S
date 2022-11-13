using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{
    [SerializeField] GameObject prefabTarget;
    private GameObject enemyGO;
    [SerializeField] float enemySpeed = 1.5f;
    [SerializeField] float frequency = 1.0f;
    [SerializeField] float magnitude = 0.5f;
    private int knockback = 5;
    private Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        enemyGO = Instantiate(prefabTarget, new Vector3(-34f, 4.12f, 55.02f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        enemyGO.transform.position += new Vector3(Mathf.Sin(Time.time * frequency) * magnitude, 0, -enemySpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO:End the game and unit lose PV and knockback
        if(other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            enemyGO.transform.position -= new Vector3(0, 0, knockback);
            Debug.Log("Enemy lose 1PV");
        }
        
    }
}
