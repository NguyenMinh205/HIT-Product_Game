using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum ModeClaw
{
    Wait,
    Start,
    Use,
    PickUp,
    End
}
public class ClawMachine : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HingeJoint2D leftClaw;
    [SerializeField] private HingeJoint2D rightClaw;
    [SerializeField] private GameObject chain;

    private ModeClaw mode;
    private float moveForce = 5f;
    private bool isMove;
    private int moves;
    private List<Vector3> movePos;

    public ModeClaw Mode
    {
        get => mode;
        set => mode = value;
    }
    public int Move
    {
        get => moves;
    }
    private void Awake()
    {
        movePos = new List<Vector3>();
        mode = ModeClaw.Wait;
    }

    private void Update()
    {
        MoveLime();
        
        AutoMove();

        MoveClaw();

        PickUp();

        CheckDeSpawn();
    }

    public void MoveClaw()
    {
        if (mode != ModeClaw.Use) return;

        float pessHorizotal = Input.GetAxis("Horizontal");
        
        if(pessHorizotal != 0 )
        {
            rb.velocity = new Vector2 (pessHorizotal * moveForce , rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    public void MoveLime()
    {
        Vector3 currentPos = line.GetPosition(0);

        Transform chain = rb.GetComponent<Transform>();

        line.SetPosition(0, new Vector3(chain.position.x, chain.position.y, currentPos.z));
        line.SetPosition(1, new Vector3(chain.position.x, chain.position.y + 3, currentPos.z));
    }
    public void SetMovePos(Vector3 pos)
    {
        movePos.Add(pos);
    }
    public void AutoMove()
    {
        if (movePos.Count == 0)
        {
            isMove = false;
            return;
        }
        else
            isMove = true;

        Vector3 pos1 = chain.transform.position;
        Vector3 pos2 = movePos[0];
        pos1.z = 0;
        pos2.z = 0;
        if (Vector3.Distance(pos1, pos2) > 0.1f)
        {
            rb.velocity = (movePos[0] - chain.transform.position).normalized * moveForce;
        }
        else
        {
            movePos.RemoveAt(0);
            moves++;
            rb.velocity = Vector3.zero;
            CheckMoves();
        }
    }
    public void StartClaw(Vector3 posStart)
    {
        moves = 0;
        mode = ModeClaw.Start;
        SetMovePos(posStart);
        Vector3 pos = posStart;
        pos.y -= 1f;
        SetMovePos(pos);
    }
    public void EndClaw(Vector3 posEnd)
    {
        SetMovePos(posEnd);
        Vector3 pos = posEnd;
        pos.y += 5f;
        OpenClaw();
        StartCoroutine(DelayDeSpawn(1f, pos));
    }
    public void CheckMoves()
    {
        if (moves == 2 && mode == ModeClaw.Start)
        {
            mode = ModeClaw.Use;
            SetClaw(true);
        }
    }
    public void PickUp()
    {
        if(Input.GetKeyDown(KeyCode.Space) && mode == ModeClaw.Use)
        {
            Vector3 pos = new Vector3(chain.transform.position.x, chain.transform.position.y - 2.2f, 0f);
            SetMovePos(pos);
            mode = ModeClaw.PickUp;
            StartCoroutine(DelayPickUp(2f));
        }
    }

    public void SetClaw(bool isClaw)
    {
        leftClaw.useMotor = isClaw;
        rightClaw.useMotor = isClaw;
    }
    public void CheckDeSpawn()
    {
        if (chain.transform.position.y > 5f)
            PoolingManager.Despawn(gameObject);
    }

    IEnumerator DelayPickUp(float time)
    {
        yield return new WaitForSeconds(time);

        CloseClaw();

        SetMovePos(new Vector3(chain.transform.position.x, chain.transform.position.y + 2.5f, 0f));
        mode = ModeClaw.End;
    }
    IEnumerator DelayDeSpawn(float time, Vector3 pos)
    {
        yield return new WaitForSeconds(time);
        SetMovePos(pos);
    }

    public void OpenClaw()
    {
        JointMotor2D motor = leftClaw.motor;
        motor.motorSpeed = 50f;
        leftClaw.motor = motor;

        motor = rightClaw.motor;
        motor.motorSpeed = -50f;
        rightClaw.motor = motor;
    }
    public void CloseClaw()
    {
        JointMotor2D motor = leftClaw.motor;
        motor.motorSpeed = -50f;
        leftClaw.motor = motor;

        motor = rightClaw.motor;
        motor.motorSpeed = 50f;
        rightClaw.motor = motor;
    }
}
