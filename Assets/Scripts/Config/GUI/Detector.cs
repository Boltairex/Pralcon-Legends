using UnityEngine;
using UnityEngine.EventSystems;

public class Detector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject MGUI;
    
    public void OnPointerEnter(PointerEventData data)
    {
        MGUI.GetComponent<GUISet>().IsOn = true;
        MGUI.GetComponent<GUISet>().CursorOn = gameObject.name;
    }
    public void OnPointerExit(PointerEventData data)
    {
        MGUI.GetComponent<GUISet>().IsOn = false;
        MGUI.GetComponent<GUISet>().CursorOn = null;
    }
}
