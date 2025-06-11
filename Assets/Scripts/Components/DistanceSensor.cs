using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSensor : Component
{
    private float distance;
    public LayerMask shipMask;
    [SerializeField] private Transform raycastPosition;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
    }
    public override void UpdateComponent(float deltaTime)
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition.position, transform.up,Mathf.Infinity,~shipMask);
        distance = hit.distance;
        //Debug.Log(hit.collider);
       // Debug.Log(distance);
    }
    public override float FetchVar(string varName)
    {
        if (varName == "distance") return distance;
        return 0;
    }
}
