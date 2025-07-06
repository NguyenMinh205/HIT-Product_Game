using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Character/CharacterDatabase")]
public class CharacterDatabaseSO : ScriptableObject
{
    public List<Character> characters;

    public void SetupStartData()
    {
        if (PlayerPrefs.GetInt("IsSetupStartCharacterData", 0) == 1)
        {
            Debug.Log("PlayerPrefs already initialized, skipping default save.");
            return;
        }

        if (characters == null || characters.Count == 0)
        {
            Debug.LogWarning("Character list is null or empty in CharacterDatabaseSO!");
            return;
        }

        foreach (Character character in characters)
        {
            PlayerPrefs.SetInt($"CharacterUnlocked_{character.id}", character.isUnlocked ? 1 : 0);
            if (character.skins != null)
            {
                for (int i = 0; i < character.skins.Count; i++)
                {
                    PlayerPrefs.SetInt($"SkinUnlocked_{character.id}_{i}", character.skins[i].isUnlocked ? 1 : 0);
                }
            }
        }

        PlayerPrefs.SetInt("IsSetupStartCharacterData", 1);
        PlayerPrefs.Save();
        Debug.Log("Initialized PlayerPrefs with default character and skin states.");
    }

    public void LoadUnlockedStates()
    {
        if (characters == null || characters.Count == 0)
        {
            Debug.LogWarning("Character list is null or empty in CharacterDatabaseSO!");
            return;
        }

        foreach (Character character in characters)
        {
            character.isUnlocked = PlayerPrefs.GetInt($"CharacterUnlocked_{character.id}", 0) == 1;

            if (character.skins != null)
            {
                for (int i = 0; i < character.skins.Count; i++)
                {
                    character.skins[i].isUnlocked = PlayerPrefs.GetInt($"SkinUnlocked_{character.id}_{i}", 0) == 1;
                }
            }
        }
        Debug.Log("Loaded unlocked states from PlayerPrefs.");
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
        Character character = characters.Find(c => c.id == id);
        if (character == null)
        {
            Debug.LogWarning($"Character with ID {id} not found!");
            return;
        }
        character.isUnlocked = true;
        PlayerPrefs.SetInt($"CharacterUnlocked_{id}", 1);

        if (character.skins != null && character.skins.Count > 0)
        {
            character.skins[0].isUnlocked = true;
            PlayerPrefs.SetInt($"SkinUnlocked_{id}_0", 1);
            Debug.Log($"Unlocked default skin (index 0) for character: {character.name}");
        }

        PlayerPrefs.Save();
        Debug.Log($"Unlocked character: {character.name}");
    }

    public void UnlockSkin(string characterId, int skinIndex)
    {
        Character character = characters.Find(c => c.id == characterId);
        if (character == null)
        {
            Debug.LogWarning($"Character with ID {characterId} not found!");
            return;
        }
        if (skinIndex < 1 || skinIndex >= character.skins.Count)
        {
            Debug.LogWarning($"Invalid skin index {skinIndex} for character {character.name}!");
            return;
        }
        character.skins[skinIndex].isUnlocked = true;
        PlayerPrefs.SetInt($"SkinUnlocked_{characterId}_{skinIndex}", 1);
        PlayerPrefs.Save();
        Debug.Log($"Unlocked skin {skinIndex} for character: {character.name}");
    }
}