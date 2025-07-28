using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "NPCData")]
public class NPCData : ScriptableObject
{
    public List<NPC_data> npcList = new List<NPC_data>();
}

[System.Serializable]
public class NPC_data
{
    public string id;
    public string name;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;
}
