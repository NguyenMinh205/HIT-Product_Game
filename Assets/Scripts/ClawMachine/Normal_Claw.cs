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
    public override void PickUp()
    {
        if (chain.transform.position.y > lowLimit.transform.position.y)
        {
            rb.velocity = Vector2.down * moveForce;
        }
        else
        {
            rb.velocity = Vector2.zero;
            CloseClaw();
            StartCoroutine(DelayPickUp(1.5f));
        }
    }

    protected override IEnumerator DelayOpen(float time)
    {
        yield return new WaitForSeconds(time);
        OpenClaw();
        yield return new WaitForSeconds(time);

        if (mode != ModeClaw.DeSpawn)
        {
            mode = ModeClaw.DeSpawn;
            if (clawController != null)
                clawController.SetCurrentClaw();
        }
    }

    public void OpenClaw()
    {
        SetClawStrength(clawStrength, true);
    }

    public void CloseClaw()
    {
        SetClawStrength(clawStrength * 2, false);
    }

    private void SetClawStrength(float strength, bool isOpening)
    {
        if (leftClaw == null || rightClaw == null) return;

        var leftMotor = leftClaw.motor;
        var rightMotor = rightClaw.motor;

        if (isOpening)
        {
            leftMotor.motorSpeed = strength * closeAngle;
            rightMotor.motorSpeed = -strength * closeAngle;
        }
        else
        {
            leftMotor.motorSpeed = -strength * closeAngle;
            rightMotor.motorSpeed = strength * closeAngle;
        }

        leftMotor.maxMotorTorque = Mathf.Abs(strength * 2);
        rightMotor.maxMotorTorque = Mathf.Abs(strength * 2);

        leftClaw.motor = leftMotor;
        rightClaw.motor = rightMotor;
    }
}
