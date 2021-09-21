using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffsetScrolling : MonoBehaviour {
    public float scrollSpeed;

    private Image renderer;
    private Vector2 savedOffset;

    void Start () {
        renderer = GetComponent<Image> ();
    }

    void Update () {
        float x = Mathf.Repeat (Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2 (x, 0);
        renderer.material.SetTextureOffset("_MainTex", offset);
    }
}