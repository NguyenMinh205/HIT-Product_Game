using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TranDuc;

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
    [SerializeField] private GameObject choiceKeepPlayingUI;
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

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        DOVirtual.DelayedCall(0.2f, () =>
        {
            AudioManager.Instance.PlayMusicStartGame();
        });
        characterDatabaseSO.SetupStartData();
    }

    public void OnStartButton()
    {
        AudioManager.Instance.PlaySoundClickButton();

        if (DataManager.Instance.GameData.IsKeepingPlayGame)
        {
            choiceKeepPlayingUI.SetActive(true);
            return;
        }
        characterSelectionScreen.SetActive(true);
        startScreen.gameObject.SetActive(false);
    }

    public void OnChoiceKeepPlayingButton()
    {
        AudioManager.Instance.PlaySoundClickButton();
        SceneManager.LoadScene(1);
    }

    public void OnChoiceNewGameButton()
    {
        AudioManager.Instance.PlaySoundClickButton();
        DataManager.Instance.GameData.ClearGameplayData();
        DataManager.Instance.GameData.SetKeepPlayState(false);
        characterSelectionScreen.SetActive(true);
        startScreen.gameObject.SetActive(false);
        choiceKeepPlayingUI.SetActive(false);
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
        DataManager.Instance.GameData.SetKeepPlayState(false);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}