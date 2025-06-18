using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject[] vehicles;
    public static int currentVehicle = 0;
    public Vector3 spawnPoint;
    // Start is called before the first frame update
    void Awake()
    {
        Instantiate(vehicles[currentVehicle], spawnPoint, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPoint, 1);
    }
    public void ChangeVehicle(int newVehicle)
    {
        currentVehicle = newVehicle;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
