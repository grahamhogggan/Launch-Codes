using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityIndicator : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -1 * Vector3.SignedAngle(rb.velocity, Vector3.up, Vector3.forward)));
        if (rb.velocity.magnitude < 0.1f)
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }
}
