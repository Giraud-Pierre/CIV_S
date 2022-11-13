using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] float horizontalSensitivity = 3f;
    // vertical rotation speed
    [SerializeField] float verticalSensitivity = 3f;
    [SerializeField] private MeshRenderer meshRenderer;
    private float mouseX;
    private float mouseY;
    private float xRotation;
    private float yRotation;
    private Camera camera;
    GameObject bullet;

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SpawnBullet();
        }

        /*if (Input.GetButtonDown("Fire3"))
        {
            StartCoroutine(SpawnBullets());
        }*/

        mouseX = Input.GetAxis("Mouse X") * horizontalSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * verticalSensitivity;
        yRotation += mouseX;
        xRotation -= mouseY;
        camera.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
        //bool hit = Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitObject);
        //Debug.Log(hit);
        //Debug.Log(hitObject); //Donne que le type de l'objet
        //Debug.DrawRay(camera.transform.position, camera.transform.forward * 1000f, Color.red);
        //Debug.Log(hitObject.distance);

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
    }

    private void SpawnBullet()
    {
        //Instantiate(bullet, new Vector3(player.transform.position.x+1, player.transform.position.y, player.transform.position.z), Quaternion.identity);

        //J'instantie la balle
        bullet = Instantiate(bulletPrefab, cameraTransform.position, Quaternion.identity);

        //Je récupère son rigidbody
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        //J'applique une force initiale à la balle, AddForce prend en paramètre la direction de la force et son intensité
        bulletRigidbody.AddForce(cameraTransform.forward * 1000f);
    }

    private IEnumerator SpawnBullets() //On crée une coroutine
    {
        for(int i = 0; i<5; i++)
        {
            SpawnBullet();
            yield return new WaitForSeconds(.1f); //Programme attend 2secondes puis fait spawn la deuxième balle
        }
    }
}
