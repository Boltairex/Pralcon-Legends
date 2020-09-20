using UnityEngine;
using System.Collections.Generic;

public class VariableContainer : MonoBehaviour
{
    public List<Player> P = new List<Player>();

    void Start()
    {
        Dictionary.VC = this;
        DontDestroyOnLoad(this);
    }
}