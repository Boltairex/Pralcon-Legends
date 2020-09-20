using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public ulong ID;
    public string Nickname;
    public Sprite Avatar;

    public bool Team;

    void Start() {
        Dictionary.VC.P.Add(this);    
        Dictionary.NB.UpdatePlayersCount();
    }

    void OnDestroy() {
        Dictionary.VC.P.Remove(this);
        Dictionary.NB.UpdatePlayersCount();
    }
}
