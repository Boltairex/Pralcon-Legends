using UnityEngine;
using static Dictionary;

public class GUIAnim : MonoBehaviour
{
    public GameObject In;
    public GameObject Out;
    public int speed = 10;
    public bool DisableOnOut;

    public GUIState State;

    void Start()
    {
        if(State == GUIState.Out)
            this.transform.position = Out.transform.position;
        else if(State == GUIState.In)
            this.transform.position = In.transform.position;
        else if(State == GUIState.Off)
            this.gameObject.SetActive(false);
    }

    void Update()
    {
        if(State == GUIState.MovingIn)
        {
            this.transform.position = Vector2.Lerp(this.transform.position,In.transform.position,Time.deltaTime * speed);
            if(Vector2.Distance(this.transform.position,In.transform.position) < 2)
                State = GUIState.In;
        }
        else if(State == GUIState.MovingOut)
        {
            this.transform.position = Vector2.Lerp(this.transform.position,Out.transform.position,Time.deltaTime * speed);
            if(Vector2.Distance(this.transform.position,Out.transform.position) < 2)
                State = GUIState.Out;
        }
        else if(State == GUIState.Out && DisableOnOut)
        {
            State = GUIState.Off;
            this.gameObject.SetActive(false);
        }
    }

    public void Switch(string Message)
    {
        if(Message == "ToServer")
            MenuC.CurLayer = MenuController.Layers.Servers;
        else if(Message == "ToOptions")
            MenuC.CurLayer = MenuController.Layers.Options;
        else if(Message == "ToButtons")
            MenuC.CurLayer = MenuController.Layers.Buttons;
        else if(Message == "In")
            State = GUIState.MovingIn;
        else if(Message == "Out")
            State = GUIState.MovingOut;
        else if(Message == "ExitApp")
            Application.Quit();
        else if(State == GUIState.In || State == GUIState.MovingIn)
            State = GUIState.MovingOut;
        else if(State == GUIState.Out || State == GUIState.MovingOut)
            State = GUIState.MovingIn;
        else if(State == GUIState.Off)
            State = GUIState.MovingIn;
    }
}
