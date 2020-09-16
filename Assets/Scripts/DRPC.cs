using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Discord;

public class DRPC : MonoBehaviour
{
    DiscordRpc.EventHandlers handlers;

    public Sprite Avatar;
    public string Nickname;
    public string UserID;

    DiscordRpc.RichPresence presence;

    string gamestate;

    [HideInInspector]
    public bool lplyInitiated;

    bool update20SecLoop;

    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        presence = new DiscordRpc.RichPresence();
        PlayerPrefs.SetInt("USE_DISCORD", 0);
        if (PlayerPrefs.GetInt("USE_DISCORD") == 1) //do zmiany wylaczone dla testów
        {
            Debug.Log("Discord: Now Initiating");

            handlers = new DiscordRpc.EventHandlers();
            handlers.readyCallback += ReadyCallback;
            handlers.errorCallback += ErrorCallback;
            DiscordRpc.Initialize("717791092300316672", ref handlers, true, "");
            if(update20SecLoop)
                StartCoroutine(YMHUpdatePresence());
        }
        else
            Debug.Log("Discord: Not initiating - Disabled");
    }

    public void ReadyCallback(ref DiscordRpc.DiscordUser user)
    {
        Debug.Log(string.Format("Połączono z kontem discord {0}#{1}: {2}", user.username, user.discriminator, user.userId));
        if (PlayerPrefs.GetInt("USE_DISCORD") == 1) {
            Nickname = user.username;
            UserID = user.userId;
            StartCoroutine(LoadPlayerAvatar("cdn.discordapp.com/avatars/" + user.userId + "/" + user.avatar + ".png"));
        }
    }

    public void ErrorCallback(int errorCode, string message)
    {
        Debug.Log(message + "|" + errorCode);
    }

    void Update()
    {
        DiscordRpc.RunCallbacks();
    }

    public void UpdateJoin(string partyId, int partySize, int partyMax, string joinSecret)
    {
        presence.partyId = partyId;
        presence.partySize = partySize;
        presence.partyMax = partyMax;
        presence.joinSecret = joinSecret;
        print("Join Settings Updated");
    }

    public void UpdatePresence(string largeKey, string largeText, string smallKey, string smallText, string state, string details, Int64 startTimestamp, Int64 endTimeStamp, string _gamestate)
    {
        presence.largeImageKey = largeKey;
        presence.largeImageText = largeText;
        presence.state = state;
        presence.details = details;
        gamestate = _gamestate;
        presence.startTimestamp = startTimestamp;
        presence.endTimestamp = endTimeStamp;

        presence.smallImageKey = smallKey;
        presence.smallImageText = smallText;
        if(!update20SecLoop)
            PushPresence();
        print("Updating Presence...");
    }

    void OnDisable()
    {
        DiscordRpc.Shutdown();
    }

    private void OnApplicationQuit()
    {
        DiscordRpc.Shutdown();
    }

    public void PushPresence()
    {
        DiscordRpc.UpdatePresence(presence);
    }

    IEnumerator LoadPlayerAvatar(string url) {
        print("initiating avatar");
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {

            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
                Debug.Log(uwr.error);

            else
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                Sprite avatar = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                Avatar = avatar;
                lplyInitiated = true;
            }
        }
    }

    IEnumerator YMHUpdatePresence()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            DiscordRpc.UpdatePresence(presence);
        }
    }
}
