using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanKnockback : MonoBehaviour
{
    private RaycastHit raycastHit;

    private void Start()
    {
        Physics.Raycast(transform.position, transform.forward, out raycastHit, 5f);
        print(raycastHit);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue);
    }
}
