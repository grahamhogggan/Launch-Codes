using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Component
{
    public float thrust;
    public float actualThrust;
    private string? bindKey = null;
    public AudioSource sound;

    public GameObject indicator;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
        actualThrust = thrust;
    }
    public override void UpdateComponent(float deltaTime)
    {
        if (bindKey != null)
        {
            if (Input.GetKey(bindKey))
            {
                actualThrust = thrust;
            }
            else
            {
                actualThrust = 0;
            }
        }
        if (actualThrust > 0)
        {
            indicator.SetActive(true);
            if (sound!=null&&!sound.isPlaying)
            {
                sound.Play();
            }
        }
        else
        {
            indicator.SetActive(false);
            if(sound!=null)
            sound.Stop();
        }

        vehicleBody.AddForce(transform.up * actualThrust * deltaTime, ForceMode2D.Force);
        float angle = Vector3.SignedAngle(transform.up, vehicle.transform.position - transform.position, transform.forward) * Mathf.PI / 180;
        float moment = Vector3.Distance(transform.position, vehicle.transform.position) * Mathf.Sin(angle);
        //Debug.Log("moment: "+moment+" Angle: "+angle);
        vehicleBody.AddTorque(moment * deltaTime * actualThrust);
    }
    public override void ReceiveCommand(string command)
    {
        if (command == "shutdown")
        {
            actualThrust = 0;
        }
        if (command == "startup")
        {
            actualThrust = thrust;
        }
        if (command == "unbind")
        {
            bindKey = null;
        }
        string[] split = command.Split(" ");
        if (split[0] == "bind")
        {
            bindKey = split[1];
        }
        if (split[0] == "set")
        {
            //Debug.Log(command);
            actualThrust = Mathf.Max(0, Mathf.Min(float.Parse(split[1]), thrust));
        }

    }
    public override float FetchVar(string varName)
    {
        if (varName == "maxThrust") return thrust;
        if (varName == "actualThrust") return actualThrust;
        return 0;
    }
}
