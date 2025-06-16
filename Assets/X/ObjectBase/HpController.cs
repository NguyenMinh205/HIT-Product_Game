using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : Singleton<HpController>
{
    [SerializeField] private List<HPManager> listHp;

    public void showHp(int index)
    {
        listHp[index].gameObject.SetActive(true);
    }
    public void SetHp(int index, int hp,int currenHp)
    {
        listHp[index].SetHp(hp, currenHp);
    }
}
