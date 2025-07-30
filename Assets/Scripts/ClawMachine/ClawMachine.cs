using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ModeClaw
{
    Wait,
    Start,
    AutoMove,
    Use,
    PickUp,
    End,
    DeSpawn
}
public class ClawMachine : MonoBehaviour
{
    [SerializeField] protected LineRenderer rope;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected GameObject chain;
    [SerializeField] private float ropeLength = 10f;

    [Space]
    [Header("ClawLimits")]
    public GameObject lowLimit;
    public GameObject leftLimit;
    public GameObject rightLimit;

    [Space]
    [Header("Position")]
    public Transform posStartClaw;
    public Transform posStopClaw;
    public Vector3 posMove;

    protected ModeClaw mode;
    protected float moveForce = 5f;

    protected ClawController clawController;

    public ModeClaw Mode
    {
        get => mode;
        set => mode = value;
    }
    public ClawController ClawController
    {
        set => this.clawController = value;
    }
    private void Update()
    {
        UpdateRope();
        Claw();
    }
    public void Claw()
    {
        switch(mode)
        {
            case ModeClaw.Wait:
                break;

            case ModeClaw.Start:
                StartClaw();
                break;

            case ModeClaw.AutoMove:
                AutoMove();
                break;

            case ModeClaw.Use:
                MoveClaw();
                CheckPickUp();
                break;

            case ModeClaw.PickUp:
                PickUp();
                break;

            case ModeClaw.End:
                EndClaw();
                break;

            case ModeClaw.DeSpawn:
                Despawn();
                break;

        }
    }

    public void MoveClaw()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0)
        {
            CheckPosLimit(horizontal);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    public void CheckPosLimit(float pressHorizontal)
    {
        if(pressHorizontal > 0)
        {
            if(chain.transform.position.x <= rightLimit.transform.position.x)
                rb.velocity = new Vector2(pressHorizontal * moveForce, rb.velocity.y);
            else
                rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if(pressHorizontal < 0)
        {
            if (chain.transform.position.x >= leftLimit.transform.position.x)
                rb.velocity = new Vector2(pressHorizontal * moveForce, rb.velocity.y);
            else
                rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    public void UpdateRope()
    {
        Vector3 currentPos = rope.GetPosition(0);

        rope.SetPosition(0, new Vector3(chain.transform.position.x, chain.transform.position.y, currentPos.z));
        rope.SetPosition(1, new Vector3(chain.transform.position.x, chain.transform.position.y + ropeLength, currentPos.z));
    }

    public void AutoMove()
    {
        if(chain.transform.position.x < posMove.x)
        {
            rb.velocity = Vector2.right * moveForce;
        }
        else
        {
            rb.velocity = Vector2.zero;
            mode = ModeClaw.Wait;
        }
    }
    public virtual void StartClaw()
    {
        //Debug.Log("Start Claw ");
        if (chain.transform.position.x <= posStartClaw.position.x)
        {
            rb.velocity = Vector2.right * moveForce;
        }    
        else if (chain.transform.position.y > posStartClaw.position.y)
        {
            rb.velocity = Vector2.down * moveForce;
        }
        else if (chain.transform.position.y <= posStartClaw.position.y)
        {
            rb.velocity = new Vector2(0, 0);
            mode = ModeClaw.Use;
        }
    }
    public void EndClaw()
    {
        if (chain.transform.position.x <= posStopClaw.position.x)
            rb.velocity = Vector2.right * moveForce;
        else if (chain.transform.position.y < posStopClaw.position.y)
        {
            rb.velocity = Vector2.zero * moveForce;
            StartCoroutine(DelayOpen(1f));
        }
    }
    public void Despawn()
    {
        if (chain.transform.position.y < posStopClaw.position.y)
        {
            rb.velocity = Vector2.up * moveForce;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CheckPickUp()
    {
        if(Input.GetKeyDown(KeyCode.Space) && mode != ModeClaw.PickUp)
        {
            Debug.Log("Claw is Pick up");
            mode = ModeClaw.PickUp;
        }
    }
    public virtual void PickUp()
    {
        //Debug.Log("Pick Up");
        if(chain.transform.position.y > lowLimit.transform.position.y)
        {
            rb.velocity = Vector2.down * moveForce;
        }
        else
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(DelayPickUp(1.5f));
        }
    }

    protected IEnumerator DelayPickUp(float time)
    {
        yield return new WaitForSeconds(time);

        if(chain.transform.position.y < posStartClaw.position.y)
        {
            rb.velocity = Vector2.up * moveForce;
        }
        else
        {
            rb.velocity = Vector2.zero;
            mode = ModeClaw.End;
        }

    }
    protected virtual IEnumerator DelayOpen(float time)
    {
        //OpenClaw();
        yield return new WaitForSeconds(time);

        if (this == null || gameObject == null || clawController == null)
            yield break;

        if (mode != ModeClaw.DeSpawn)
        {
            mode = ModeClaw.DeSpawn;
            clawController.ChangeClaw();
        }
    }
}
