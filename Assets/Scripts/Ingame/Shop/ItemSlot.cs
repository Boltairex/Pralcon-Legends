using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image Image;

    private Item _item;
    public Item Item {
        get { return _item; }
        set
        {
            _item = value;
            if (_item == null)
            {
                Image.enabled = false;
            } else
            {
                Image.sprite = _item.ItemIcon;
                Image.enabled = true;
            }

        }



    }

    private void OnValidate()
    {
        if (Image == null)
        {
            Image = GetComponent<Image>();
        }
    }
}
