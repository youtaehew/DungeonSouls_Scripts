using System;
using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public static EnemySound instance;
    
    [SerializeField] private AudioClip DeadSound;


    private void Awake()
    {
        instance = this;
    }
    
    public void EnemyDieSound()
    {
        EazySoundManager.PlaySound(DeadSound, false);
    }
    
    
}
