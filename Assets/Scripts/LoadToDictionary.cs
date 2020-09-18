using UnityEngine;

public class LoadToDictionary : MonoBehaviour
{
    public Sprite CheckOn;
    public Sprite CheckOff;
    public Sprite Logo;

    void Awake()
    {
        Dictionary.CheckOn = CheckOn;
        Dictionary.CheckOff = CheckOff;
        Dictionary.Logo = Logo;


        Destroy(this);
    }
}
