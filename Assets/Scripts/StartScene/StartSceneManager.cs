using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : Singleton<StartSceneManager>
{
    [Header("Button")]
    [SerializeField] private Button startBtn;
    [SerializeField] private Button compendiumBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button gachaBtn;
    [SerializeField] private Button quitBtn;

    [Space]
    [Header("Screen")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject compendiumScreen;
    [SerializeField] private GameObject optionScreen;
    [SerializeField] private GameObject characterSelectionScreen;
    [SerializeField] private GameObject difficultySelectionScreen;

    [Space]
    [Header("GachaScreen")]
    [SerializeField] private GameObject gachaScreen;
    [SerializeField] private GameObject gachaMachine;

    [Space]
    [Header("UI Choice Character")]
    [SerializeField] private UIChoicePlayer uiChoicePlayer;
    [SerializeField] private CharacterDatabaseSO characterDatabaseSO;
    public UIChoicePlayer _UIChoicePlayer => uiChoicePlayer;

    private void Awake()
    {
        characterDatabaseSO.SetupStartData();
    }
    public void OnStartButton()
    {
        AudioManager.Instance.PlaySoundClickButton();
        characterSelectionScreen.SetActive(true);
        startScreen.gameObject.SetActive(false);
    }

    public void OnCompendiumButton()
    {
        AudioManager.Instance.PlaySoundClickButton();
        compendiumScreen.SetActive(true);
        startScreen.gameObject.SetActive(false);
    }

    public void OnOptionButton()
    {
        AudioManager.Instance.PlaySoundClickButton();
        optionScreen.SetActive(true);
        startScreen.gameObject.SetActive(false);
    }

    public void OnGachaButton()
    {
        AudioManager.Instance.PlaySoundClickButton();
        gachaScreen.SetActive(true);
        gachaMachine.SetActive(true);
        startScreen.gameObject.SetActive(false);
    }
    
    public void OnDifficultyButton()
    {
        AudioManager.Instance.PlaySoundClickButton();
        characterSelectionScreen.SetActive(false);
        difficultySelectionScreen.SetActive(true);
    }

    public void BackScreen(GameObject screen)
    {
        AudioManager.Instance.PlaySoundClickButton();
        screen.SetActive(false);
        startScreen.gameObject.SetActive(true);
    }
    public void BackScreenFromGacha()
    {
        AudioManager.Instance.PlaySoundClickButton();
        gachaScreen.SetActive(false);
        gachaMachine.SetActive(false);
        startScreen.gameObject.SetActive(true);
    }
    public void BackScreenFromDifficulty()
    {
        AudioManager.Instance.PlaySoundClickButton();
        characterSelectionScreen.SetActive(true);
        difficultySelectionScreen.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }    

    public void QuitGame()
    {
        Application.Quit();
    }
}
