using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject uiShow;
    [SerializeField] private Transform uiParent;


    private void Awake()
    {
        ObserverManager<IDInfoObject>.AddDesgisterEvent(IDInfoObject.ShowInfo, ShowInfo);
    }

    public void ShowInfo(object obj)
    {
        if (obj is Vector3 pos)
        {
            pos.y -= 3;
            PoolingManager.Spawn(uiShow, pos, Quaternion.identity, uiParent);
        }
        else
        {
            Debug.LogWarning("ShowInfo expected Vector3 but received: " + obj.GetType());
        }
    }
}

