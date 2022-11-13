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
        Instantiate(prefabTarget, new Vector3(-35.47f, 4.12f, 55.02f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
