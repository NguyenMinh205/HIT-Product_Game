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

    void Start()
    {
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

    public void SaveDifficulty(float extraDamagePercent, float extraHealthPercent)
    {
        PlayerPrefs.SetFloat("ExtraDamagePercent", extraDamagePercent);
        PlayerPrefs.SetFloat("ExtraHealthPercent", extraHealthPercent);
        PlayerPrefs.Save();
    }

    public void OnDisable()
    {
        SaveDifficulty(extraDamagePercent, extraHealthPercent);
    }
}