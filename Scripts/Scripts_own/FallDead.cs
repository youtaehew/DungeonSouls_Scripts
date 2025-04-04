using System;
using System.Collections;
using System.Collections.Generic;
using Invector;
using Invector.vCharacterController;
using UnityEngine;

public class FallDead : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<vThirdPersonController>().TakeDamage(new vDamage(500));
        }
    }
}
