using Gameplay;
using System.Collections;
using UnityEngine;

public class PachinkoClaw : MonoBehaviour
{
    [SerializeField] private Rigidbody2D slider;
    [SerializeField] private HingeJoint2D leftClaw;
    [SerializeField] private HingeJoint2D rightClaw;
    [SerializeField] private Transform itemPosition;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float closeAngle = 45f;
    [SerializeField] private float clawStrength = 100f;
    private bool isOpen;
    private PachinkoMachine machine;

    public Transform ItemPosition => itemPosition;

    public void Init(PachinkoMachine pachinkoMachine, Vector3 spawnPos)
    {
        machine = pachinkoMachine;
        transform.position = spawnPos;
        SetClawStrength(0);
        isOpen = false;
    }

    public void Update()
    {
        if (isOpen || machine.State != GameState.Movingclaw) return;

        if (Input.GetKey(KeyCode.A))
            MoveLeft();
        else if (Input.GetKey(KeyCode.D))
            MoveRight();
        else if (slider != null)
            slider.velocity = new Vector2(0f, slider.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && machine.State == GameState.Movingclaw)
        {
            StartCoroutine(OpenClaw());
            machine.DropItem();
        }
    }

    private void MoveLeft()
    {
        if (slider != null)
            slider.velocity = new Vector2(-moveSpeed, slider.velocity.y);
    }

    private void MoveRight()
    {
        if (slider != null)
            slider.velocity = new Vector2(moveSpeed, slider.velocity.y);
    }

    private IEnumerator OpenClaw()
    {
        isOpen = true;
        SetClawStrength(clawStrength);
        yield return new WaitForSeconds(1f);
        SetClawStrength(0);
        isOpen = false;
    }

    private void SetClawStrength(float strength)
    {
        if (leftClaw == null || rightClaw == null) return;

        var leftMotor = leftClaw.motor;
        var rightMotor = rightClaw.motor;
        if (strength > 0)
        {
            leftMotor.motorSpeed = strength * closeAngle;
            rightMotor.motorSpeed = -strength * closeAngle;
        }
        else
        {
            leftMotor.motorSpeed = -closeAngle * 2;
            rightMotor.motorSpeed = closeAngle * 2;
        }
        leftMotor.maxMotorTorque = clawStrength;
        rightMotor.maxMotorTorque = clawStrength;
        leftClaw.motor = leftMotor;
        rightClaw.motor = rightMotor;
        leftClaw.useMotor = strength > 0;
    }
}