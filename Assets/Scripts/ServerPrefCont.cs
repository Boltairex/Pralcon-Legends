using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServerPrefCont : MonoBehaviour
{
    public TMP_Text PlayersC;
    public TMP_Text Name;
    public Image CheckBox;

    public void UpdateInfo(string name, int P, int MaxP, bool Check)
    {
        if(Check)
            CheckBox.sprite = Dictionary.CheckOn;
        else
            CheckBox.sprite = Dictionary.CheckOff;

        Name.text = name;
        PlayersC.text = $"{P}/{MaxP}";
    }
}
