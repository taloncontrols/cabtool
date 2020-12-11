using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts;

public class RadialSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    bool isPointerDown = false;
    public string id;
    public string type;
    public float maxValue = 100;
    public bool isHex = false;
    // Called when the pointer enters our GUI component.
    // Start tracking the mouse
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine("TrackPointer");
    }

    // Called when the pointer exits our GUI component.
    // Stop tracking the mouse
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine("TrackPointer");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        //Debug.Log("mousedown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        //Debug.Log("mousedown");
    }

    // mainloop
    IEnumerator TrackPointer()
    {
        var ray = GetComponentInParent<GraphicRaycaster>();
        var input = FindObjectOfType<StandaloneInputModule>();

        var text = GetComponentInChildren<Text>();

        if (ray != null && input != null)
        {
            while (Application.isPlaying)
            {

                // TODO: if mousebutton down
                if (isPointerDown)
                {

                    Vector2 localPos; // Mouse position  
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, ray.eventCamera, out localPos);

                    // local pos is the mouse position.
                    float angle = (Mathf.Atan2(-localPos.y, localPos.x) * 180f / Mathf.PI + 180f) / 360f;
                    if (angle > 0.5f) angle = 0.5f;
                    GetComponent<Image>().fillAmount = angle;

                    GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, angle*2);

                    //text.text = ((int)(angle*360f)).ToString();
                    int value = (int)(angle * 2 * maxValue);
                    text.text = isHex ? $"0x{value:X4}" : value.ToString();
                    //Debug.Log(localPos+" : "+angle);	
                    Change(text.text);
                }

                yield return 0;
            }
        }
        else
            UnityEngine.Debug.LogWarning("Could not find GraphicRaycaster and/or StandaloneInputModule");
    }


    private void Change(string value)
    {
        GameObject Cupboard = GameObject.Find("Cupboard");
        var cabinetService = Cupboard.GetComponent<CabinetService>();
        cabinetService.Change(id, value);
        Setup(value);
    }

    public void Setup(string value)
    {
        if (type == "led")
        {
            GameObject light = GameObject.Find("Sphere");

            if (light != null)
            {
                var led = light.GetComponent<Led>();
                led.SetValue(value);
            }
        }
    }

}
