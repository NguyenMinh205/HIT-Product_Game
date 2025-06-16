using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [Header("Claw")]
    [SerializeField] private List<ClawMachine> claws;
    [SerializeField] private ClawMachine clawPrefabs;
    [SerializeField] private int quantityClaws;

    [Space]
    [Header("Position")]
    [SerializeField] private Transform posStartClaw;
    [SerializeField] private Transform posEndClaw;
    [SerializeField] private List<Transform> posSpawnClaws;
    [SerializeField] private Transform ClawParent;

    [Space]
    [Header("Limits")]
    [SerializeField] private GameObject leftClawLimit;
    [SerializeField] private GameObject rightClawLimit;
    [SerializeField] private GameObject lowClawLimit;


    private ClawMachine currentClaw;
    private ClawMachine endClaw;
    private bool isCurrent;
    private bool isStart;

    public bool IsStart
    {
        set => this.isStart = value;
    }
    private void Awake()
    {
        this.isStart = false;
    }
    private void Update()
    {
        Execute();
    }
    public void Execute()
    {
        if (!isStart) return;
        
        if (GameController.Instance.Turn == TurnPlay.Enemy)  return;

        if(GameController.Instance.IsChange01)
        {
            ResetMachineClaw();
            GameController.Instance.IsChange01 = false;
        }

        if (currentClaw == null)
        {
            ChangeClaw();
        }
        else if(currentClaw.Mode == ModeClaw.DeSpawn)
        {
            ChangeClaw();
        }
        
        //CheckCurrentClaw();

    }
    public void ChangeClaw()
    {
        if (claws.Count == 0)
        {
            checkNextTurn();
            return;
        }

        currentClaw = claws[0];
        StartClaw();

        claws.Remove(currentClaw);
        SetPosClaw();
    }
    public void StartClaw()
    {
        currentClaw.Mode = ModeClaw.Start;
    }
    public void CheckCurrentClaw()
    {
        
    }
    public void SetPosClaw()
    {
        for(int i = 0; i < claws.Count; i++)
        {
            claws[i].posMove = posSpawnClaws[i].position;
            claws[i].Mode = ModeClaw.AutoMove;
        }
    }
    public void ResetMachineClaw()
    {
        Debug.Log("Reset Claw Machine");
        claws.Clear();
        Spawn();
    }
    public void Spawn()
    {
        for (int i = 0; i < quantityClaws ; i++)
        {
            ClawMachine newClaw = PoolingManager.Spawn(clawPrefabs, posSpawnClaws[i].position, Quaternion.identity, ClawParent);
            newClaw.leftLimit = leftClawLimit;
            newClaw.rightLimit = rightClawLimit;
            newClaw.posStartClaw = posStartClaw;
            newClaw.lowLimit = lowClawLimit;
            newClaw.posStopClaw = posEndClaw;
            claws.Add(newClaw);
        }
    }

    public void checkNextTurn()
    {
        if (claws.Count == 0 && currentClaw == null)
        {
            GameController.Instance.Turn = TurnPlay.Enemy;
        }
    }

}
