using System.Collections;
using System.Collections.Generic;
using TranDuc;
using UnityEngine;

public class SetUpCharacterStart : MonoBehaviour
{
    [SerializeField] CharacterDatabaseSO characterDatabaseSO;

    private void Start()
    {
        /*List<CharacterState> list = new List<CharacterState>(DataManager.Instance.GameData.CharacterStates);

        CharacterState state = list.Find(s => s.id == "Cha01");
        state.skinUnlocks[2] = false;

        DataManager.Instance.GameData.SetCharacterStates(list);
        DataManager.Instance.GameData.Save();*/
    }
}
