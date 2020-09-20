using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Dictionary;

public class MenuController : MonoBehaviour
{
    public GUIAnim Buttons;
    public GUIAnim Options;
    public GUIAnim Servers;

    [Header("PlayersBar")]
    public Image Avatar;
    public TMP_Text Text;
    public Image Dot;
    public RawImage ButtonSprite;

    protected Layers curlayer = Layers.Buttons;
    public Layers CurLayer
    {
        get { return curlayer; }
        set
        {
            curlayer = value;
            OnLayerChange(value);
        }
    }

    void Awake()
    {
        Dictionary.MenuC = this;
        UpdatePlayerGUI();
        DotUpdate();
        //DS.UpdatePresence("logo","None","","","In menu","",0,0,"");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CurLayer != Layers.ServerTab && !Dictionary.ColorActive && !ESCTimer)
        {
            ESCTimer = true;
            CurLayer = Layers.Buttons;
        }
    }

    void OnLayerChange(Layers LocalLayer)
    {
        print(LocalLayer);
        if (LocalLayer != Layers.ServerTab)
        {
            Buttons.Switch("Out");
            Servers.Switch("Out");
            Options.Switch("Out");

            if (LocalLayer == Layers.Buttons)
            {
                Buttons.gameObject.SetActive(true);
                Buttons.Switch("In");
            }
            else if (LocalLayer == Layers.Servers)
            {
                Servers.gameObject.SetActive(true);
                Servers.Switch("In");
            }
            else if (LocalLayer == Layers.Options)
            {
                Options.gameObject.SetActive(true);
                Options.Switch("In");
            }
        }
    }

    public void DotUpdate()
    {
        if(Dictionary.CT == ConnectionType.Unconnected)
            Dot.color = new Color(1,0,0,1);
        else if(Dictionary.CT == ConnectionType.Connected)
            Dot.color = new Color(0,0,1,1);
        else if(Dictionary.CT == ConnectionType.Joined)
            Dot.color = new Color(0,1,0,1);
    }

    public void UpdatePlayerGUI()
    {
        if(Dictionary.LocalPlayer.Avatar != null)
            Avatar.sprite = LocalPlayer.Avatar;
        else
            Avatar.sprite = Dictionary.Logo;

        if(Dictionary.LocalPlayer.Username != null)
            Text.text = LocalPlayer.Username;
        else
            Text.text = "???";
    }

    public enum Layers
    {
        Buttons,
        Servers,
        Options,
        ServerTab
    }
}