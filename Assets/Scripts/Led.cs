using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Led : MonoBehaviour
{
    // Start is called before the first frame update
    float repeatRate = 1f;
    bool isOn = false;
    bool isBlink = false;
    bool isFast = false;
    Color color;
    void Start()
    {
        //InvokeRepeating("Blink", 0, 0.5f);
    }
    public void SetValue(string svalue)
    {
        int value;
        if (!int.TryParse(svalue,out value))
        {
            return;
        }
        if (isBlink)
        {
            CancelInvoke("Blink");
        }
        repeatRate = 1f;
        isOn = false;
        isBlink = false;
        isFast = false;
        if (value == 0)
        {
            isOn = false;
        }
        else
        {
            isOn = true;
        }
        Enable(isOn);
        switch (value)
        {
            case 1:
                color = Color.red;
                break;
            case 2:
                color = Color.green;
                break;
            case 3:
                color = Color.blue;
                break;
            case 4:
                color = Color.white;
                break;
            case 5:
                color = Color.yellow;
                break;
            case 6:
                color = new Color(128, 0, 128);
                break;
            case 7:
                color = Color.red;
                isBlink = true;
                break;
            case 8:
                color = Color.green;
                isBlink = true;
                break;
            case 9:
                color = Color.blue;
                isBlink = true;
                break;
            case 10:
                color = Color.white;
                isBlink = true;
                break;            
            case 11:
                color = Color.red;
                isBlink = true;
                isFast = true;
                break;

        }
        if (isFast)
        {
            repeatRate = 0.5f;
        }
        Material mymat = GetComponent<Renderer>().material;
        mymat.EnableKeyword("_EMISSION");
        mymat.SetColor("_EmissionColor", color);
        mymat.SetColor("_Color", color);
        if (isBlink)
            InvokeRepeating("Blink", 0, repeatRate);
    }
    private void Enable(bool val)
    {
        var renderer = GetComponent<MeshRenderer>();
        renderer.enabled = val;
    }
    void  Blink()
    {
        var renderer = GetComponent<MeshRenderer>();
        renderer.enabled = !renderer.enabled;        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
