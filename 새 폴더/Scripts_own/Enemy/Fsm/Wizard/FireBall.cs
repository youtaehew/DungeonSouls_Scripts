using System;
using System.Collections;
using System.Collections.Generic;
using Invector;
using Invector.vCharacterController;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private int power;



    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.FireAttackSound();
            vThirdPersonMotor player = other.GetComponent<vThirdPersonMotor>();
            vThirdPersonController playerController = other.GetComponent<vThirdPersonController>();
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
                player.TakeDamage(new vDamage(power, true), true);
            }
        }
    }
}
