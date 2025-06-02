using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnPlay
{
       Player,
       Enemy
}


public class GameController : Singleton<GameController>
{
    [SerializeField] private TurnPlay turnGame;
    private bool isChange01;
    private bool isChange02;

    private void Awake()
    {
        turnGame = TurnPlay.Player;
        isChange01 = false;
        isChange02 = false;
    }
    public TurnPlay Turn
    {
        get => this.turnGame;
        set 
        {
            if (this.turnGame != value)
            {
                this.turnGame = value;
                this.isChange01 = true;
                this.isChange02 = true;
            }
            else
            {
                this.isChange01 = false;
                this.isChange02 = false;
            }
        }
    }
    
    public bool IsChange01
    {
        get => this.isChange01;
        set => this.isChange01 = value;
    }

    public bool IsChange02
    {
        get => this.isChange02;
        set => this.isChange02 = value;
    }
}

