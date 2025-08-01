using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChoiceDifficulty : MonoBehaviour
{
    [SerializeField] private List<Difficulty> difficultyList;
    [SerializeField] private Image difficultyImage;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI descriptionLevelTxt;

    private float extraDamagePercent;
    private float extraHealthPercent;
    private int selectIndex = 0;

    void OnEnable()
    {
        if (GameData.Instance != null) GameData.Instance.LoadStartGameData();
        selectIndex = GameData.Instance?.startData.selectedDifficultyIndex ?? 0;
        extraDamagePercent = GameData.Instance?.startData.extraDamagePercent ?? 0f;
        extraHealthPercent = GameData.Instance?.startData.extraHealthPercent ?? 0f;
        UpdateChange();
    }

    public void NextOption()
    {
        AudioManager.Instance?.PlaySoundClickButton();
        selectIndex = (selectIndex + 1) % difficultyList.Count;
        UpdateChange();
    }

    public void PrevOption()
    {
        AudioManager.Instance?.PlaySoundClickButton();
        selectIndex = (selectIndex - 1 + difficultyList.Count) % difficultyList.Count;
        UpdateChange();
    }

    public void UpdateChange()
    {
        if (difficultyList == null || difficultyList.Count == 0) return;

        Difficulty currentDifficulty = difficultyList[selectIndex];
        difficultyImage.sprite = currentDifficulty.describeDifficulty;
        levelTxt.text = currentDifficulty.level;
        descriptionLevelTxt.text = currentDifficulty.levelDescription;
        extraDamagePercent = currentDifficulty.extraDamagePercent;
        extraHealthPercent = currentDifficulty.extraHealthPercent;
    }

    public void SaveDifficulty()
    {
        GameData.Instance.startData.selectedDifficultyIndex = selectIndex;
        GameData.Instance.startData.extraDamagePercent = extraDamagePercent;
        GameData.Instance.startData.extraHealthPercent = extraHealthPercent;
        GameData.Instance.SaveStartGameData();
    }

    void OnDisable()
    {
        SaveDifficulty();
    }
}