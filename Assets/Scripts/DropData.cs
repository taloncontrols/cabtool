using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class DropData : MonoBehaviour
{
    // Start is called before the first frame update
    Dropdown m_Dropdown;
    public Text m_Text;
    public Dropdown m_DropdownPeripheral;
    public Image m_Image;
    bool isReady;
    string[] smartCardData = new string[] { "7F-2C-4A-00", "7F-2C-4A-01", "7F-2C-4A-02" };
    string[] barCodeData = new string[] { "12345678901", "12345678902", "12345678903" };
    string[] fingerPrintData = new string[] { "fingerprint 1", "fingerprint 2", "fingerprint 3" };
    string m_type;
    void Start()
    {
        //Fetch the Dropdown GameObject
        m_Dropdown = GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.ClearOptions();
        m_Dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(m_Dropdown);
        });
        m_Image.enabled = false;
        m_Text.text = "";

        isReady = true;

    }


    public void LoadData(string type)
    {
        StartCoroutine(waiter(type));

       
    }
    //Ouput the new value of the Dropdown into Text

    IEnumerator waiter(string type)
    {
      
        while (isReady == false)
        {
            yield return new WaitForSeconds(0.1f);
        }

        
        m_type = type;
        m_Dropdown.ClearOptions();
        var options = new List<Dropdown.OptionData>();
        switch (type)
        {
            case "SmartCardReaderPCSC":
                foreach (var data in smartCardData)
                {
                    options.Add(new Dropdown.OptionData(data));
                }
                break;
            case "BarcodeReaderBS80":
                foreach (var data in barCodeData)
                {
                    options.Add(new Dropdown.OptionData(data));
                }
                break;
            case "FingerprintDPUruNet":
                foreach (var data in fingerPrintData)
                {
                    options.Add(new Dropdown.OptionData(data));
                }
                break;
        }


        m_Dropdown.AddOptions(options);
        DropdownValueChanged(m_Dropdown);
    }
        void DropdownValueChanged(Dropdown change)
    {
        //m_Text.text = "New Value : " + change.value;
        m_Text.text = "";

        bool active = true;
        if (m_type != "FingerprintDPUruNet")
        {
            active = false;
        }
        //GameObject myObject = GameObject.Find("ImageFP");
        //var myImage = myObject.GetComponent<UnityEngine.UI.Image>();
        //myImage.enabled = active;
        m_Image.enabled = active;
        // myObject.SetActive(active);
        if (m_type != "FingerprintDPUruNet")
        {
            return;
        }


        int width = 200;
        int height = 200;
        //byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/sprite.png");
        //string path = @"C:\data\sample"+change.value+".png";
        //string path = Application.persistentDataPath+"/sample" + change.value + ".png";
        string path = Application.dataPath + "/Image/sample" + change.value + ".png";
        if (!File.Exists(path)) return;
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Trilinear;
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.0f), 1.0f);

        m_Image.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
