using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetsController : MonoBehaviour
{
    [SerializeField] GameObject prefabTarget;
    [SerializeField] uint appearanceFrequency = 10;
    private float Timer;
    private float randomX;
    private float randomY;
    private float randomZ;
    private Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        Timer = appearanceFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            randomX = Random.Range(-20, 20);
            randomY = Random.Range(5, 20);
            randomZ = Random.Range(-20, 20);
            rotation = Quaternion.Euler(Random.Range(0, 90), Random.Range(0, 360), 0);
            Instantiate(prefabTarget, new Vector3(randomX, randomY, randomZ), rotation);
            //Debug.Log("instantiate");
            Timer = appearanceFrequency;
        }
    }
}
