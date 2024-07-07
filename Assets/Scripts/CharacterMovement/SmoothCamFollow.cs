using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float damp;
    private Vector3 vel = Vector3.zero;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, followTarget.position + new Vector3(0,0,-10), ref vel, damp);
    }
}
