using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class PRDiscordRPC : MonoBehaviour
{
    DiscordRpc.EventHandlers handlers;

    public Sprite Avatar;
    public string Nickname;
    public string UserID;

    DiscordRpc.RichPresence presence;

    string gamestate;

    [HideInInspector]
    public bool lplyInitiated;

    void OnEnable()
    {
        presence = new DiscordRpc.RichPresence();

            Debug.Log("Discord: Now Initiating");

            handlers = new DiscordRpc.EventHandlers();
            handlers.readyCallback += ReadyCallback;
            handlers.errorCallback += ErrorCallback;
            DiscordRpc.Initialize("655464840222998537", ref handlers, true, "");
            StartCoroutine(UpdatePresence());
    }

    public void ReadyCallback(ref DiscordRpc.DiscordUser user)
    {
        Debug.Log(string.Format("Połączono z kontem discord {0}#{1}: {2}", user.username, user.discriminator, user.userId));
        Nickname = user.username;
        UserID = user.userId;
        StartCoroutine(LoadPlayerAvatar("cdn.discordapp.com/avatars/" + user.userId + "/" + user.avatar + ".png"));
        //print(user.avatar);

    }

    public void ErrorCallback(int errorCode, string message)
    {
        Debug.Log(message);
    }

    void Update()
    {
        DiscordRpc.RunCallbacks();

        if(SceneManager.GetActiveScene().name == "Menu" && gamestate != "menu")
        {
            presence.largeImageKey = "ymhicon";
            presence.largeImageText = "You're my Hope";
            presence.state = "In Menu";
            presence.details = "";
            gamestate = "menu";
            presence.startTimestamp = 0;

            presence.smallImageKey = "";
            presence.smallImageText = "";
        }
        else if (SceneManager.GetActiveScene().name == "Menu")
        {

            presence.largeImageKey = "ymhicon";
            presence.largeImageText = "In Lobby";
            presence.state = 0 + "/5 Players";
            presence.details = "In Lobby";
            gamestate = "lobby";
            presence.startTimestamp = 0;

            presence.smallImageKey = "";
            presence.smallImageText = "";
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
                Sprite avatar = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                Avatar = avatar;
                lplyInitiated = true;
            }
        }
    }

    IEnumerator UpdatePresence()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            DiscordRpc.UpdatePresence(presence);
        }
    }
}
