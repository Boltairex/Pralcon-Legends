using UnityEngine;

public class Settings : MonoBehaviour
{
    public static KeyCode Q { get; protected set; }
    public static KeyCode W { get; protected set; }
    public static KeyCode E { get; protected set; }
    public static KeyCode R { get; protected set; }

    public static KeyCode B { get; protected set; }

    public static KeyCode N1 { get; protected set; }
    public static KeyCode N2 { get; protected set; }
    public static KeyCode N3 { get; protected set; }
    public static KeyCode N4 { get; protected set; }
    public static KeyCode N5 { get; protected set; }
    public static KeyCode N6 { get; protected set; }

    public static KeyCode SP1 { get; protected set; }
    public static KeyCode SP2 { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        Q = KeyCode.Q;
        W = KeyCode.W;
        E = KeyCode.E;
        R = KeyCode.R;

        B = KeyCode.B;

        N1 = KeyCode.Alpha1;
        N2 = KeyCode.Alpha2;
        N3 = KeyCode.Alpha3;
        N4 = KeyCode.Alpha4;
        N5 = KeyCode.Alpha5;
        N6 = KeyCode.Alpha6;

        SP1 = KeyCode.D;
        SP2 = KeyCode.F;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
