using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class PerkDatabase : MonoBehaviour
{
    [SerializeField] private List<PerkBase> perks;

    private void Start()
    {
        perks = Resources.LoadAll<PerkBase>("PerkSO").ToList();
    }

    public PerkBase GetRandomItem()
    {
        return perks[Random.Range(0, perks.Count)];
    }

    public PerkBase GetPerkById(string id)
    {
        return perks.Find(perk => perk.id == id);
    }
}
