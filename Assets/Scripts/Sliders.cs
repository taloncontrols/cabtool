﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    // Start is called before the first frame update
    public int numSelectors = 5;
    public GameObject[] selectorArr;
    public GameObject selector; //selected in the editor
    //public Slider[] sliderArr;
    CabinetService cabinetService;
    void Start()
    {
        GameObject Cupboard = GameObject.Find("Cupboard");
        cabinetService = Cupboard.GetComponent<CabinetService>();
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {

        cabinetService.OnChangeValue += ChangeValue;
        while (!cabinetService.IsReady())
        {
            yield return new WaitForSeconds(1);
        }

        var ios = cabinetService.ios;
        if (ios != null && ios.Count > 0)
        {
          
            var selected = cabinetService.GetIosSliders();
            numSelectors = selected.Count;

            if (numSelectors > 0)
            {
                selectorArr = new GameObject[numSelectors];
                //sliderArr = new Slider[numSelectors];
                for (int i = 0; i < numSelectors; i++)
                {
                    var sensor = selected[i];
                   

                    GameObject go = Instantiate(selector, new Vector3((float)100.0f, (float)((float)Screen.height - 50f - i * 30f ), 0f), Quaternion.identity) as GameObject;
                    var name = go.transform.Find("Name").gameObject;
                    name.GetComponent<Text>().text = sensor.Name;

                    var valueObject = go.transform.Find("Value").gameObject;
                    valueObject.GetComponent<SliderValueToText>().id = sensor.Id;

                    var SliderObject = go.transform.Find("SliderObject").gameObject;
                    var slider = SliderObject.GetComponent<Slider>();
                    slider.value = string.IsNullOrWhiteSpace(sensor.Value)?0f: (float)System.Convert.ToInt32(sensor.Value);
                    slider.maxValue= string.IsNullOrWhiteSpace(sensor.Range) ? 1f : (float)System.Convert.ToInt32(sensor.Range);

                    if (slider.maxValue == 1|| slider.maxValue == 2)
                    {
                        var rectTransform= SliderObject.GetComponent<RectTransform>();
                        float sliderSize = 50;
                        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 100, sliderSize* slider.maxValue);
                        
                    }

                    
                    //go.transform.parent = GameObject.Find("Canvas").transform;
                    go.transform.SetParent(GameObject.Find("Canvas").transform);
                    //go.transform.localScale = Vector3.one * 45;
                    //go.transform.localRotation = Quaternion.identity;
                    selectorArr[i] = go;
                    //sliderArr[i] = slider;
                }
            }
        }
    }

   
    //void Start()
    //{
    //    selectorArr = new GameObject[numSelectors];
    //    for (int i = 0; i < numSelectors; i++)
    //    {
    //        GameObject go = Instantiate(selector, new Vector3((float)100.0f, (float)((float)i * 50f+150f ), 0f), Quaternion.identity) as GameObject;


    //        go.transform.parent = GameObject.Find("Canvas").transform;
    //        //go.transform.localScale = Vector3.one * 45;
    //        //go.transform.localRotation = Quaternion.identity;
    //        selectorArr[i] = go;
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
    }
   
    public void ChangeValue(IoItem sensor)
    {
       
        bool found = false;
        var selected = cabinetService.GetIosSliders();
        int numSelectors = selected.Count;
        int i;
        for (  i = 0; i < numSelectors; i++)
        {
            var s = selected[i];
            if (sensor.Id == s.Id)
            {
                found = true;
                break;
            }
        }
        if (!found) return;

        //var slider = sliderArr[i];
        //slider.value = string.IsNullOrWhiteSpace(sensor.Value) ? 0f : (float)System.Convert.ToInt32(sensor.Value);

        var go = selectorArr[i];
        var valueObject = go.transform.Find("Value").gameObject;
        var sliderValueToText=valueObject.GetComponent<SliderValueToText>();
        sliderValueToText.ChangeSliderValue(sensor.Value);
        
        
    }
}

