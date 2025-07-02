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
    //[SerializeField] private TextMeshProUGUI listItemStartTxt;

    private Character curCharacter;
    private int selectedOption = 0;
    private int skinSelectOption = 0;

    private void OnEnable()
    {
        UpdateCharacter();
    }
    public void NextOption()
    {
        AudioManager.Instance.PlaySoundClickButton();
        selectedOption++;
        if (selectedOption == _characterDatabaseSO.CharacterCount())
        {
            selectedOption = 0;
        }
        UpdateCharacter();
    }   

    public void PrevOption()
    {
        AudioManager.Instance.PlaySoundClickButton();
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

        if (skinSelectOption == curCharacter.skins.Count)
        {
            skinSelectOption = 0;
        }    

        characterSprite.sprite = curCharacter.skins[skinSelectOption];
    }    
    
    public void UpdateCharacter()
    {
        curCharacter = _characterDatabaseSO.GetCharacter(selectedOption);
        skinSelectOption = 0;
        characterSprite.sprite = curCharacter.skins[0];
        nameTxt.text = curCharacter.name;
        characterDescriptionTxt.text = curCharacter.description;
        abilityDescriptionTxt.text = curCharacter.abilityDescription;
        foreach (Transform child in listStartItem)
        {
            Destroy(child.gameObject);
        }    
        foreach (ItemInventory item in curCharacter.initialItems)
        {
            GameObject newItemInventoryPrefab = Instantiate(itemInventoryPrefab, this.transform.position, Quaternion.identity, listStartItem);
            Image icon = newItemInventoryPrefab.GetComponent<Image>();
            icon.sprite = item.itemBase.icon;
            icon.SetNativeSize();
            icon.rectTransform.sizeDelta *= 0.65f;
            newItemInventoryPrefab.GetComponentInChildren<TextMeshProUGUI>().SetText((item.quantity).ToString());
        } 
            
    }

    public void ConfirmSelection()
    {
        SaveSelection(curCharacter, skinSelectOption);
    }

    public void SaveSelection(Character character, int skinIndex)
    {
        PlayerPrefs.SetString("SelectedCharacterId", character.id);
        PlayerPrefs.SetInt("SelectedSkinIndex", skinIndex);
        PlayerPrefs.Save();
    }
}
