using UnityEngine;
using UnityEngine.UI;

public class ColorScript : MonoBehaviour
{
    public Image Red;
    public Image Green;
    public Image Blue;

    public Slider RedSlider;
    public Slider GreenSlider;
    public Slider BlueSlider;

    public bool Team;
    // 0
    // 1

    float StartVal = 0.2f;

    bool Lock;

    void Start()
    {
        RedSlider.minValue = StartVal;
        GreenSlider.minValue = StartVal;
        BlueSlider.minValue = StartVal;

        UpdateColors();
    }

    public void UpdateColors()
    {
        Lock = true;
        if (!Team)
        {
            RedSlider.value = Dictionary.TeamOneColor.r;
            GreenSlider.value = Dictionary.TeamOneColor.g;
            BlueSlider.value = Dictionary.TeamOneColor.b;

            Red.color = new Color(Dictionary.TeamOneColor.r, 0, 0, 1);
            Green.color = new Color(0, Dictionary.TeamOneColor.g, 0, 1);
            Blue.color = new Color(0, 0, Dictionary.TeamOneColor.b, 1);
        }
        else
        {
            RedSlider.value = Dictionary.TeamTwoColor.r;
            GreenSlider.value = Dictionary.TeamTwoColor.g;
            BlueSlider.value = Dictionary.TeamTwoColor.b;

            Red.color = new Color(Dictionary.TeamTwoColor.r, 0, 0, 1);
            Green.color = new Color(0, Dictionary.TeamTwoColor.g, 0, 1);
            Blue.color = new Color(0, 0, Dictionary.TeamTwoColor.b, 1);
        }

        Lock = false;
    }

    public void OnColorChange()
    {
        if (!Lock)
        {
            Red.color = new Color(RedSlider.value, 0, 0, 1);
            Green.color = new Color(0, GreenSlider.value, 0, 1);
            Blue.color = new Color(0, 0, BlueSlider.value, 1);

            if (!Team)
                Dictionary.TeamOneColor = new Color(RedSlider.value, GreenSlider.value, BlueSlider.value, 1);
            else
                Dictionary.TeamTwoColor = new Color(RedSlider.value, GreenSlider.value, BlueSlider.value, 1);
        }
    }
}
