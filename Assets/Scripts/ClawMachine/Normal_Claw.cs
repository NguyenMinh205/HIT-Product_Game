using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Claw : ClawMachine
{
    [SerializeField] private HingeJoint2D leftClaw;
    [SerializeField] private HingeJoint2D rightClaw;

    [SerializeField] private float closeAngle = 45f;
    [SerializeField] private float clawStrength = 25f;

    public override void StartClaw()
    {
        Debug.Log("Start Normal Claw");
        if (chain.transform.position.x <= posStartClaw.position.x)
            rb.velocity = Vector2.right * base.moveForce;
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
    protected override IEnumerator DelayPickUp(float time)
    {
        CloseClaw();
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
        OpenClaw(); // Gọi OpenClaw của Normal_Claw
        yield return new WaitForSeconds(time);

        if (mode != ModeClaw.DeSpawn)
        {
            mode = ModeClaw.DeSpawn;
            if (clawController != null)
                clawController.ChangeClaw();
        }
    }

    public void OpenClaw()
    {
        Debug.Log("Open Normal Claw");
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
        Debug.Log("Close Normal Claw");
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
