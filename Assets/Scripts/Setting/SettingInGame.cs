using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInGame : Singleton<SettingInGame>
{
    [SerializeField] private GameObject dime;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private GameObject optionUI;
    public bool isPause = false;

    public void PauseAndShowSetting()
    {
        AudioManager.Instance.PlaySoundClickButton();
        dime.SetActive(true);
        settingUI.SetActive(true);
        isPause = true;
        Time.timeScale = 0;
    }

    public void Continue()
    {
        Time.timeScale = 1;
        AudioManager.Instance.PlaySoundClickButton();
        dime.SetActive(false);
        settingUI.SetActive(false);
        isPause = false;
    }    

    public void ShowOption()
    {
        AudioManager.Instance.PlaySoundClickButton();
        optionUI.SetActive(true);
        dime.SetActive(false);
        settingUI.SetActive(false);
    }    

    public void BackToSettingUI()
    {
        AudioManager.Instance.PlaySoundClickButton();
        optionUI.SetActive(false);
        dime.SetActive(true);
        settingUI.SetActive(true);
    }    
}
