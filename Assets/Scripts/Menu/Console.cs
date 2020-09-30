using UnityEngine;

public class Console : MonoBehaviour
{
    public Commands CM;

    public GameObject ConsoleObj;

    public TMPro.TMP_InputField Input;
    public TMPro.TMP_Text Output;

    void Awake()
    {
        Dictionary.CS = this;
    }

    public void InsertResult(string s) => Output.text += $"{s}\n";

    public void InsertCommand()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter)) CreateInfo();
    }

    public void CreateInfo()
    {

    }
}

public interface Command
{
    void RunCommand(string Name);
}