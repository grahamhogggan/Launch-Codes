using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocitySensor : Component
{
    private Vector2 velocity;
    private float angularVelocity;
    private Rigidbody2D rb;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
    }
    public override void UpdateComponent(float deltaTime)
    {
        velocity = vehicleBody.velocity;
        angularVelocity = vehicleBody.angularVelocity;
    }
    public override float FetchVar(string varName)
    {
        if (varName == "angular") return angularVelocity;
        if (varName == "vertical") return velocity.y;
        if (varName == "horizontal") return velocity.x;
        return 0;
    }
}
