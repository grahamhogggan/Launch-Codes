using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUnit : Component
{
    float result;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
        result = 0;
    }
    public override void UpdateComponent(float deltaTime)
    {
    }
    public override float FetchVar(string varName)
    {
        if (varName == "result") return result;
        return 0;
    }
    public override void ReceiveCommand(string command)
    {
        string[] tokens = command.Split(" ");
        if (tokens[0] == "add")
        {
            float num1 = float.Parse(tokens[1]);
            float num2 = float.Parse(tokens[2]);
            result = num1 + num2;
        }
        if (tokens[0] == "subtract")
        {
            float num1 = float.Parse(tokens[1]);
            float num2 = float.Parse(tokens[2]);
            result = num1 - num2;
        }
        if (tokens[0] == "multiply")
        {
            float num1 = float.Parse(tokens[1]);
            float num2 = float.Parse(tokens[2]);
            result = num1 * num2;
        }
        if (tokens[0] == "divide")
        {
            float num1 = float.Parse(tokens[1]);
            float num2 = float.Parse(tokens[2]);
            result = num1 / num2;
        }
    }
}
