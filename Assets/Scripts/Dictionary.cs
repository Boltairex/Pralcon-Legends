using UnityEngine;

public class Dictionary : MonoBehaviour
{
    protected ulong id;
    protected string username;
    protected Sprite avatar;

    protected ConnectionType ct = ConnectionType.Unconnected;

    static public Dictionary This;

    static public NetworkBase NB;
    static public Mirror.NetworkManager NM;
    static public Mirror.TelepathyTransport TT;
    static public Mirror.Discovery.NetworkDiscovery ND;

    static public Console CS;

    static public VariableContainer VC;
    static public DRPC DS;
    static public MenuController MenuC;
    static public ServersScript ServS;
    static public int MaxPlayers = 0;

    static public bool ColorActive = false;

    static public ConnectionType CT
    {
        get{return This.ct;}
        set{This.ct = value;
        MenuC.DotUpdate();}
    }

    static public Color TeamOneColor;
    static public Color TeamTwoColor;

    static public bool Team;
    // false = 1 team
    // true = 2 team

    static public bool ESCTimer = false;
    int Timer = 0;

    void Awake()
    {
        This = this;

        TeamOneColor = new Color(1, 0, 0, 1);
        TeamTwoColor = new Color(0, 0, 1, 1);

        NM = this.GetComponent<Mirror.NetworkManager>();
        TT = this.GetComponent<Mirror.TelepathyTransport>();
        ND = this.GetComponent<Mirror.Discovery.NetworkDiscovery>();
    }

    public static Sprite Logo;
    public static Sprite CheckOn;
    public static Sprite CheckOff;

    void Update()
    {
        if (ESCTimer)
        {
            Timer++;
            if (Timer >= 30)
            {
                ESCTimer = false;
                Timer = 0;
            }
        }
    }

    public Sprite BytesToSprite(byte[] Bytes)
    {
        Texture2D T = new Texture2D(512,512);
        T.LoadImage(Bytes);
        return Sprite.Create(T,new Rect(0,0,512,512),Vector2.zero);
    }

    public enum GUIState
    {
        Off,
        MovingIn,
        In,
        MovingOut,
        Out,
        Staying
    }
    
    public enum ConnectionType
    {
        Unconnected,
        Connected,
        Joined
    }

    public static class LocalPlayer
    {
        public static ulong ID
        {
            get{return This.id;}
            set{This.id = value;}
        }
        public static string Username
        {
            get{return This.username;}
            set{This.username = value;
            MenuC.UpdatePlayerGUI();}
        }
        public static Sprite Avatar
        {
            get{return This.avatar;}
            set{This.avatar = value;
            MenuC.UpdatePlayerGUI();
            ConvertSpriteToByte();}
        }
        //Wykonać przy wysłaniu konwersję avataru na byte'y.

        static void ConvertSpriteToByte() => NetworkAvatar = Avatar.texture.EncodeToJPG();
        
        public static byte[] NetworkAvatar;
    }
}
