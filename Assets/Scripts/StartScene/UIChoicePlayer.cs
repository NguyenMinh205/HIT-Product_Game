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
            string savedCharacterId = GameData.Instance != null ? GameData.Instance.startData.selectedCharacterId : string.Empty;
            int savedSkinIndex = GameData.Instance != null ? GameData.Instance.startData.selectedSkinIndex : 0;

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
        }
    }

    public void NextOption()
    {
        AudioManager.Instance?.PlaySelectCharacter();
        selectedOption = (selectedOption + 1) % _characterDatabaseSO.CharacterCount();
        UpdateCharacter();
    }

    public void PrevOption()
    {
        AudioManager.Instance?.PlaySelectCharacter();
        selectedOption = (selectedOption - 1 + _characterDatabaseSO.CharacterCount()) % _characterDatabaseSO.CharacterCount();
        UpdateCharacter();
    }

    public void ChangeSkin()
    {
        AudioManager.Instance?.PlaySelectCharacter();
        skinSelectOption = (skinSelectOption + 1) % curCharacter.skins.Count;
        UpdateCharacterAnimator();
    }

    private void UpdateCharacterAnimator()
    {
        if (characterSpriteRenderer == null || characterAnimator == null || curCharacter == null) return;

        Skin skin = curCharacter.skins[skinSelectOption];
        if (skin.skin != null)
        {
            characterSpriteRenderer.sprite = skin.skin;
            Color color = characterSpriteRenderer.color;
            color.a = (skin.isUnlocked && curCharacter.isUnlocked) ? unlockedSpriteAlpha : lockedSpriteAlpha;
            characterSpriteRenderer.color = color;
        }

        if (skin.anim != null && !characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Buffing"))
        {
            characterAnimator.runtimeAnimatorController = skin.anim;
            characterAnimator.SetTrigger("IsBuffing");
        }

        if (lockIcon != null)
        {
            bool isLocked = !(skin.isUnlocked && curCharacter.isUnlocked);
            lockIcon.SetActive(isLocked);
            Image lockImage = lockIcon.GetComponent<Image>();
            if (lockImage != null)
            {
                Color lockColor = lockImage.color;
                lockColor.a = isLocked ? 1.0f : 0.0f;
                lockImage.color = lockColor;
            }
        }
    }

    public void UpdateCharacter()
    {
        if (_characterDatabaseSO == null || _characterDatabaseSO.CharacterCount() == 0) return;

        curCharacter = _characterDatabaseSO.GetCharacter(selectedOption);
        skinSelectOption = 0;
        UpdateCharacterAnimator();

        nameTxt.text = curCharacter.name;
        characterDescriptionTxt.text = curCharacter.description;
        abilityDescriptionTxt.text = curCharacter.abilityDescription;

        foreach (Transform child in listStartItem)
        {
            PoolingManager.Despawn(child.gameObject);
        }

        if (curCharacter.initialItems != null)
        {
            List<(ItemInventory item, GameObject obj)> itemOrder = new List<(ItemInventory, GameObject)>();
            foreach (ItemInventory item in curCharacter.initialItems)
            {
                GameObject newItem = PoolingManager.Spawn(itemInventoryPrefab, Vector3.zero, Quaternion.identity, listStartItem);
                Image icon = newItem.GetComponent<Image>();
                ItemBase itemBase = item.GetItemBase();
                if (itemBase != null)
                {
                    icon.sprite = itemBase.icon;
                    icon.SetNativeSize();
                    icon.rectTransform.sizeDelta *= 0.75f;
                    newItem.GetComponentInChildren<TextMeshProUGUI>().text = item.quantity.ToString();
                }
                itemOrder.Add((item, newItem));
            }

            for (int i = 0; i < itemOrder.Count; i++)
            {
                itemOrder[i].obj.transform.SetSiblingIndex(i);
            }
            Canvas.ForceUpdateCanvases();
        }
    }

    public void ConfirmSelection()
    {
        if (curCharacter != null && curCharacter.isUnlocked && curCharacter.skins[skinSelectOption].isUnlocked)
        {
            if (GameData.Instance != null)
            {
                GameData.Instance.startData.selectedCharacterId = curCharacter.id;
                GameData.Instance.startData.selectedSkinIndex = skinSelectOption;
                GameData.Instance.SaveStartGameData();
            }
            StartSceneManager.Instance.OnDifficultyButton();
        }
    }
}