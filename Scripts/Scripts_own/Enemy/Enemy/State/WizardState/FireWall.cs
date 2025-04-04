using System;
using System.Collections;
using System.Collections.Generic;
using Invector;
using Invector.vCharacterController;
using Unity.VisualScripting;
using UnityEngine;

public class FireWall : MonoBehaviour
{
   private ParticleSystem ps;
   List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
   private vThirdPersonMotor player;
   private vThirdPersonController playerController;
   private bool isHit;

   [SerializeField] private int Power;

   private void Start()
   {
      isHit = false;
      ps = GetComponent<ParticleSystem>();
      player = FindObjectOfType<vThirdPersonMotor>();
      playerController = FindObjectOfType<vThirdPersonController>();
      
   }

   private void OnParticleSystemStopped()
   {
      isHit = false;
   }

   private void OnParticleTrigger()
   {
      if (isHit == false)
      {
         isHit = true;
         ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

         foreach (var v in enter)
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
            GameManager.instance.FireAttackSound();
            player.TakeDamage(new vDamage(Power, true), true);
         }
      }
   }
}
