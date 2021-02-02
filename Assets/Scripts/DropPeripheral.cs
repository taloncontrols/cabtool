using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class DropPeripheral : MonoBehaviour
{
    // Start is called before the first frame update
    Dropdown m_DropDown;
    public Text m_Text;
    public Dropdown m_DropDownData;
    void Start()
    {
        //Fetch the Dropdown GameObject
        m_DropDown = GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action      
       
        m_DropDown.ClearOptions();
        m_DropDown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(m_DropDown);
        });

        m_Text.text = "";

        StartCoroutine(waiter());

    }

    IEnumerator waiter()
    {
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();

        while (!cabinetService.IsReady())
        {
            yield return new WaitForSeconds(1);
        }

        var devices = cabinetService.devices;
        if (devices != null && devices.Count > 0)
        {
            var selected = cabinetService.devices.Where(x => !string.IsNullOrWhiteSpace(x.ClassName)).ToList();
            int numSelectors = selected.Count;

            if (numSelectors > 0)
            {
                var options = new List<Dropdown.OptionData>();
                foreach (var device in selected)
                {
                    options.Add(new Dropdown.OptionData(device.Name??device.Type));
                }

                m_DropDown.AddOptions(options);
                DropdownValueChanged(m_DropDown);
            }
        }
    }
    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {
        //m_Text.text = "New Value : " + change.value;
        m_Text.text = "";
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();

        var type = cabinetService.GetDeviceType(change.value);
        m_DropDownData.GetComponent<DropData>().LoadData(type);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
