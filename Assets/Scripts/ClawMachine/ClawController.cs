using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [Header("Claw")]
    [SerializeField] private List<ClawMachine> claws;
    [SerializeField] private ClawMachine clawPrefabs;

    [Space]
    [Header("Position")]
    [SerializeField] private Transform posStartClaw;
    [SerializeField] private Transform posEndClaw;
    [SerializeField] private List<Transform> posSpawnClaws;
    [SerializeField] private Transform ClawParent;


    private ClawMachine currentClaw;
    private ClawMachine endClaw;
    private bool isCurrent;

    private void Start()
    {
        ResetMachineClaw();
    }
    private void Update()
    {
        Execute();
    }
    public void Execute()
    {
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
        
        CheckCurrentClaw();


    }
    public void ChangeClaw()
    {
        if (claws.Count == 0) return;

        currentClaw = claws[0];
        StartClaw();

        claws.Remove(currentClaw);
        SetPosClaw();
    }
    public void StartClaw()
    {
        currentClaw.StartClaw(posStartClaw.position);
    }
    public void CheckCurrentClaw()
    {
        if(currentClaw.Mode == ModeClaw.End)
        {
            currentClaw.EndClaw(posEndClaw.position);
            if(currentClaw.Move == 5)
            {
                ChangeClaw();
            }
        }
    }
    public void SetPosClaw()
    {
        for(int i = 0; i < claws.Count; i++)
        {
            claws[i].SetMovePos(posSpawnClaws[i].position);
        }
    }
    public void ResetMachineClaw()
    {
        claws.Clear();
        Spawn();
    }
    public void SetTurnPlayer()
    {
        if(claws.Count == 0)
        {
            GameController.Instance.Turn = TurnPlay.Enemy;
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            GameController.Instance.Turn = TurnPlay.Player;
        }
    }
    public void Spawn()
    {
        for (int i = 0; i < posSpawnClaws.Count; i++)
        {
            ClawMachine newClaw = PoolingManager.Spawn(clawPrefabs, posSpawnClaws[i].position, Quaternion.identity, ClawParent);
            claws.Add(newClaw);
        }
    }
}
