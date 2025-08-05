using UnityEngine;
using System.Collections.Generic;
using TranDuc;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Character/CharacterDatabase")]
public class CharacterDatabaseSO : ScriptableObject
{
    public List<Character> characters;

    public void SetupStartData()
    {
        if (DataManager.Instance.GameData.CharacterStates.Count > 0)
        {
            Debug.Log("Character states already initialized, skipping default save.");
            return;
        }
        List<CharacterState> lstCharacter = new List<CharacterState>();
        foreach (Character character in characters)
        {
            CharacterState state = new CharacterState
            {
                id = character.id,
                isUnlocked = character.isUnlocked,
                skinUnlocks = new List<bool>()
            };
            if (character.skins != null)
            {
                foreach (var skin in character.skins)
                {
                    state.skinUnlocks.Add(skin.isUnlocked);
                }
            }
            lstCharacter.Add(state);
        }
        if(lstCharacter.Count > 0) DataManager.Instance.GameData.SetCharacterStates(lstCharacter);
        Debug.Log("Initialized character and skin states.");
    }

    public void LoadUnlockedStates()
    {
        if (DataManager.Instance.GameData.CharacterStates.Count == 0)
        {
            Debug.LogWarning("No character states found, initializing default.");
            SetupStartData();
            return;
        }

        foreach (var state in DataManager.Instance.GameData.CharacterStates)
        {
            Character character = characters.Find(c => c.id == state.id);
            if (character != null)
            {
                character.isUnlocked = state.isUnlocked;
                for (int i = 0; i < state.skinUnlocks.Count && i < character.skins.Count; i++)
                {
                    character.skins[i].isUnlocked = state.skinUnlocks[i];
                }
            }
        }
        Debug.Log("Loaded character and skin states.");
    }

    public int CharacterCount()
    {
        return characters.Count;
    }

    public Character GetCharacter(int selectedOption)
    {
        return characters[selectedOption];
    }

    public Character GetCharacterById(string id)
    {
        return characters.Find(c => c.id == id);
    }

    public void UnlockCharacter(string id)
    {
        var list = new List<CharacterState>(DataManager.Instance.GameData.CharacterStates);
        var state = list.Find(s => s.id == id);
        if (state != null)
        {
            state.isUnlocked = true;
            DataManager.Instance.GameData.SetCharacterStates(list);
            DataManager.Instance.GameData.Save();
        }
    }

    public void UnlockSkin(string characterId, int skinIndex)
    {
        var list = new List<CharacterState>(DataManager.Instance.GameData.CharacterStates);
        var state = list.Find(s => s.id == characterId);
        if (state != null)
        {
            state.isUnlocked = true;
            DataManager.Instance.GameData.SetCharacterStates(list);
            DataManager.Instance.GameData.Save();
        }
    }
}