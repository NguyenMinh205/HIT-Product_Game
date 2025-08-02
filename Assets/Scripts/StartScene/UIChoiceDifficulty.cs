using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TranDuc;

public class UIChoiceDifficulty : MonoBehaviour
{
    [SerializeField] private List<Difficulty> difficultyList;
    [SerializeField] private Image difficultyImage;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI descriptionLevelTxt;

    private float extraDamagePercent;
    private float extraHealthPercent;

    private int selectIndex = 0;

    void Start()
    {
        GameData.Instance.LoadStartGameData();
        selectIndex = GameData.Instance.startData.selectedDifficultyIndex;
        extraDamagePercent = GameData.Instance.startData.extraDamagePercent;
        extraHealthPercent = GameData.Instance.startData.extraHealthPercent;
        UpdateChange();
    }

    public void NextOption()
    {
        AudioManager.Instance.PlaySoundClickButton();
        selectIndex++;
        if (selectIndex >= difficultyList.Count)
        {
            selectIndex = 0;
        }
        UpdateChange();
    }

    public void PrevOption()
    {
        AudioManager.Instance.PlaySoundClickButton();
        selectIndex--;
        if (selectIndex < 0)
        {
            selectIndex = difficultyList.Count - 1;
        }
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