using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Radials : MonoBehaviour
{
    // Start is called before the first frame update
    public int numSelectors = 3;
    public GameObject[] selectorArr;
    public GameObject selector; //selected in the editor
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

            var selected = cabinetService.GetIosRadials();
            numSelectors = selected.Count;

            if (numSelectors > 0)
            {
                selectorArr = new GameObject[numSelectors];
                for (int i = 0; i < numSelectors; i++)
                {
                    var sensor = selected[i];


                    GameObject go = Instantiate(selector, new Vector3((float)(Screen.width-120f), (float)((float)Screen.height-100f- i * 80f), 0f), Quaternion.identity) as GameObject;
                    
                    //value.GetComponent<SliderValueToText>().id = sensor.Id;

                    var sliderObject = go.transform.Find("RadialSliderImage").gameObject;

                    var name = sliderObject.transform.Find("Name").gameObject;
                    name.GetComponent<Text>().text = sensor.Name;
                    var valueObject = sliderObject.transform.Find("Value").gameObject;
                    valueObject.GetComponent<Text>().text = sensor.Value;


                    var image = sliderObject.GetComponent<Image>();
                    bool isHex = sensor.Range.StartsWith("0x");

                    float value = string.IsNullOrWhiteSpace(sensor.Value) ? 0f : (float)System.Convert.ToInt32(sensor.Value, isHex ? 16 : 10);
                    float maxValue = string.IsNullOrWhiteSpace(sensor.Range) ? 1f : (float)System.Convert.ToInt32(sensor.Range, isHex ? 16:10);
                    float angle = 0.5f* value / maxValue;

                    

                    var script = sliderObject.GetComponent<RadialSlider>();
                    script.id = sensor.Id;
                    script.type = sensor.Type;
                    script.maxValue = maxValue;
                    script.isHex = isHex;
                    script.Setup(sensor.Value);
                    image.fillAmount = angle;

                    image.color = Color.Lerp(Color.red, Color.green, angle * 2);

                    //go.transform.parent = GameObject.Find("Canvas").transform;
                    go.transform.SetParent(GameObject.Find("Canvas").transform);
                    //go.transform.localScale = Vector3.one * 45;
                    //go.transform.localRotation = Quaternion.identity;
                    selectorArr[i] = go;
                }
            }
        }
    }

    //void Start()
    //{
    //    selectorArr = new GameObject[numSelectors];
    //    for (int i = 0; i < numSelectors; i++)
    //    {
    //        GameObject go = Instantiate(selector, new Vector3((float)650.0f, (float)((float)i * 90f+150f), 0f), Quaternion.identity) as GameObject;


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
        var selected = cabinetService.GetIosRadials();
        int numSelectors = selected.Count;
        int i;
        for (i = 0; i < numSelectors; i++)
        {
            var s = selected[i];
            if (sensor.Id == s.Id)
            {
                found = true;
                break;
            }
        }

        if (!found) return;

        var go = selectorArr[i];

        var sliderObject = go.transform.Find("RadialSliderImage").gameObject;

        var name = sliderObject.transform.Find("Name").gameObject;
        name.GetComponent<Text>().text = sensor.Name;
        var valueObject = sliderObject.transform.Find("Value").gameObject;
        valueObject.GetComponent<Text>().text = sensor.Value;


        var image = sliderObject.GetComponent<Image>();
        bool isHex = sensor.Range.StartsWith("0x");

        float value = string.IsNullOrWhiteSpace(sensor.Value) ? 0f : (float)System.Convert.ToInt32(sensor.Value, isHex ? 16 : 10);
        float maxValue = string.IsNullOrWhiteSpace(sensor.Range) ? 1f : (float)System.Convert.ToInt32(sensor.Range, isHex ? 16 : 10);
        float angle = 0.5f * value / maxValue;



        var script = sliderObject.GetComponent<RadialSlider>();
        script.id = sensor.Id;
        script.type = sensor.Type;
        script.maxValue = maxValue;
        script.isHex = isHex;
        script.Setup(sensor.Value);
        image.fillAmount = angle;

        image.color = Color.Lerp(Color.red, Color.green, angle * 2);
    }
}
