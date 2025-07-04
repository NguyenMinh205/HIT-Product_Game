using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIActionEnemy : MonoBehaviour
{
    [SerializeField] private List<ActionEnemy> listAciton; // 2-1-3


    public void SetAction(Sprite icon, int index, int damage)
    {
        listAciton[index].SetUIAction(icon, damage);
    }
    public void UnActionIndexEnemy(int index)
    {
        listAciton[index].UnShow();
    }
    public void UnShowActionEnemy()
    {
        gameObject.SetActive(false);
    }

    public void Execute(int index)
    {
        listAciton[index].UnShow();
    }
}

