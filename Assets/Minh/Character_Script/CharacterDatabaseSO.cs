using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Character/CharacterDatabase")]
public class CharacterDatabaseSO : ScriptableObject
{
    public List<Character> characters;

    public void SetupStartData()
    {
        GameData.Instance.LoadStartGameData();
        if (GameData.Instance.startData.characterStates.Count > 0)
        {
            Debug.Log("Character states already initialized, skipping default save.");
            return;
        }

        GameData.Instance.startData.characterStates.Clear();
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
            GameData.Instance.startData.characterStates.Add(state);
        }
        GameData.Instance.SaveStartGameData();
        Debug.Log("Initialized character and skin states.");
    }

    public void LoadUnlockedStates()
    {
        GameData.Instance.LoadStartGameData();
        if (GameData.Instance.startData.characterStates == null || GameData.Instance.startData.characterStates.Count == 0)
        {
            Debug.LogWarning("No character states found, initializing default.");
            SetupStartData();
            return;
        }

        foreach (var state in GameData.Instance.startData.characterStates)
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
        CharacterState state = GameData.Instance.startData.characterStates.Find(s => s.id == id);
        if (state != null)
        {
            state.isUnlocked = true;
        }
        if (state != null)
        {
            state.skinUnlocks[0] = true;
        }

        GameData.Instance.SaveStartGameData();
    }

    public void UnlockSkin(string characterId, int skinIndex)
    {
        CharacterState state = GameData.Instance.startData.characterStates.Find(s => s.id == characterId);
        if (state != null)
        {
            state.skinUnlocks[skinIndex] = true;
        }

        GameData.Instance.SaveStartGameData();
    }
}