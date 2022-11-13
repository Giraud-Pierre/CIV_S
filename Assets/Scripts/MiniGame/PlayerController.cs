using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] float horizontalSensitivity = 3f;
    // vertical rotation speed
    [SerializeField] float verticalSensitivity = 3f;
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
        mouseX = Input.GetAxis("Mouse X") * horizontalSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * verticalSensitivity;
        yRotation += mouseX;
        xRotation -= mouseY;
        camera.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //TODO:End the game and unit lose PV
            Debug.Log("You lost");
    }

    private void SpawnBullet()
    {
        //Instantiate(bullet, new Vector3(player.transform.position.x+1, player.transform.position.y, player.transform.position.z), Quaternion.identity);

        //J'instantie la balle
        bullet = Instantiate(bulletPrefab, camera.transform.position, Quaternion.identity);

        //Je récupère son rigidbody
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        //J'applique une force initiale à la balle, AddForce prend en paramètre la direction de la force et son intensité
        bulletRigidbody.AddForce(camera.transform.forward * 1000f);
    }
}
