using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drawers : MonoBehaviour
{
    // Start is called before the first frame update
    public int numSelectors = 5;
    public GameObject[] selectorArr;
    public GameObject selector; //selected in the editor
    public GameObject peripheralGO;
    CabinetService cabinetService;
    void Start()
    {
        //peripheralGO.SetActive(false);
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
        peripheralGO.SetActive(true);
        var containers = cabinetService.containers;
        if (containers != null && containers.Count > 0)
        {
            
            var drawers = cabinetService.GetDrawers();
            numSelectors = drawers.Count;

            if (numSelectors > 0)
            {
                selectorArr = new GameObject[numSelectors];
                for (int i = 0; i < numSelectors; i++)
                {
                    var drawer = drawers[i];
                    var containerId = drawer.Id;
                    var io = cabinetService.ios.FirstOrDefault(n => n.ContainerId == containerId);
                    float x = (float)0.5f;
                    float y = (float)(0.7f - i * 0.3f);
                    float z = 0.1f;
                    if (io.Value == "0")
                    {
                         x = x + 0.8f;
                        z = z - 0.06f;
                    }
                   
                    GameObject go = Instantiate(selector, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                    var openBox = go.GetComponent<OpenBox>();
                    openBox.containerId = drawer.Id;

                    if (io.Value == "0")
                    {
                        openBox.boxOpened = true;
                    }
                    go.transform.parent = GameObject.Find("Cupboard").transform;
                    //go.transform.localScale = Vector3.one * 45;
                    go.transform.localScale = new Vector3( 45,45,60);
                    go.transform.localRotation = Quaternion.identity;
                    selectorArr[i] = go;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeValue(IoItem sensor)
    {
        if (sensor.Type != "closed") return;
        bool found = false;
        var selected = cabinetService.GetDrawers();
        int numSelectors = selected.Count;
        int i;
        for (i = 0; i < numSelectors; i++)
        {
            var s = selected[i];
            if (sensor.ContainerId == s.Id)
            {
                found = true;
                break;
            }
        }
        if (!found) return;

        var drawer = selected[i];
        var go = selectorArr[i];

        float x = (float)0.5f;
        float y = (float)(0.7f - i * 0.3f);
        float z = 0.1f;
        if (sensor.Value == "0")
        {
            x = x + 0.8f;
            z = z - 0.06f;
        }
        var openBox = go.GetComponent<OpenBox>();
        openBox.containerId = drawer.Id;

        if (sensor.Value == "0")
        {
            openBox.boxOpened = true;
        }
        else
        {
            openBox.boxOpened = false;
        }
        go.transform.position = new Vector3(x, y, z);
        cabinetService.ChangeLocal(sensor.Id, sensor.Value);
    }
}
