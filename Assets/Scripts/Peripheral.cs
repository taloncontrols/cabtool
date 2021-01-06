using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Peripheral : MonoBehaviour
{
    // Start is called before the first frame update
    public Button m_ButtonGenerate;
    public Dropdown m_DropDown;
    public Text m_Text;
    void Start()
    {
        m_ButtonGenerate.onClick.AddListener(TaskOnClick);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TaskOnClick()
    {
        if (m_DropDown.options.Count == 0) return;

        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button, dropdown value=" + m_DropDown.value);
        //m_Text.text = "New Value : " + m_DropDown.options[m_DropDown.value].text;
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();
        var devices = cabinetService.devices;
        var deviceItem = devices[m_DropDown.value];
        var type = !string.IsNullOrEmpty(deviceItem.ClassName) ? deviceItem.ClassName : deviceItem.Type;
        switch (type)
        {
            case "SmartCardReaderPCSC":
              
                break;
            case "BarcodeReaderHID":
              
                break;
            case "FingerprintDPUruNet":
              
                break;

        }
    }
}
