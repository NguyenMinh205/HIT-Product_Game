using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInGame : Singleton<SettingInGame>
{
    [SerializeField] private GameObject dime;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private GameObject optionUI;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button backToSettingButton;
    public bool isPause = false;

    private void Start()
    {
        pauseButton.onClick.AddListener(Pause);
        continueButton.onClick.AddListener(Continue);
        optionButton.onClick.AddListener(ShowOption);
        backToSettingButton.onClick.AddListener(BackToSettingUI);
    }

    public void Pause ()
    {
        AudioManager.Instance.PlaySoundClickButton();
        dime.SetActive(true);
        settingUI.SetActive(true);
        isPause = true;
    }

    public void Continue()
    {
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
