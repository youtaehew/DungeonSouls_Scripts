using System;
using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using UnityEngine;

public class CustomSound : MonoBehaviour
{
    public static CustomSound instance;

    [SerializeField] private AudioClip ChooseClip;
    [SerializeField] private AudioClip CancelClip;
    [SerializeField] private AudioClip MoveClip;

    private void Awake()
    {
        instance = this;
    }

    public void choosePlaySound()
    {
        EazySoundManager.PlaySound(ChooseClip);
    }
    public void cancelPlaySound()
    {
        EazySoundManager.PlaySound(CancelClip);
    }
    public void movePlaySound()
    {
        EazySoundManager.PlaySound(MoveClip);
    }
    
    
    
    
}
