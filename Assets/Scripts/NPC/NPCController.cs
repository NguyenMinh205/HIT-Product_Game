using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NPCData npcData;
    [SerializeField] private Transform npcParent;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Transform posNPCSpawn;
    private NPC npcCurrent;
    public void SetPosSpawnNPC(GameObject currentRoom)
    {
        Debug.Log("SetPosSpawnNPC");
        posNPCSpawn = currentRoom.transform.Find("NPCPosition");
    }
    public void SpawnNPC(string typeNPC)
    {
        GameObject npc = Instantiate(npcPrefab, posNPCSpawn.position, Quaternion.identity, npcParent);
        npcCurrent = npc.transform.Find("NPC_Prefab").GetComponent<NPC>();

        foreach(NPC_data npcData in npcData.npcList)
        {
            if (npcData.id == typeNPC)
            {
                npcCurrent.Init(npcData);
                break;
            }
        }
    }
    public void EndGame()
    {
        if (npcCurrent != null)
        {
            Destroy(npcCurrent.gameObject);
            npcCurrent = null;
        }
    }
}
