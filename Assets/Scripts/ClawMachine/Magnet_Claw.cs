using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Magnet_Claw : ClawMachine
{
    [SerializeField] private GameObject magnetClaw;
    [SerializeField] private Sprite spriteMagnetOn;
    [SerializeField] private Sprite spriteMagnetOff;

    private CapsuleCollider2D capsule;
    //private CircleCollider2D circle;

    private void Awake()
    {
        magnetClaw.GetComponent<PointEffector2D>().enabled = false;
        capsule = magnetClaw.GetComponent<CapsuleCollider2D>();
        //circle = magnetClaw.GetComponent<CircleCollider2D>();
        capsule.enabled = false;
        //circle.enabled = false;
    }

    protected override IEnumerator DelayPickUp(float time)
    {
        SetStateMagnet(true);
        yield return new WaitForSeconds(time);

        if (chain.transform.position.y < posStartClaw.position.y)
        {
            rb.velocity = Vector2.up * moveForce;
        }
        else
        {
            rb.velocity = Vector2.zero;
            mode = ModeClaw.End;
        }

    }
    protected override IEnumerator DelaOpen(float time)
    {
        SetStateMagnet(false);
        yield return new WaitForSeconds(time);
        if (mode != ModeClaw.DeSpawn)
        {
            mode = ModeClaw.DeSpawn;
            clawController.ChangeClaw();
        }
    }

    public void SetStateMagnet(bool val)
    {
        //Set hinh anh
        if(val)
            magnetClaw.GetComponent<SpriteRenderer>().sprite = spriteMagnetOn;
        else
            magnetClaw.GetComponent<SpriteRenderer>().sprite = spriteMagnetOff;

        //set collider2D
        capsule.enabled = val;
       // circle.enabled = val;

        //Bat tat magnet
        PointEffector2D effector = magnetClaw.GetComponent<PointEffector2D>();
        effector.enabled = val;
    }

}
