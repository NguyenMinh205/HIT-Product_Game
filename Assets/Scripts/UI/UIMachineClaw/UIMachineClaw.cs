using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIMahcine
{
    OnUI,
}
public class UIMachineClaw : MonoBehaviour
{
    [SerializeField] private GameObject listPerks;
    private void Awake()
    {
        ObserverManager<UIMahcine>.AddDesgisterEvent(UIMahcine.OnUI, SetUIMachine);
    }
    private void OnDisable()
    {
        ObserverManager<UIMahcine>.RemoveAddListener(UIMahcine.OnUI, SetUIMachine);
    }

    public void SetUIMachine(object obj)
    {
        if (obj is bool val)
        {
            listPerks.SetActive(val);
        }
    }
}
