using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPart : MonoBehaviour
{
    // Start is called before the first frame update
    void OnMouseDown()
    {
        transform.parent.GetComponent<MoveBoard>().OnMouseDown();
    }
}
