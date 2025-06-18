using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detachment : Component
{
    public GameObject[] AssociatedComponents;
    private float angle;
    public AudioSource src;
    public Vector2 centerOfMassChange;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
    }
    public override void UpdateComponent(float deltaTime)
    {
        //do nothing
    }
    public override void ReceiveCommand(string command)
    {
        if (command == "detach")
        {
            foreach (GameObject obj in AssociatedComponents)
            {
                obj.transform.SetParent(transform);
            }
            foreach (Component c in GetComponentsInChildren<Component>())
            {
                vehicle.GetComponent<Vehicle>().Components.Remove(c);
            }
            foreach (AudioSource src in GetComponentsInChildren<AudioSource>())
            {
                src.Stop();
            }
            if (src != null)
            {
                src.Play();
            }
            transform.parent = null;
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.velocity = vehicleBody.velocity + (Vector2)(transform.position-vehicle.transform.position).normalized;
            vehicle.GetComponent<Vehicle>().RecalulateCenterOfMass(new Vector2(0,0));
            foreach(Transform stillAttached in vehicle.transform)
            {
                stillAttached.position+=vehicle.transform.TransformDirection(-1 * centerOfMassChange);
            }
            vehicleBody.MovePosition(vehicle.transform.position+vehicle.transform.TransformDirection((Vector3)centerOfMassChange));
            vehicleBody.MoveRotation(vehicle.transform.rotation);


            
            Destroy(this);
            
        }
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.parent.position+transform.parent.TransformDirection((Vector3)centerOfMassChange), 1);
    }
}
