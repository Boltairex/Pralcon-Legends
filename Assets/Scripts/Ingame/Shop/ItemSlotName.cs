using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotName : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private Item _item;
    public Item Item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item == null)
            {
                text.enabled = false;
            }
            else
            {
                text.SetText(_item.ItemName);
                text.enabled = true;
            }

        }



    }

    private void OnValidate()
    {
        if (text)
        {
            text = gameObject.GetComponent<TextMeshProUGUI>();
        }
    }
}
