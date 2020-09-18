using UnityEngine.EventSystems;
using UnityEngine;

public class Server : MonoBehaviour, IPointerClickHandler
{
    public ServersScript SendTo;
    public int Index;
    public string Name;
    public string IP;
    public int Port;
    public int PlayersC;
    public bool NeedPassword;
    public int MaxP;

    void Start()
    {
        if(SendTo == null)
            SendTo = Dictionary.ServS;
    }

    public void OnPointerClick(PointerEventData data) => SendTo.ClickedOnServer(this);
}