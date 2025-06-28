using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public List<DataItem> listDataItem = new List<DataItem>();
}

[System.Serializable]
public class DataItem
{
    public string id;
    public string name;
    public Sprite icon; 
}
