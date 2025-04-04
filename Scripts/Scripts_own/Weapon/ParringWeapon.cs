using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParringWeapon : MonoBehaviour
{
    [SerializeField] private AudioClip parryAudioClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon"))
        { 
            GameManager.PlaySound(parryAudioClip,1);
            EnemyManager enemyManager = other.GetComponentInParent<EnemyManager>();
            Animator ani = other.GetComponentInParent<Animator>();
            Weapon weapon = other.GetComponent<Weapon>();
            
            weapon.EnableCol_animationEvent(0);
            enemyManager.isStun = true;
            ani.SetBool("isParring", true);
        }
    }
}
