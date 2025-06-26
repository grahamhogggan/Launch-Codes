using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Component
{
    public float thrust;
    public float actualThrust;
    public LayerMask shipLayer;
    public GameObject landingDustFX;
    private GameObject landingDustInstant;
    private float LDDT;
    #nullable enable
    private string? bindKey = null;
    #nullable disable
    public AudioSource sound;

    public GameObject indicator;
    public override void InitializeComponent()
    {
        base.InitializeComponent();
        actualThrust = 0;
        if(landingDustFX!=null)
        {
            landingDustInstant = Instantiate(landingDustFX, transform.position, Quaternion.identity);
            landingDustInstant.SetActive(false);
            LDDT = 0;
        }
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
            RaycastHit2D hitted = Physics2D.Raycast(transform.position, -1 * transform.up, 100, ~shipLayer);
            if(landingDustFX!=null&&hitted.collider!=null&&hitted.distance<10)
            {
                LDDT += deltaTime;
                if(deltaTime>0.05)
                {
                    foreach(ParticleSystem ps in landingDustInstant.GetComponentsInChildren<ParticleSystem>())
                    ps.Emit((int)(deltaTime/0.05));
                }
                else if(LDDT>0.05)
                {
                    foreach(ParticleSystem ps in landingDustInstant.GetComponentsInChildren<ParticleSystem>())
                    ps.Emit(1);
                    LDDT = 0;
                }

            }
        }
        else
        {
            indicator.SetActive(false);
            if(sound!=null)
            sound.Stop();
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -1 * transform.up, 100, ~shipLayer);
            if(landingDustFX!=null&&hit.collider!=null&&hit.distance<10)
            {
                if(landingDustFX!=null)
                {
                    landingDustInstant.transform.position = hit.point;
                    landingDustInstant.transform.rotation = Quaternion.Euler(0,0,-1 * Vector2.SignedAngle(hit.normal, Vector2.up));
                    landingDustInstant.SetActive(true);
                }
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
