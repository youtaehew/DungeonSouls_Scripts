using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Collider : MonoBehaviour
{
    private Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public void Detect()
    {
        collider.enabled = true;
    }
}
