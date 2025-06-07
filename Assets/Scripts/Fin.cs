using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fin : Component
{
    public float finPower;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
    }
    public override void UpdateComponent(float deltaTime)
    {
        Vector2 up = transform.up;
        Vector2 velocity = (Vector2)vehicleBody.velocity;
        if (velocity.magnitude < 0f)
        {
            return;
        }
        float mainAngle = Vector2.Angle(up, velocity)*3.14195f/180;
        Debug.Log(up + "," + velocity);
        //Debug.Log(mainAngle);
        float VX = Mathf.Cos(mainAngle) * velocity.magnitude;
        float VY = Mathf.Sin(mainAngle) * velocity.magnitude;
        Debug.Log(VX + ", " + VY);

        Vector2 newVelocity = up.normalized * VX + (Vector2)transform.right.normalized * -1 * VY * (1-finPower*deltaTime);
        vehicleBody.velocity = newVelocity;
        Debug.Log(newVelocity);

    }
}
