using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private InfoManager uiShow;
    [SerializeField] private Transform uiParent;

    [SerializeField] private UIActionEnemy actionEnemyPrefabs;
    [SerializeField] private Transform actionParent;

    [Space]
    [Header("UI Machine")]
    [SerializeField] private GameObject PachinkoUI;
    [SerializeField] private GameObject TumblerUI;
    [SerializeField] private GameObject GachaUI;
    [SerializeField] private GameObject MapUI;

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
            pos.y = pos.y + 0.8f + 0.5f;
            UIActionEnemy ui = PoolingManager.Spawn(actionEnemyPrefabs, pos, Quaternion.identity, actionParent);
            ui.Enemy = enemy;
            ui.Enemy.UI = ui;
        }
    }
    public void SetActiveMapUI(bool value)
    {
        MapUI.SetActive(value);
    }
    public void OpenPachinkoUI(bool var)
    {
        PachinkoUI.SetActive(var);
    }
    public void OpenTumblerUI(bool var)
    {
        TumblerUI.SetActive(var);
    }
    public void OpenGaChaUI(bool var)
    {
        GachaUI.SetActive(var);
    }    
}

