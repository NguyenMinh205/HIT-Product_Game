using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private InfoManager uiShow;
    [SerializeField] private Transform uiParent;

    [SerializeField] private UIActionEnemy actionEnemyPrefabs;
    [SerializeField] private Transform actionParent;
    private void Awake()
    {
        ObserverManager<IDInfoObject>.AddDesgisterEvent(IDInfoObject.ShowInfo, ShowInfo);
    }

    public void ShowInfo(object obj)
    {
        if (obj is ObjectBase objInfo)
        {
            Debug.Log("UI");
            Vector3 pos = objInfo.gameObject.transform.position;
            pos.y -= 0.8f;
            InfoManager info = PoolingManager.Spawn(uiShow, pos, Quaternion.identity, uiParent);
            info.Obj = objInfo;
            info.UpdateHp();
            info.UpdateArmor();
            info.Obj.Info = info;
            Debug.Log("Show Action");
            ShowAction(obj, pos);
        }
        else
        {
            Debug.LogWarning("ShowInfo expected Vector3 but received: " + obj.GetType());
        }
    }

    public void ShowAction(object obj, Vector3 pos)
    {
        if (obj is Enemy enemy)
        {
            pos.y = pos.y + 0.8f + 1f;
            UIActionEnemy ui = PoolingManager.Spawn(actionEnemyPrefabs, pos, Quaternion.identity, actionParent);
            ui.Enemy = enemy;
            ui.Enemy.UI = ui;
        }
    }
}

