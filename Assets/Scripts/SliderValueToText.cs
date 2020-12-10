using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    public Slider sliderUI;
    public string id;
    private Text textSliderValue;

    void Start()
    {
        //textSliderValue = GetComponent<Text>();
        if (sliderUI!=null)
        ShowSliderValue(sliderUI.value);
    }

    public void ShowSliderValue(float value)
    {
        textSliderValue = GetComponent<Text>();
        //string sliderMessage = " " + sliderUI.value;
        if (textSliderValue != null)
        {
            textSliderValue.text = value.ToString("0");
            Change(value.ToString("0"));
        }

    }

    private void Change(string value)
    {
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();        
        cabinetService.Change(id, value);
    }
}
