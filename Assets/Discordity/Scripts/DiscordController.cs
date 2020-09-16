using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Discord;
using System.Text;

public class DiscordController : MonoBehaviour {

	public Discord.Discord discord;

    public long AppClientId;
    public Text statusLabel;
    public Text userLabel;
    //Status
    public Button connectButton;
    public Button disconnectButton;
    //Activities player
    public InputField updateStateInput;
    public InputField updateDetailInput;
    public Button updateButton;
    public Button clearButton;
    //Activities friends
    public Dropdown dropDownFriends;
    public Button inviteToJoinButton;
    public Button inviteToSpectateButton;
    //Activities requests
    public Text requestLabel;
    public Button acceptRequest;
    public Button rejectRequest;

    private List<Relationship> relationships;

    // Use this for initialization
    void Start() {
        connectButton.onClick.AddListener(connect);
        updateButton.onClick.AddListener(updateActivity);
        clearButton.onClick.AddListener(clearActivity);
        inviteToJoinButton.onClick.AddListener(inviteToJoin);
        inviteToSpectateButton.onClick.AddListener(inviteToSpectate);
        disconnectButton.onClick.AddListener(disconnect);
    }

    void connect() {
        System.Environment.SetEnvironmentVariable("DISCORD_INSTANCE_ID", "0");
        //System.Environment.SetEnvironmentVariable("DISCORD_INSTANCE_ID", "1");
        discord = new Discord.Discord(AppClientId, (System.UInt64)Discord.CreateFlags.Default);
        UserManager userManager = discord.GetUserManager();
        userManager.OnCurrentUserUpdate += () => {
            var currentUser = userManager.GetCurrentUser();
            userLabel.text = currentUser.Username;
            disconnectButton.interactable = true;
            updateStateInput.interactable = true;
            updateDetailInput.interactable = true;
            updateButton.interactable = true;
            clearButton.interactable = true;
            dropDownFriends.interactable = true;
            inviteToJoinButton.interactable = true;
            inviteToSpectateButton.interactable = true;
        };

        var relationshipManager = discord.GetRelationshipManager();

        // Assign this handle right away to get the initial relationships update.
        // This callback will only be fired when the whole list is initially loaded or was reset

        // Wait for OnRefresh to fire to access a stable list
        // Filter a user's relationship list to be just friends
        // Use this list as your base
        relationships = new List<Relationship>();
        relationshipManager.OnRefresh += () => {
            relationshipManager.Filter((ref Relationship relationship) => {
                return relationship.Type == Discord.RelationshipType.Friend;
            });

            // Loop over all friends a user has.
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            for (var i = 0; i < relationshipManager.Count(); i++) {
                // Get an individual relationship from the list
                var r = relationshipManager.GetAt((uint)i);
                relationships.Add(r);
                
                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = r.User.Username;
                options.Add(option);
            }
            dropDownFriends.AddOptions(options);
        };

        var activityManager = discord.GetActivityManager();

        LobbyManager lobbyManager = discord.GetLobbyManager();
        // Received when someone accepts a request to join or invite.
        // Use secrets to receive back the information needed to add the user to the group/party/match
        activityManager.OnActivityJoin += secret => {
            Debug.Log("OnJoin " + secret);
            lobbyManager.ConnectLobbyWithActivitySecret(secret, (Discord.Result result, ref Discord.Lobby lobby) => {
                Debug.Log("Connected to lobby: " + lobby.Id);
                // Connect to voice chat, used in this case to actually know in overlay if your successful in connecting.
                lobbyManager.ConnectVoice(lobby.Id, (Discord.Result voiceResult) => {

                    if (voiceResult == Discord.Result.Ok) {
                        Debug.Log("New User Connected to Voice! Say Hello! Result: " + voiceResult);
                    } else {
                        Debug.Log("Failed with Result: " + voiceResult);
                    };
                });
                //Connect to given lobby with lobby Id
                lobbyManager.ConnectNetwork(lobby.Id);
                lobbyManager.OpenNetworkChannel(lobby.Id, 0, true);
                foreach (var user in lobbyManager.GetMemberUsers(lobby.Id)) {
                    //Send a hello message to everyone in the lobby
                    lobbyManager.SendNetworkMessage(lobby.Id, user.Id, 0,
                        Encoding.UTF8.GetBytes(string.Format("Hello, " + user.Username + "!")));
                }
                //Sends this off to a Activity callback named here as 'UpdateActivity' passing in the discord instance details and lobby details
                UpdateActivity(discord, lobby);
            });
        };

        void UpdateActivity(Discord.Discord discord, Discord.Lobby lobby) {
            //Creates a Static String for Spectate Secret.
            string discordSpectateSecret = "wdn3kvj320r8vk3";
            string spectateActivitySecret = discordSpectateSecret;
            var activity1 = new Discord.Activity
            {
                State = "Playing Co-Op",
                Details = "In a Multiplayer Match!",
                
                Assets =
            {
                LargeImage = "matchimage1",
                LargeText = "Inside the Arena!",
            },
                Party = {
                Id = lobby.Id.ToString(),
                Size = {
                    CurrentSize = lobbyManager.MemberCount(lobby.Id),
                    MaxSize = (int)lobby.Capacity,
                },
            },
                Secrets = {
                Spectate = spectateActivitySecret,
                Join = "123",
            },
                Instance = true,
            };

            activityManager.UpdateActivity(activity1, result => {
                Debug.Log("Updated to Multiplayer Activity: " + result);

                // Send an invite to another user for this activity.
                // Receiver should see an invite in their DM.
                // Use a relationship user's ID for this.
                // activityManager
                //   .SendInvite(
                //       364843917537050624,
                //       Discord.ActivityActionType.Join,
                //       "",
                //       inviteResult =>
                //       {
                //           Console.WriteLine("Invite {0}", inviteResult);
                //       }
                //   );
            });
        }
        statusLabel.text = "Connected.";
    }

