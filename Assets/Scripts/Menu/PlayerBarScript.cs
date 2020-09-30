using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBarScript : MonoBehaviour
{
    public Image AvatarBack;
    public Image Avatar;

    public TMP_Text Nickname;
    public TMP_Text Desc;

    public void SetVariables(string name, string desc, Sprite avatar)
    {
        Avatar.sprite = avatar;
        Nickname.text = name;
        Desc.text = desc;
    }
    
    public void ColorChange(Color C) => AvatarBack.color = C;
}
