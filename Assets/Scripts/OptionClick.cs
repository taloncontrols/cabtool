using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
public class OptionClick : MonoBehaviour
{
    public Button m_ButtonOption;
    public Button m_ButtonOptionOK;
    public Button m_ButtonOptionCancel;
    public GameObject panelOption;
    public InputField m_InputField;
    // Start is called before the first frame update
    void Start()
    {
        panelOption.SetActive(false);
        m_ButtonOption.onClick.AddListener(TaskOnClick);

        m_ButtonOptionOK.onClick.AddListener(TaskOnClickOK);
        m_ButtonOptionCancel.onClick.AddListener(TaskOnClickCancel);
    }
    void TaskOnClick()
    {
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();
        m_InputField.text = cabinetService.targetUrl;
        panelOption.SetActive(true);
    }

    void TaskOnClickOK()
    {
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();
        cabinetService.targetUrl = m_InputField.text;
        panelOption.SetActive(false);
    }
    void TaskOnClickCancel()
    {
        panelOption.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
