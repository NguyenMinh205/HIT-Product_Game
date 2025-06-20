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
        get => this.isStart;
        set => this.isStart = value;
    }
    private void Awake()
    {
        this.isStart = false;
    }
    public void ChangeClaw()
    {
        if (claws.Count == 0)
        {
            Debug.Log("Claws have 0 claw -> next turn ");
            Debug.Log("Next Turn By Claw");
            GameController.Instance.isCheckTurnByClaw = true;
        }
        else
        {
            currentClaw = claws[0];
            StartClaw();
            claws.Remove(currentClaw);
            SetPosClaw();
        }
    }
    public void StartClaw()
    {
        if (currentClaw == null) 
        {
            Debug.Log("Current Claw is null");
            currentClaw = claws[0];
            claws.Remove(currentClaw);
            SetPosClaw();
        }
        currentClaw.Mode = ModeClaw.Start;
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
        Debug.Log("Reset -> Spawn");
        Spawn();
        ChangeClaw();
        StartClaw();
    }
    public void Spawn()
    {
        for (int i = 0; i < quantityClaws ; i++)
        {
            Debug.Log("Spawn Claw" + i.ToString());
            //ClawMachine newClaw = PoolingManager.Spawn(clawPrefabs, posSpawnClaws[i].position, Quaternion.identity, ClawParent);
            ClawMachine newClaw = Instantiate(clawPrefabs, posSpawnClaws[i].position, Quaternion.identity, ClawParent);
            newClaw.ClawController = this;
            newClaw.leftLimit = leftClawLimit;
            newClaw.rightLimit = rightClawLimit;
            newClaw.posStartClaw = posStartClaw;
            newClaw.lowLimit = lowClawLimit;
            newClaw.posStopClaw = posEndClaw;
            claws.Add(newClaw);
        }
    }

    public void EndGame()
    {
        for(int i=0; i < claws.Count; i++)
        {
            Destroy(claws[i].gameObject);
        }
        claws.Clear();
        Destroy(currentClaw.gameObject);
        currentClaw = null;
    }

}
