using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Component
{
    private string? key;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
        key = null;
    }
    public override void UpdateComponent(float deltaTime)
    {
        //do nothing
    }
    public override float FetchVar(string varName)
    {
        if (varName == "value") return Input.GetKey(key) ? 0 : 1;
        return 0;
    }
    public override void ReceiveCommand(string command)
    {
        string[] tokens = command.Split(" ");
        if (tokens[0] == "bind")
        {
            key = tokens[1];
        }
    }
}
