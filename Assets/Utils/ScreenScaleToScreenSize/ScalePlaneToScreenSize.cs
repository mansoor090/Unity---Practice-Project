using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlaneToScreenSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start () {
 
        var height = Camera.main.orthographicSize * 2.0;
        var width = height * Screen.width / Screen.height;
        transform.localScale = new Vector3 ((float)width, (float)height, 0.1f);
    }
}
