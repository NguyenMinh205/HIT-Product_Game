using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
    [SerializeField] private LineRenderer line;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HingeJoint2D leftClaw;
    [SerializeField] private HingeJoint2D rightClaw;
    [SerializeField] private GameObject chain;

    [Space]
    [Header("ClawLimits")]
    //[SerializeField] private Transform topLimit;
    public GameObject lowLimit;
    public GameObject leftLimit;
    public GameObject rightLimit;

    [Space]
    [Header("Position")]
    public Transform posStartClaw;
    public Transform posStopClaw;
    public Vector3 posMove;

    private ModeClaw mode;
    private float moveForce = 5f;

    [SerializeField] private float closeAngle = 45f;
    [SerializeField] private float clawStrength = 25f;

    public ModeClaw Mode
    {
        get => mode;
        set => mode = value;
    }
    private void Update()
    {
        MoveLine();
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
                DeSpawn();
                break;

        }
    }

    public void MoveClaw()
    {
        float pessHorizotal = Input.GetAxis("Horizontal");

        if (pessHorizotal != 0)
        {
            CheckPosLimit(pessHorizotal);
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
    public void MoveLine()
    {
        Vector3 currentPos = line.GetPosition(0);

        line.SetPosition(0, new Vector3(chain.transform.position.x, chain.transform.position.y, currentPos.z));
        line.SetPosition(1, new Vector3(chain.transform.position.x, chain.transform.position.y + 6, currentPos.z));
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
    public void StartClaw()
    {
        if (chain.transform.position.x <= posStartClaw.position.x)
            rb.velocity = Vector2.right * moveForce;
        else if (chain.transform.position.y > posStartClaw.position.y)
        {
            rb.velocity = Vector2.down * moveForce;
        }
        else if (chain.transform.position.y <= posStartClaw.position.y)
        {
            OpenClaw();
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
            StartCoroutine(DelaOpen(1.2f));
        }
    }
    public void DeSpawn()
    {
        if (chain.transform.position.y < posStopClaw.position.y)
        {
            rb.velocity = Vector2.up * moveForce;
        }
        else
            DeSpawnClaw();
    }
    public void CheckPickUp()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mode = ModeClaw.PickUp;
        }
    }
    public void PickUp()
    {
        Debug.Log("Pick Up");
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

    public void SetClaw(bool isClaw)
    {
        leftClaw.useMotor = isClaw;
        rightClaw.useMotor = isClaw;
    }
    public void DeSpawnClaw()
    {
        Debug.Log("DeSpawn");
        PoolingManager.Despawn(gameObject);
    }

    IEnumerator DelayPickUp(float time)
    {
        CloseClaw();
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
    IEnumerator DelaOpen(float time)
    {
        OpenClaw();
        yield return new WaitForSeconds(time);
        mode = ModeClaw.DeSpawn;
    }

    public void OpenClaw()
    {
        Debug.Log("Open");
        JointMotor2D motor = leftClaw.motor;
        motor.motorSpeed = 50f;
        motor.maxMotorTorque = 50f;
        leftClaw.motor = motor;

        motor = rightClaw.motor;
        motor.motorSpeed = -50f;
        motor.maxMotorTorque = 50f;
        rightClaw.motor = motor;

    }
    public void CloseClaw()
    {
        Debug.Log("Close");
        JointMotor2D motor = leftClaw.motor;
        motor.motorSpeed = -50f;
        motor.maxMotorTorque = 10000f;
        leftClaw.motor = motor;

        motor = rightClaw.motor;
        motor.motorSpeed = 50f;
        motor.maxMotorTorque = 10000f;
        rightClaw.motor = motor;

    }
}
