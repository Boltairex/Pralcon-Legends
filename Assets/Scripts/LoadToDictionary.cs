using UnityEngine;
using static Dictionary;

public class LoadToDictionary : MonoBehaviour
{
    public Sprite checkon;
    public Sprite checkoff;
    public Sprite logo;

    public GameObject playerbarpref;

    void Awake()
    {
        CheckOn = checkon;
        CheckOff = checkoff;
        Logo = logo;
        PlayerBarPref = playerbarpref;


        Destroy(this);
    }
}
