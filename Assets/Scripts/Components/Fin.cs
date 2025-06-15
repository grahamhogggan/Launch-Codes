using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fin : Component
{
    public float finPower;
    public float rotationalSize;
    private string? keyF;
    private string? keyB;
    public float rotationAngle;
    private Quaternion homeRotation;
    Vector3 forceVector;
    float X;
    float Y;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
        homeRotation = transform.localRotation;
        keyF = null;
        keyB = null;
    }
    public override void UpdateComponent(float deltaTime)
    {
        if (keyF != null && Input.GetKey(keyF))
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, rotationAngle), deltaTime) * homeRotation;
        }
        else if (keyB != null && Input.GetKey(keyB))
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, -rotationAngle), deltaTime) * homeRotation;
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, homeRotation, deltaTime);
        }
        Vector2 up = transform.up;
        Vector2 velocity = (Vector2)vehicleBody.velocity;
        if (velocity.magnitude < 0f)
        {
            return;
        }
        float mainAngle = Vector2.SignedAngle(up, velocity) * 3.14195f / 180;
        //Debug.Log(up + "," + velocity);
        //Debug.Log(mainAngle);
        float VX = Mathf.Cos(mainAngle) * velocity.magnitude;
        X = VX;
        float VY = Mathf.Sin(mainAngle) * velocity.magnitude;
        Y = VY;
        // Debug.Log(VX + ", " + VY);

        Vector2 newVelocity = up.normalized * VX + (Vector2)transform.right.normalized * -1 * VY * (1 - finPower * deltaTime);
        vehicleBody.velocity = newVelocity;
        //Debug.Log(newVelocity);
        float force = VY;
        float angle = Vector3.SignedAngle(transform.right, vehicle.transform.position - transform.position, transform.forward) * Mathf.PI / 180;
        float moment = Vector3.Distance(transform.position, vehicle.transform.position) * Mathf.Sin(angle);
        vehicleBody.AddTorque(moment * deltaTime * force * rotationalSize);
        //Debug.Log(moment * deltaTime * force * rotationalSize);
        forceVector = transform.right * force;

    }
    public override void ReceiveCommand(string command)
    {
        string[] tokens = command.Split(" ");
        if (tokens[0] == "bind")
        {
            if (tokens[1] == "forward")
            {
                keyF = tokens[2];
            }
            if (tokens[1] == "backward")
            {
                keyB = tokens[2];
            }
        }
    }
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + forceVector);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)vehicleBody.velocity);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * X);
        Gizmos.DrawLine(transform.position + transform.up * X, transform.position + transform.up * X-transform.right * Y);
    }
}
