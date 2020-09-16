using UnityEngine;
using System;
using System.Collections.Generic;
public class ItemManager : MonoBehaviour
{
    [SerializeField]
    public List<Item> ItemBank = new List<Item>();
}

[Serializable]
public class Item
{
    public int ItemID;
    public string ItemName;
    public string ItemDescription;
    public int ItemCost;
    public Sprite ItemIcon;
}