using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Options : MonoBehaviour
{
    public TMP_InputField Framerate;
    public Image VSync;

    public GameObject Content;

    float Y = 0;
    float y = 0;

    void Start()
    {
        Content.transform.localPosition = new Vector2(150,y);

        Application.targetFrameRate = 90;
        QualitySettings.vSyncCount = 0;
        VSyncChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (Dictionary.MenuC.CurLayer == MenuController.Layers.Options)
        {
            Y += Input.GetAxis("Mouse ScrollWheel") * -340;
            Y = Mathf.Clamp(Y, 0, 1000);
        }
        print(Y);
        y = Mathf.Lerp(y, Y, Time.deltaTime * 10);

        Content.transform.localPosition = new Vector2(150,y);
    }

    public void OnFramerateChange()
    {
        if(Framerate.text != null)
        {
            int x = int.Parse(Framerate.text);
            x = Mathf.Clamp(x,30,300);
            Framerate.text = x.ToString();
            if(QualitySettings.vSyncCount == 0)
                Application.targetFrameRate = x;
        }
    }

    public void VSyncChange()
    {
        if(QualitySettings.vSyncCount == 0)
        {
            VSync.sprite = Dictionary.CheckOn;
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 80;
        }
        else
        {
            VSync.sprite = Dictionary.CheckOff;
            QualitySettings.vSyncCount = 0;
            int x = int.Parse(Framerate.text);
            x = Mathf.Clamp(x,30,300);
            Application.targetFrameRate = x;
        }
        print($"{Application.targetFrameRate}, {QualitySettings.vSyncCount}");
    }

    public void SlideTo(float point) => Y = point; 
}
