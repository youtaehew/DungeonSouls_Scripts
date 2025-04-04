using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Invector;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.AI;

public class Weapon : MonoBehaviour
{
    public int power;
    [SerializeField]private Collider weaponCol;
    [SerializeField] private bool isPlayer = false;

    private void Awake()
    {
        weaponCol = GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isPlayer)
        {
            if (other.CompareTag("Enemy"))
            {   
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();
                enemyStats.TakeDamage(power, false);
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                vThirdPersonMotor player = other.GetComponent<vThirdPersonMotor>();
                vThirdPersonController playerController = other.GetComponent<vThirdPersonController>();
                playerController.parryCol.enabled = false;
                if (player != null)
                {
                    if (playerController.isHoldingAttack)
                    {
                        playerController.chargingAttackEnd();
                    }
                    if (playerController.isHeal)
                    {
                        playerController.healEnd();
                        playerController.hideSword();
                    }
                    if (playerController.isParrying)
                    {
                        playerController.FinishParrying();
                    }

                    if (!playerController.isRolling)
                    {
                        player.TakeDamage(new vDamage(power, true));
                    }
                    
                    EnableCol_animationEvent(0);
                }
            }
        }
        
        
    }

    public void EnableCol_animationEvent(int enable)
    {
        weaponCol.enabled = Convert.ToBoolean(enable);
        
    }
    
    

    
}