using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveBoard : MonoBehaviour
{
   

    private bool boxOpened;
    private bool coroutineAllowed;
    private Vector3 initialPosition;
    private Vector3 finalPosition;

    private Vector3 initialLocalScale;
    private Vector3 finalLocalScale;
    private string id;
    // Start is called before the first frame update

    void Start()
    {
        
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
        
        var ios = cabinetService.ios;
        if (ios != null && ios.Count > 0)
        {
            Start2();
            var selected = cabinetService.ios.Where(x => x.ValueType != "string"  && x.Type == "cardiacBoardIn").ToList();
           

            if (selected.Count > 0)
            {
               
                for (int i = 0; i < 1; i++)
                {
                    var sensor = selected[i];
                    id = sensor.Id;
                    if (sensor.Value == "0")
                    {
                        boxOpened = true;
                        transform.position = finalPosition;
                        transform.localScale = finalLocalScale;
                    }
                    
                }
            }
        }
    }
    void Start2()
    {
        boxOpened = false;
        coroutineAllowed = true;
        initialPosition = transform.position;
        initialLocalScale = transform.localScale;
        finalLocalScale = new Vector3(initialLocalScale.x*0.05f, initialLocalScale.y * 0.05f, initialLocalScale.z * 0.05f);
        //var v3Pos = new Vector3(0.0f, 0.0f, 0.25f);
        Camera camera = Camera.main;
        //var plane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));
        //Ray ray;
        //float distance;
        //ray = Camera.main.ViewportPointToRay(new Vector3(0, 0, 0));
        //if (plane.Raycast(ray, out distance))
        //{
        //    Vector3 botLeft = ray.GetPoint(distance);
        //    finalPosition = botLeft;
        //}
        //finalPosition = camera.ViewportToWorldPoint(v3Pos);
        var depth = (transform.position.y - camera.transform.position.y);

        var upperLeftScreen = new Vector3(0, Screen.height, depth);
        var upperRightScreen = new Vector3(Screen.width, Screen.height, depth);
        var lowerLeftScreen = new Vector3(0, 0, depth);
        var lowerRightScreen = new Vector3(Screen.width, 0, depth);

        //Corner locations in world coordinates
        var upperLeft = camera.ScreenToWorldPoint(upperLeftScreen);
        var upperRight = camera.ScreenToWorldPoint(upperRightScreen);
        var lowerLeft = camera.ScreenToWorldPoint(lowerLeftScreen);
        var lowerRight = camera.ScreenToWorldPoint(lowerRightScreen);
        upperLeft.y = upperRight.y = lowerLeft.y = lowerRight.y = transform.position.y;

        Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        finalPosition = p;

        
    }

    public void OnMouseDown()
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
        float speed  = 5f;
        var dist = Vector3.Distance(initialPosition, finalPosition);
        if (!boxOpened)
        {
            //for (float i = 0f; i <= 1f; i += 0.1f)
            //{
            //    transform.localPosition = new Vector3(transform.localPosition.x - 0.1f,
            //        transform.localPosition.y,
            //        transform.localPosition.z);
            //    yield return new WaitForSeconds(0f);
            //}
            for (float i = 0.0f; i < 1.0f; i += (speed * Time.deltaTime) / dist)
            {
                transform.position = Vector3.Lerp(initialPosition, finalPosition, i);
                transform.localScale = Vector3.Lerp(initialLocalScale, finalLocalScale, i);

                yield return new WaitForSeconds(0f);
            }
           
            boxOpened = true;
        }
        else
        {
            //for (float i = 1f; i >= 0f; i -= 0.1f)
            //{
            //    transform.localPosition = new Vector3(transform.localPosition.x + 0.1f,
            //        transform.localPosition.y,
            //        transform.localPosition.z);
            //    yield return new WaitForSeconds(0f);
            //}
            //transform.position = initialPosition;

            for (float i = 0.0f; i < 1.0f; i += (speed * Time.deltaTime) / dist)
            {
                transform.position = Vector3.Lerp(finalPosition,initialPosition,  i);
                transform.localScale = Vector3.Lerp( finalLocalScale, initialLocalScale, i);
                yield return new WaitForSeconds(0f);
            }
            transform.position = initialPosition;
            boxOpened = false;
        }
        coroutineAllowed = true;
        string value = "1";
        if (boxOpened)
        {
            value = "0";
        }
        Change(value);
    }

    private void Change(string value)
    {
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();
        cabinetService.Change(id, value);
    }
}