    void disconnect() {
        //discord.Dispose(); bug
    }

    /// <summary>
    /// Update the user status in Discord.
    /// If your game doesn't have multiplayer, then you can remove the "Party" and "Secrets" attributes within the activity
    /// </summary>
    void updateActivity() {
        var stateText = updateStateInput.text;
        var stateDetails = updateDetailInput.text;
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity {
            State = stateText,
            Details = stateDetails,
            Timestamps =
            {
                Start = 5,
            },
                Assets =
            {
                    LargeImage = "thumbnail", // Larger Image Asset Key
                    LargeText = "foo largeImageText", // Large Image Tooltip
                    SmallImage = "thumbnail", // Small Image Asset Key
                    SmallText = "foo smallImageText", // Small Image Tooltip
            },
                Party =
            {
                    Id = "foo partyID",
                    Size = {
                        CurrentSize = 1,
                        MaxSize = 4,
                    },
            },
                 Secrets =
            {
                    Match = "foo matchSecret",
                    Join = "foo joinSecret",
                    Spectate = "foo spectateSecret",
            },
            Instance = true,
        };

        activityManager.UpdateActivity(activity, (result) => {
            if (result == Discord.Result.Ok) {
                Debug.Log("Update Success!");
            } else {
                Debug.Log("Update Failed");
            }
        });
    }

    void clearActivity() {
        var activityManager = discord.GetActivityManager();
        activityManager.ClearActivity((result) => {
            if (result == Discord.Result.Ok) {
                Debug.Log("Clear Success!");
            } else {
                Debug.Log("Clear Failed");
            }
        });
    }

    /// <summary>
    /// Invite other Discord users to join your in-game party.
    /// Needs a selected user in the dropdown.
    /// </summary>
    void inviteToJoin() {

        Relationship r = relationships[dropDownFriends.value];

        var userId = r.User.Id;
        discord.GetActivityManager().SendInvite(userId, Discord.ActivityActionType.Join, "Come play " + r.User.Username + "!", (result) => {
            if (result == Discord.Result.Ok) {
                statusLabel.text = "Invitation sent";
            } else {
                statusLabel.text = "Invitation sent Error";
            }
        });

    }

    /// <summary>
    /// Invite other Discord users to join in spectate mode.
    /// Needs a selected user in the dropdown.
    /// </summary>
    void inviteToSpectate() {

        Relationship r = relationships[dropDownFriends.value];

        var userId = r.User.Id;
        discord.GetActivityManager().SendInvite(userId, Discord.ActivityActionType.Spectate, "Come spectate " + r.User.Username + "!", (result) => {
            if (result == Discord.Result.Ok) {
                statusLabel.text = "Invitation sent";
            } else {
                statusLabel.text = "Invitation sent Error";
            }
        });

    }
    
    // Update is called once per frame
    void Update() {
        if (discord != null) {
            discord.RunCallbacks();
        }
		
	}
}