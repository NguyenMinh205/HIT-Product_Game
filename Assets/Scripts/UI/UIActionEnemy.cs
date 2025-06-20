using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIActionEnemy : MonoBehaviour
{
    [SerializeField] private List<ActionEnemy> listAciton;

    private Enemy enemy;

    public Enemy Enemy
    {
        get => this.enemy;
        set => this.enemy = value;
    }


    public void Execute(int index)
    {
        listAciton[index].UnShow();
    }
    public void OnShowAttack(int index, int damage)
    {
        listAciton[index].AttackShow(damage);
    }
    public void OnShowShield(int index)
    {
        listAciton[index].ShieldShow();
    }

}

