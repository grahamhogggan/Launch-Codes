using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleSensor : Component
{
    private float angle;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
    }
    public override void UpdateComponent(float deltaTime)
    {
        angle = transform.rotation.eulerAngles.z;
    }
    public override float FetchVar(string varName)
    {
        if (varName == "angle") return angle;
        return 0;
    }
}
