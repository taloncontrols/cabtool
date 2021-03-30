using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    public Slider sliderUI;
    public string id;
    private Text textSliderValue;
    private bool isBeingDragged = false;
    
    void Start()
    {
        //sliderUI = GetComponent<Slider>();

        if (sliderUI != null)
            ShowSliderValue(sliderUI.value);
    }

    public void OnEndDrag()
    {
        OnDrag();
        //Debug.Log("drag end");
        isBeingDragged = false;
    }

    public void OnBeginDrag()
    {

        //Debug.Log("drag start");

        isBeingDragged = true;
    }
    public void OnDrag()
    {
        //Debug.Log("drag");

        isBeingDragged = true;

        if (sliderUI != null)
            ShowSliderValue(sliderUI.value,true);
    }

    public void ShowSliderValue(float value, bool notify=false)
    {
      
        textSliderValue = GetComponent<Text>();
        //string sliderMessage = " " + sliderUI.value;
        if (textSliderValue != null)
        {
            textSliderValue.text = value.ToString("0");
            if (notify)
            Change(value.ToString("0"));
        }

    }

    public void ChangeSliderValue(string value)
    {
        sliderUI.value = string.IsNullOrWhiteSpace(value) ? 0f : (float)System.Convert.ToInt32(value);
        ShowSliderValue(sliderUI.value);
    }

    private void Change(string value)
    {
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();
        cabinetService.Change(id, value);
    }
}
