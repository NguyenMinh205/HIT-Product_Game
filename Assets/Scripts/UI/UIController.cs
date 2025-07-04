using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [Header("UI Machine")]
    [SerializeField] private GameObject PachinkoUI;
    [SerializeField] private GameObject TumblerUI;
    [SerializeField] private GameObject GachaUI;
    [SerializeField] private GameObject MapUI;

    private void Awake()
    {

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

