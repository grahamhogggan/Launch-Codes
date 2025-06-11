using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampedCameraFollower : MonoBehaviour
{
    public GameObject target;
    public float damp;
    private Vector3 velocity;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("mainVehicle");
        rb = target.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x - rb.velocity.x * damp, target.transform.position.y - rb.velocity.y * damp, transform.position.z);
        GetComponent<Camera>().orthographicSize -= Input.GetAxis("Mouse ScrollWheel")*10;

    }
}
