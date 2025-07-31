using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

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

    protected override void Awake()
    {
        base.Awake();
        GameData.Instance.LoadStartGameData();
    }

    private void Start()
    {
        DOVirtual.DelayedCall(0.2f, () =>
        {
            AudioManager.Instance.PlayMusicStartGame();
        });
    }

    public void OnStartButton()
    {
        AudioManager.Instance.PlaySoundClickButton();

        if (GameData.Instance.startData.isKeepingPlayGame)
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
        GameData.Instance.ClearMainGameData();
        GameData.Instance.startData.isKeepingPlayGame = false;
        GameData.Instance.SaveStartGameData();
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
        GameData.Instance.startData.isKeepingPlayGame = false;
        GameData.Instance.SaveStartGameData();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        GameData.Instance.SaveStartGameData();
    }
}