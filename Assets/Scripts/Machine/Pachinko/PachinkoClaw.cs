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
    private float offsetItem;
    private bool isOpen;
    private bool isMoving;
    private PachinkoMachine machine;

    public Transform ItemPosition => itemPosition;

    public void Init(PachinkoMachine pachinkoMachine, Vector3 spawnPos)
    {
        machine = pachinkoMachine;
        transform.position = spawnPos;
        offsetItem = slider.transform.position.y - itemPosition.transform.position.y;
        SetClawStrength(0);
        isOpen = false;
        isMoving = false;
        if (slider != null)
            slider.freezeRotation = true;
    }

    public void Update()
    {
        if (isOpen || machine.State != PachinkoState.Movingclaw) return;

        if (Input.GetKey(KeyCode.A) && machine.LeftClawLimit.position.x < slider.transform.position.x)
        {
            MoveLeft();
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D) && machine.RightClawLimit.position.x > slider.transform.position.x)
        {
            MoveRight();
            isMoving = true;
        }
        else
        {
            isMoving = false;
            if (slider != null)
            {
                slider.velocity = new Vector2(0f, slider.velocity.y); 
                slider.angularVelocity = 0f; 
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && machine.State == PachinkoState.Movingclaw && !isMoving)
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
        if (itemPosition != null)
            itemPosition.position = new Vector3(slider.transform.position.x, slider.transform.position.y - offsetItem, 0);
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
            leftClaw.useMotor = true;
            rightClaw.useMotor = true;
        }
        else
        {
            leftMotor.motorSpeed = -closeAngle * 2;
            rightMotor.motorSpeed = closeAngle * 2;
            leftClaw.useMotor = false;
            rightClaw.useMotor = false;
        }
        leftMotor.maxMotorTorque = clawStrength;
        rightMotor.maxMotorTorque = clawStrength;
        leftClaw.motor = leftMotor;
        rightClaw.motor = rightMotor;
    }
}