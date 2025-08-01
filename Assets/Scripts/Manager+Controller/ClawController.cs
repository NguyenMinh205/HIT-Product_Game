using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject lowClawMagnetLimit;

    private bool isListClawNull;
    public bool IsListClawNull => isListClawNull;
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

    }
    public void RemoveCurClaw()
    {
        currentClaw = null;
    }
    public void SetCurrentClaw()
    {
        if (currentClaw == null && claws.Count > 0) 
        {
            currentClaw = claws[0];
            claws.Remove(currentClaw);
            currentClaw.Mode = ModeClaw.Start;
            SetPosClaw();
            ObserverManager<EventID>.PostEven(EventID.OnUseClaw);
        }
        checkListClaw();
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
        if (!isStart) return;

        Debug.Log("Reset Claw Machine");
        claws.Clear();
        Debug.Log("Reset -> Spawn");
        Spawn();
        ChangeClaw();
        SetCurrentClaw();
    }
    public void Spawn(int quality = 0)
    {
        if (GamePlayController.Instance.IsEndGame)  return;
 
        if(quality > 0)
        {
            quantityClaws = quality;
        }
        else
        {
            quantityClaws = GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.ClawPerTurn;
        }

        SpawnClaw(quantityClaws);
    }
    public void SpawnClaw(int val)
    {
        if (val <= 0) return;

        for (int i = 0; i < val; i++)
        {
            if (clawPrefabs == null) return;
            ClawMachine newClaw = Instantiate(clawPrefabs, posSpawnClaws[i].position, Quaternion.identity, ClawParent);
            newClaw.ClawController = this;
            newClaw.leftLimit = leftClawLimit;
            newClaw.rightLimit = rightClawLimit;
            newClaw.posStartClaw = posStartClaw;

            if (newClaw is Magnet_Claw magnetClaw)
            {
                magnetClaw.lowLimit = lowClawMagnetLimit;
            }
            else newClaw.lowLimit = lowClawLimit;

            newClaw.posStopClaw = posEndClaw;
            claws.Add(newClaw);
        }
        checkListClaw();
    }
    public void checkListClaw()
    {
        if (claws.Count <= 0 && currentClaw == null)
            isListClawNull = true;
        else
            isListClawNull = false;
        GamePlayController.Instance.CheckTurnPlayer();
    }

    public void EndGame()
    {
        for(int i=0; i < claws.Count; i++)
        {
            Destroy(claws[i].gameObject);
        }
        claws.Clear();
        if (currentClaw != null)
            Destroy(currentClaw.gameObject);
    }

}
