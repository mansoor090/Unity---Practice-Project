using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsLooking : MonoBehaviour
{


    public Transform Player;
    public float     dotVlaue  = 0;
    public bool      isLooking = false;
    void OnDrawGizmos()
    {
        Vector3 pt = Player.position;
        pt = pt.normalized;

        dotVlaue = Vector3.Dot(transform.forward, (Player.position   - transform.position).normalized);
        
        if (dotVlaue > 0f)
        {
            isLooking = true;
        }
        else
        {
            isLooking = false;
        }
        Gizmos.DrawLine(transform.position, pt);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
