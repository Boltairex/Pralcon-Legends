using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static Dictionary;

public class PRDiscordRPC : MonoBehaviour
{
    DiscordRpc.EventHandlers Handlers;
    DiscordRpc.RichPresence Presence;

    public string Nickname;
    public string UserID;

    string gamestate;

    [HideInInspector]
    public bool lplyInitiated;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        Presence = new DiscordRpc.RichPresence();

        Debug.Log("Discord: Now Initiating");

        Handlers = new DiscordRpc.EventHandlers();
        Handlers.readyCallback += ReadyCallback;
        Handlers.errorCallback += ErrorCallback;
        DiscordRpc.Initialize("655464840222998537", ref Handlers, true, "");
        StartCoroutine(UpdatePresence());
    }

    public void ReadyCallback(ref DiscordRpc.DiscordUser user)
    {
        Debug.Log(string.Format("Połączono z kontem discord {0}#{1}: {2}", user.username, user.discriminator, user.userId));
        Nickname = user.username;
        UserID = user.userId;
        StartCoroutine(LoadPlayerAvatar("cdn.discordapp.com/avatars/" + user.userId + "/" + user.avatar + ".png"));
    }

    public void ErrorCallback(int errorCode, string message)
    {
        Debug.Log(message);
    }

    void Update()
    {
        DiscordRpc.RunCallbacks();

        if(SceneManager.GetActiveScene().name == "Menu" && !InLobby)
        {
            Presence.largeImageKey = "logo";
            Presence.largeImageText = "Pralcon Legends";
            Presence.state = "In Menu, waiting for friends";
            Presence.details = "The best fanmade MOBA!";
            gamestate = "menu";
            Presence.startTimestamp = 0;

            Presence.smallImageKey = "plogo";
            Presence.smallImageText = "Pralcon Legends";
        }
        else if (SceneManager.GetActiveScene().name == "Menu" && InLobby)
        {
            Presence.largeImageKey = "logo";
            Presence.largeImageText = RoomName;
            Presence.state = Players.Length + "/"+ MaxPlayers +" Players";
            Presence.details = TeamName;
            gamestate = "lobby";
            Presence.startTimestamp = 0;

            Presence.smallImageKey = "plogo";
            Presence.smallImageText = "In Lobby";
        }
    }

    void OnDisable()
    {
        Debug.Log("Discord: shutdown");
        DiscordRpc.Shutdown();
    }

    IEnumerator LoadPlayerAvatar(string url) {
        print("initiating avatar");
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                print("working");
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                Dictionary.Avatar = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                lplyInitiated = true;
            }
        }
    }

    IEnumerator UpdatePresence()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            DiscordRpc.UpdatePresence(Presence);
        }
    }
}
