using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChoicePlayer : MonoBehaviour
{
    [SerializeField] private CharacterDatabaseSO _characterDatabaseSO;
    [SerializeField] private GameObject characterDisplayObject;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI characterDescriptionTxt;
    [SerializeField] private TextMeshProUGUI abilityDescriptionTxt;
    [SerializeField] private Transform listStartItem;
    [SerializeField] private GameObject itemInventoryPrefab;
    [SerializeField] private GameObject lockIcon;

    private readonly float lockedSpriteAlpha = 120f / 255f;
    private readonly float unlockedSpriteAlpha = 1.0f;

    private Character curCharacter;
    private int selectedOption = 0;
    private int skinSelectOption = 0;

    private SpriteRenderer characterSpriteRenderer;
    private Animator characterAnimator;

    private void OnEnable()
    {
        if (_characterDatabaseSO != null)
        {
            _characterDatabaseSO.SetupStartData();
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
        InitializeCharacterDisplay();
        UpdateCharacter();
    }

    private void InitializeCharacterDisplay()
    {
        if (characterDisplayObject != null)
        {
            characterSpriteRenderer = characterDisplayObject.GetComponent<SpriteRenderer>();
            characterAnimator = characterDisplayObject.GetComponent<Animator>();
            if (characterSpriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer not found on CharacterDisplayObject!");
            }
            if (characterAnimator == null)
            {
                Debug.LogError("Animator not found on CharacterDisplayObject!");
            }
        }
        else
        {
            Debug.LogError("characterDisplayObject is not assigned!");
        }
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
        AudioManager.Instance.PlaySelectCharacter();
        skinSelectOption++;
        if (skinSelectOption >= curCharacter.skins.Count)
        {
            skinSelectOption = 0;
        }
        UpdateCharacterAnimator();
    }

    private void UpdateCharacterAnimator()
    {
        if (characterSpriteRenderer != null && characterAnimator != null)
        {
            if (curCharacter.skins[skinSelectOption].skin != null)
            {
                characterSpriteRenderer.sprite = curCharacter.skins[skinSelectOption].skin;
                Color color = characterSpriteRenderer.color;
                color.a = curCharacter.skins[skinSelectOption].isUnlocked && curCharacter.isUnlocked ? unlockedSpriteAlpha : lockedSpriteAlpha;
                characterSpriteRenderer.color = color;
            }
            else
            {
                Debug.LogWarning($"Skin sprite is null for skin {skinSelectOption} of character {curCharacter.name}");
            }
            if (curCharacter.skins[skinSelectOption].anim != null)
            {
                characterAnimator.runtimeAnimatorController = curCharacter.skins[skinSelectOption].anim;
                characterAnimator.SetTrigger("IsBuffing");
                Debug.Log($"Triggering 'IsBuffing' for skin {skinSelectOption} of character {curCharacter.name}");
            }
            else
            {
                Debug.LogWarning($"No animator controller assigned for skin {skinSelectOption} of character {curCharacter.name}");
            }
        }

        if (lockIcon != null)
        {
            bool isSkinUnlocked = curCharacter.skins[skinSelectOption].isUnlocked;
            bool isCharacterUnlocked = curCharacter.isUnlocked;
            lockIcon.SetActive(!isSkinUnlocked || !isCharacterUnlocked);
            Image lockImage = lockIcon.GetComponent<Image>();
            if (lockImage != null)
            {
                Color lockColor = lockImage.color;
                lockColor.a = (!isSkinUnlocked || !isCharacterUnlocked) ? 1.0f : 0.0f;
                lockImage.color = lockColor;
            }
        }
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
        UpdateCharacterAnimator();

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
            icon.rectTransform.sizeDelta *= 0.75f;
            newItemInventoryPrefab.GetComponentInChildren<TextMeshProUGUI>().SetText(item.quantity.ToString());
        }
    }

    public void ConfirmSelection()
    {
        if (curCharacter.isUnlocked && curCharacter.skins[skinSelectOption].isUnlocked)
        {
            SaveSelection(curCharacter, skinSelectOption);
            StartSceneManager.Instance.OnDifficultyButton();
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