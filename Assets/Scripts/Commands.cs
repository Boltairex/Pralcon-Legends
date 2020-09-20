using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine;
using System.Reflection;
using System.IO;

public class Commands : MonoBehaviour
{
    Commands This;
    public List<Command> C = new List<Command>();

    void Start()
    {
        This = this;
        Dictionary.CS.CM = this;
    }

    public class Answer : Command
    {
        string CName = "ping";

        public void RunCommand(string Name)
        {
            if (CName == Name)
            {

            }
        }
    }
}
