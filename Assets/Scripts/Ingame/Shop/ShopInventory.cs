using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] List<Item> itemsname;
    [SerializeField] Transform itemsParent;
    [SerializeField] Transform itemsnameParent;
    [SerializeField] ItemSlot[] itemSlots;
    [SerializeField] ItemSlotName[] itemnameSlots;

    private void OnValidate()
    {
        if (itemsParent != null)
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
            itemnameSlots = itemsnameParent.GetComponentsInChildren<ItemSlotName>();

        RefreshUI();
    }



    private void RefreshUI()
    {
        int i = 0;
        for (; i < items.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = items[i];
        }

        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }

        for (; i < itemsname.Count && i < itemnameSlots.Length; i++)
        {
            itemnameSlots[i].Item = itemsname[i];
        }

        for (; i < itemnameSlots.Length; i++)
        {
            itemnameSlots[i].Item = null;
        }
    }
}


