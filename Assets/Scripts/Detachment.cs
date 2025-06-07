using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detachment : Component
{
    public GameObject[] AssociatedComponents;
    private float angle;
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
            transform.parent = null;
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.velocity = vehicleBody.velocity + (Vector2)(transform.position-vehicle.transform.position).normalized;
            Destroy(this);
            
        }
        
    }
}
