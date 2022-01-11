using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* FindTarget Script
 * Created by: Dereck Mills
 * 
 * Given a transform position, this UI element will hover over it
 /**/

public class FindTarget : MonoBehaviour
{
    //Public Variables
    public Transform
        _target;
    public Vector3
        _offset;

    //Private Variables
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = cam.WorldToScreenPoint(_target.position);
        transform.position = targetPos + _offset;
    }
}
