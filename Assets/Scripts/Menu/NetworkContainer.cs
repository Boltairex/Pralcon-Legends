using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using static Dictionary;

public class NetworkContainer : NetworkBehaviour
{
    public GameObject BarPref;

    public bool Init;

    public BarSync LocalPlayerBar;
    public GameObject LocalPlayer;

    public int Range = 0;

    void Update()
    {
        if (!Team)
        { TeamName = Data.FirstTeamName; }
        else
        { TeamName = Data.SecondTeamName; }

        if (MenuC != null && SceneManager.GetActiveScene().name != "Menu")
        {
            Destroy(gameObject);
        }
    }

    public void ChangeColor()
    {
        if (Team && Online)
        {
            Team = false;
        }
        else if (!Team && Online)
        {
            Team = true;
        }

        if(LocalPlayer != null)
        {
            LocalPlayer.GetComponent<PlayersInfo>().CmdTeamSynchro(Team, LocalPlayer);
        }
        else
        { LocalPlayer = GameObject.Find("host"); }
    }

    public void CreatePlayer(GameObject Owner)
    {
        GameObject Player = Instantiate(BarPref);
        Player.transform.SetParent(MenuC.Content.transform);
        Player.GetComponent<RectTransform>().anchoredPosition = new Vector3(-210f, 90f + -140 * Range, 0);
        Range++;
        Player.transform.localScale = new Vector3(1, 1, 1);
        Owner.GetComponent<PlayersInfo>().Bar = Player;
        Player.GetComponent<BarSync>().Owner = Owner;
    }
}
