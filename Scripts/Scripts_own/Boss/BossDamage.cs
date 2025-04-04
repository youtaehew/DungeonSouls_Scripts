using System;
using System.Collections;
using System.Collections.Generic;
using EmeraldAI;
using Unity.VisualScripting;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    private GameObject Player;
    
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            // if (GetComponent<EmeraldAISystem>() != null)
            // {
            //     GetComponent<EmeraldAISystem>().Damage(15, EmeraldAISystem.TargetType.Player, Player.transform.root, 300);
            // }

            try
            {
                GameManager.instance.EnemyHit();
                GetComponent<EmeraldAISystem>().Damage(15, EmeraldAISystem.TargetType.Player, Player.transform.root, 300);
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
