using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    protected GameObject vehicle;
    protected Rigidbody2D vehicleBody;
    public string Identifier;
    public virtual void InitializeComponent()
    {
        vehicle = GetComponentInParent<Vehicle>().gameObject;
        vehicleBody = vehicle.GetComponent<Rigidbody2D>();
    }
    public virtual void UpdateComponent(float deltaTime)
    {
        Debug.Log("Update Component method was not correctly overriden on { " + gameObject.name + "}");
    }
    public virtual void ReceiveCommand(string command)
    {
        //Debug.Log(Identifier + " received command: " + command);
    }
    public virtual float FetchVar(string varName)
    {
        return 0;
    }
}
