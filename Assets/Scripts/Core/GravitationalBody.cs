using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalBody : MonoBehaviour
{
    public float mass;
    private Rigidbody2D vehicle;
    // Start is called before the first frame update
    void Start()
    {
         vehicle = GameObject.FindGameObjectWithTag("mainVehicle").GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (vehicle.position - (Vector2)transform.position).normalized;
        float distance = Vector2.Distance(vehicle.position, transform.position);
        float forceMagnitude = (mass * vehicle.mass) / Mathf.Pow(distance, 2);
        vehicle.AddForce(-1*direction * forceMagnitude);
    }
}
