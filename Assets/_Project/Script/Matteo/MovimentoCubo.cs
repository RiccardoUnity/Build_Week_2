using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoCubo : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;  
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(0f, 0f, speed);
        rb.velocity = velocity;
    }
}
