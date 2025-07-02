using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Character/CharacterDatabase")]
public class CharacterDatabaseSO : ScriptableObject
{
    public List<Character> characters;

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
}