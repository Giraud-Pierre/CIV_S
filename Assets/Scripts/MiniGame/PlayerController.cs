using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] float horizontalSensitivity = 3f;
    [SerializeField] new Camera camera;
    // vertical rotation speed
    [SerializeField] float verticalSensitivity = 3f;
    private float mouseX;
    private float mouseY;
    private float xRotation;
    private float yRotation;
    

    [SerializeField] DataForMiniGame dataForMiniGame;
    GameObject prefabTarget;
    GameObject bullet;
    private int damage;

    void Start()
    {
        prefabTarget = dataForMiniGame.ennemyPrefab;
        GameObject enemy = Instantiate(prefabTarget, new Vector3(-34f, 3.6f, 55.02f), Quaternion.identity);
        enemy.layer = 7;
        enemy.AddComponent<Rigidbody>();
        enemy.GetComponent<Rigidbody>().useGravity = false;
        enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        enemy.GetComponent<Rigidbody>().mass = 8;
        enemy.AddComponent<EnnemyController>();
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
        Debug.Log("ennemy hit");
        //TODO:End the game and unit lose PV
        Destroy(collision.gameObject);
        dataForMiniGame.isWin = false;
        SceneManager.LoadScene(1);
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

    public DataForMiniGame getDataForMiniGame()
    {
        return dataForMiniGame;
    }
}
