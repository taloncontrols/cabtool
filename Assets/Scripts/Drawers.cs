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

    void Start()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();

        while (cabinetService.containersLoaded == false || cabinetService.iosLoaded == false)
        {
            yield return new WaitForSeconds(1);
        }

        var containers = cabinetService.containers;
        if (containers != null && containers.Count > 0)
        {
            var id = cabinetService.containers[0].Id;
            var drawers = cabinetService.containers.Where(x => x.ParentId == id).ToList();
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
                    go.transform.localScale = Vector3.one * 45;
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
}
