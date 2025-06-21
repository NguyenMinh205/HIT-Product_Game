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
    [SerializeField] private GameObject compendiumScreen;
    [SerializeField] private GameObject optionScreen;
    [SerializeField] private GameObject characterSelectionScreen;
    [SerializeField] private GameObject difficultySelectionScreen;

    [Space]
    [Header("GachaScreen")]
    [SerializeField] private GameObject gachaScreen;
    [SerializeField] private GameObject gachaMachine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButton()
    {
        characterSelectionScreen.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnCompendiumButton()
    {
        compendiumScreen.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnOptionButton()
    {
        optionScreen.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OnGachaButton()
    {
        gachaScreen.SetActive(true);
        gachaMachine.SetActive(true);
        this.gameObject.SetActive(false);
    }
    
    public void OnDifficultyButton()
    {
        characterSelectionScreen.SetActive(false);
        difficultySelectionScreen.SetActive(true);
    }

    public void BackScreen(GameObject screen)
    {
        screen.SetActive(false);
        this.gameObject.SetActive(true);
    }
    public void BackScreenFromGacha()
    {
        gachaScreen.SetActive(false);
        gachaMachine.SetActive(false);
        this.gameObject.SetActive(true);
    }
    public void BackScreenFromDifficulty()
    {
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
