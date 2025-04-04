using System;
using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using Invector.vCharacterController;
using UnityEngine;

public class Touch : MonoBehaviour
{
    private vThirdPersonController player;
    private void Start()
    {
        player = FindObjectOfType<vThirdPersonController>();
        GameManager.instance.TouchSound(transform);
    }
    
}
