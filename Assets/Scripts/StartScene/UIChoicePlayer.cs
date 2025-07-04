using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UIChoicePlayer : MonoBehaviour
{
    [SerializeField] private CharacterDatabaseSO _characterDatabaseSO;
    [SerializeField] private Image characterSprite;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI characterDescriptionTxt;
    [SerializeField] private TextMeshProUGUI abilityDescriptionTxt;
    [SerializeField] private Transform listStartItem;
    [SerializeField] private GameObject itemInventoryPrefab;
    [SerializeField] private Image lockIcon;
    private readonly float lockedSpriteAlpha = 120f / 255f;
    private readonly float unlockedSpriteAlpha = 1.0f;

    private Character curCharacter;
    private int selectedOption = 0;
    private int skinSelectOption = 0;

    private void OnEnable()
    {
        if (_characterDatabaseSO != null)
        {
            _characterDatabaseSO.LoadUnlockedStates();
        }

        if (_characterDatabaseSO != null && _characterDatabaseSO.CharacterCount() > 0)
        {
            string savedCharacterId = PlayerPrefs.GetString("SelectedCharacterId", "");
            int savedSkinIndex = PlayerPrefs.GetInt("SelectedSkinIndex", 0);

            if (!string.IsNullOrEmpty(savedCharacterId))
            {
                Character savedCharacter = _characterDatabaseSO.GetCharacterById(savedCharacterId);
                if (savedCharacter != null)
                {
                    selectedOption = _characterDatabaseSO.characters.FindIndex(c => c.id == savedCharacterId);
                    skinSelectOption = savedSkinIndex < savedCharacter.skins.Count ? savedSkinIndex : 0;
                }
            }
        }
        UpdateCharacter();
    }

    public void NextOption()
    {
        AudioManager.Instance.PlaySelectCharacter();
        selectedOption++;
        if (selectedOption >= _characterDatabaseSO.CharacterCount())
        {
            selectedOption = 0;
        }
        UpdateCharacter();
    }

    public void PrevOption()
    {
        AudioManager.Instance.PlaySelectCharacter();
        selectedOption--;
        if (selectedOption < 0)
        {
            selectedOption = _characterDatabaseSO.CharacterCount() - 1;
        }
        UpdateCharacter();
    }

    public void ChangeSkin()
    {
        AudioManager.Instance.PlaySoundClickButton();
        skinSelectOption++;
        if (skinSelectOption >= curCharacter.skins.Count)
        {
            skinSelectOption = 0;
        }
        UpdateCharacterSprite();
    }

    private void UpdateCharacterSprite()
    {
        characterSprite.sprite = curCharacter.skins[skinSelectOption].skin;
        bool isSkinUnlocked = curCharacter.skins[skinSelectOption].isUnlocked;
        bool isCharacterUnlocked = curCharacter.isUnlocked;
        float alpha = isSkinUnlocked ? unlockedSpriteAlpha : lockedSpriteAlpha;
        characterSprite.color = new Color(1, 1, 1, alpha);
        lockIcon.gameObject.SetActive(!isSkinUnlocked || !isCharacterUnlocked);
    }

    public void UpdateCharacter()
    {
        if (_characterDatabaseSO == null || _characterDatabaseSO.CharacterCount() == 0)
        {
            Debug.LogError("CharacterDatabaseSO is null or empty!");
            return;
        }

        curCharacter = _characterDatabaseSO.GetCharacter(selectedOption);
        skinSelectOption = 0;
        UpdateCharacterSprite();

        nameTxt.text = curCharacter.name;
        characterDescriptionTxt.text = curCharacter.description;
        abilityDescriptionTxt.text = curCharacter.abilityDescription;

        foreach (Transform child in listStartItem)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemInventory item in curCharacter.initialItems)
        {
            GameObject newItemInventoryPrefab = Instantiate(itemInventoryPrefab, listStartItem);
            Image icon = newItemInventoryPrefab.GetComponent<Image>();
            icon.sprite = item.itemBase.icon;
            icon.SetNativeSize();
            icon.rectTransform.sizeDelta *= 0.65f;
            newItemInventoryPrefab.GetComponentInChildren<TextMeshProUGUI>().SetText(item.quantity.ToString());
        }
    }

    public void ConfirmSelection()
    {
        if (curCharacter.isUnlocked && curCharacter.skins[skinSelectOption].isUnlocked)
        {
            SaveSelection(curCharacter, skinSelectOption);
        }
        else
        {
            Debug.LogWarning("Cannot confirm selection: Character or skin is locked.");
        }
    }

    public void SaveSelection(Character character, int skinIndex)
    {
        PlayerPrefs.SetString("SelectedCharacterId", character.id);
        PlayerPrefs.SetInt("SelectedSkinIndex", skinIndex);
        PlayerPrefs.Save();
    }
}