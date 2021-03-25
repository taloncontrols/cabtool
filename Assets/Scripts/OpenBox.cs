using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class OpenBox : MonoBehaviour
{
    public string containerId;
    public bool boxOpened;
    private bool coroutineAllowed;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        //boxOpened = false;
        coroutineAllowed = true;
        initialPosition = transform.position;
    }

    private void OnMouseDown()
    {
        Invoke("RunCoroutine", 0f);
    }

    private void RunCoroutine()
    {
        StartCoroutine("OpenThatDoor");
    }

    private IEnumerator OpenThatDoor()
    {
        coroutineAllowed = false;
        if (!boxOpened)
        {
            for (float i = 0f; i <= 0.8f; i += 0.1f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x - 0.1f,
                    transform.localPosition.y,
                    transform.localPosition.z);
                yield return new WaitForSeconds(0f);
            }
            boxOpened = true;
           
        }
        else
        {
            for (float i = 0.8f; i >= 0f; i -= 0.1f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + 0.1f,
                    transform.localPosition.y,
                    transform.localPosition.z);
                yield return new WaitForSeconds(0f);
            }
            //transform.position = initialPosition;
            boxOpened = false;
           
        }
        coroutineAllowed = true;
        Change();
    }

    private void  Change()
    {
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();       
        string value;
        if (boxOpened)
        {
            value = "0";
        }
        else
        {
            value = "1";
        }
        cabinetService.ChangeByContainerId(containerId, "closed", value);
    }
}
