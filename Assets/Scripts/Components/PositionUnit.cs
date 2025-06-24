using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUnit : Component
{
    private Vector3 position;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
    }
    public override void UpdateComponent(float deltaTime)
    {
       position = transform.position;
    }
    public override float FetchVar(string varName)
    {
        if (varName == "x") return position.x;
        if (varName == "y") return position.y;
        return 0;
    }
}
