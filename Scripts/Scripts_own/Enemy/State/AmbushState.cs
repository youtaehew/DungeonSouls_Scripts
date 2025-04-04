using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : State
{
    public bool isSleeping;
    public float detectionRadius = 2;
    public string sleepAnimation;
    public string wakeAnimation;

    public LayerMask detectionLayer;

    public PursueTargetState pursueTargetState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatonManager enemyAni)
    {
        if(enemyManager.isWakeUp) return pursueTargetState;
        else if (isSleeping == false) return this;
        
        if (isSleeping && enemyManager.isInteracting == false)
        {
            enemyAni.PlayTargetAnimation(sleepAnimation, true);
        }

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - enemyManager.transform.position;
                float viewalbeAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                if (viewalbeAngle > enemyManager.minimumDetectionAngle &&
                    viewalbeAngle < enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                    isSleeping = false;
                    enemyAni.PlayTargetAnimation(wakeAnimation, true);
                }
            }
        }

        if (enemyManager.currentTarget && enemyManager.isWakeUp)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
        
    }
}
